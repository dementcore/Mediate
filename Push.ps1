if ([string]::IsNullOrEmpty($Env:NUGET_API_KEY)) {
    Write-Host "NUGET_API_KEY is empty."
} else {
    dotnet pack $Env:PROJECT_PATH -c Release -o Temp --no-build
    dotnet nuget push Temp/* --source $Env:NUGET_URL --api-key $Env:NUGET_API_KEY
}