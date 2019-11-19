FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["LinkedMink.Web.Api.EnvironManager/LinkedMink.Web.Api.EnvironManager.csproj", "LinkedMink.Web.Api.EnvironManager/"]
COPY ["LinkedMink.Net.Message/LinkedMink.Net.Message.csproj", "LinkedMink.Net.Message/"]
COPY ["LinkedMink.Web.Infastructure/LinkedMink.Web.Infastructure.csproj", "LinkedMink.Web.Infastructure/"]
COPY ["LinkedMink.Data.Base/LinkedMink.Data.Base.csproj", "LinkedMink.Data.Base/"]
COPY ["LinkedMink.Base/LinkedMink.Base.csproj", "LinkedMink.Base/"]
COPY ["LinkedMink.Data.Domain.EnvironManager/LinkedMink.Data.Domain.EnvironManager.csproj", "LinkedMink.Data.Domain.EnvironManager/"]
COPY ["LinkedMink.Web.EnvironManager/LinkedMink.Web.EnvironManager.csproj", "LinkedMink.Web.EnvironManager/"]
COPY ["LinkedMink.Data.Domain.EnvironManager.PostgreSql/LinkedMink.Data.Domain.EnvironManager.PostgreSql.csproj", "LinkedMink.Data.Domain.EnvironManager.PostgreSql/"]
COPY ["LinkedMink.Data.Domain.EnvironManager.SqlServer/LinkedMink.Data.Domain.EnvironManager.SqlServer.csproj", "LinkedMink.Data.Domain.EnvironManager.SqlServer/"]
RUN dotnet restore "LinkedMink.Web.Api.EnvironManager/LinkedMink.Web.Api.EnvironManager.csproj"
COPY . .
WORKDIR "/src/LinkedMink.Web.Api.EnvironManager"
RUN dotnet build "LinkedMink.Web.Api.EnvironManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkedMink.Web.Api.EnvironManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV IS_CONTAINERIZED true

ENTRYPOINT ["dotnet", "LinkedMink.Web.Api.EnvironManager.dll"]