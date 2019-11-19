using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LinkedMink.Web.Infastructure
{
    public abstract class BaseController<TController> : Controller
    {
        protected BaseController(BaseDbContext<ClientUser> context, ILogger<TController> logger)
        {
            DbContext = context;
            Logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentUserName = CurrentUserName;
            if (currentUserName != null)
                DbContext.ContextUser = currentUserName;

            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> ExecuteAsync<TReturn>(Func<Task<TReturn>> action)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await action.Invoke();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Message: {ex.Message}");
                    Logger.LogError($"Trace: {ex.StackTrace}");
                    throw;
                }
            }
            else
            {
                return Ok(new ServiceResult<object>
                {
                    Code = ServiceResultCode.ValidationError,
                    Errors = GetModelValidationErrors(ModelState)
                });
            }
        }

        protected string CurrentUserName =>
            HttpContext.User != null ? HttpContext.User.Identity.Name : null;

        protected long? CurrentUserId => HttpContext.User != null 
            ? (long?)long.Parse(HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub).Value) 
            : null;

        protected BaseDbContext<ClientUser> DbContext { get; }

        protected ILogger<TController> Logger { get; }

        private static object GetModelValidationErrors(ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(
                p => p.Key,
                p => p.Value.Errors.Select(e => e.ErrorMessage)
            );
        }
    }
}
