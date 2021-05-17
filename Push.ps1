$artifacts = ".\artifacts"
if(Test-Path $artifacts) { Remove-Item $artifacts -Force -Recurse }

if ([string]::IsNullOrEmpty($Env:NUGET_API_KEY)) {
    Write-Host "NUGET_API_KEY is empty."
} else {
    dotnet pack $Env:PROJECT_PATH -c Release -o $artifacts --no-build
    dotnet nuget push $artifacts --source $Env:NUGET_URL --api-key $Env:NUGET_API_KEY
}