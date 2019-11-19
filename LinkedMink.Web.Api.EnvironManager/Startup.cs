using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Base.Repositories;
using LinkedMink.Data.Domain.EnvironManager;
using LinkedMink.Data.Domain.EnvironManager.PostgreSql;
using LinkedMink.Data.Domain.EnvironManager.Repositories;
using LinkedMink.Data.Domain.EnvironManager.Repositories.Concrete;
using LinkedMink.Net.Message;
using LinkedMink.Web.EnvironManager.Services;
using LinkedMink.Web.Infastructure.Options;
using LinkedMink.Web.Infastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LinkedMink.Web.Api.EnvironManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationOptions = Configuration.GetSection("Authentication");

            // Options
            services.Configure<AuthenticationOptions>(authenticationOptions);
            services.Configure<IdentityOptions>(authenticationOptions.GetSection("IdentityOptions"));
            services.Configure<EmailOptions>(Configuration.GetSection("EmailService"));

            // Infastructure Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();

            // Environ Manager Web API Services
            services.AddScoped<IHardwareDeviceStatusService, HardwareDeviceStatusService>();

            // Environ Manager Shared
            services.AddScoped(typeof(IRepository<>), typeof(EnvironManagerRepository<>));
            services.AddScoped<ILogEntryRepository, LogEntryRepository>();

            // Entity Framework
            var connectionStringKey = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECT_STRING_KEY");
            if (connectionStringKey == null)
                connectionStringKey = "DefaultConnection";

            if (connectionStringKey == "PostgreSql")
            {
                AddEntityFrameworkProvider<PostgreSqlDbContext>(
                    (options, s) => options.UseNpgsql(s), () => services.AddEntityFrameworkNpgsql());
            }
            else
            {
                AddEntityFrameworkProvider<SqlServerDbContext>(
                    (options, s) => options.UseSqlServer(s), () => services.AddEntityFrameworkSqlServer());
            }

            // .NET Core Services
            services.AddHttpClient();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = authenticationOptions["Issuer"],
                        ValidAudience = authenticationOptions["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(authenticationOptions["SigningKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Default", defaultPolicy);
                options.DefaultPolicy = defaultPolicy;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(defaultPolicy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors();

            void AddEntityFrameworkProvider<TContext>(
                Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> addContextFunction,
                Action addEfFunction = null) where TContext : EnvironManagerDbContext
            {
                addEfFunction?.Invoke();

                services.AddDbContext<TContext>(
                    options => addContextFunction(
                        options, Configuration.GetConnectionString(connectionStringKey)));

                services
                    .AddIdentity<ClientUser, ClientRole>()
                    .AddEntityFrameworkStores<TContext>()
                    .AddDefaultTokenProviders();

                services.AddScoped<BaseDbContext<ClientUser>, TContext>();
                services.AddScoped<EnvironManagerDbContext, TContext>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var allowedOrigins = Configuration.GetSection("AllowedOrigins")
                .GetChildren()
                .Select(c => c.Value).ToArray();

            app.UseCors(builder => builder
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
