if ([string]::IsNullOrEmpty($Env:NUGET_API_KEY)) {
    Write-Host "NUGET_API_KEY is empty."
} else {
    dotnet pack $Env:PROJECT_PATH -c Release -o Temp --no-build
      Get-ChildItem Temp\ -Filter "*.nupkg" | ForEach-Object {
        Write-Host "Pushing $($_.Name)"
        dotnet nuget push $_ --skip-duplicate --source $Env:NUGET_URL --api-key $Env:NUGET_API_KEY
        if ($lastexitcode -ne 0) {
            throw ("Exec: " + $errorMessage)
        }
    }
}