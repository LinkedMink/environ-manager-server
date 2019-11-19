 param (
    [Parameter(Mandatory=$true)][string]$name
 )

function AddMigrationUpdateDatabase ([string]$dbEngineName = "SqlServer", [bool]$shouldUpdate = $false) {
    $env:ASPNETCORE_CONNECT_STRING_KEY = $dbEngineName

    dotnet ef migrations add $name `
        -p "../LinkedMink.Data.Domain.EnvironManager.${dbEngineName}/LinkedMink.Data.Domain.EnvironManager.${dbEngineName}.csproj" `
        -s "../LinkedMink.Web.Api.EnvironManager/LinkedMink.Web.Api.EnvironManager.csproj" `
        -c "${dbEngineName}DbContext"

    If ($shouldUpdate) {
        dotnet ef database update `
            -p "../LinkedMink.Data.Domain.EnvironManager.${dbEngineName}/LinkedMink.Data.Domain.EnvironManager.${dbEngineName}.csproj" `
            -s "../LinkedMink.Web.Api.EnvironManager/LinkedMink.Web.Api.EnvironManager.csproj" `
            -c "${dbEngineName}DbContext"
    }
}

$dbEngines = @(
    [tuple]::Create("SqlServer", $true), 
    [tuple]::Create("PostgreSql", $false)
)

for ($i = 0; $i -lt $dbEngines.length; $i++) {
    AddMigrationUpdateDatabase $dbEngines[$i].Item1 $dbEngines[$i].Item2
}

$env:ASPNETCORE_CONNECT_STRING_KEY = "DefaultConnection"