dotnet clean -c Release
dotnet build -c Release
dotnet test  -c Release --no-build -l trx --verbosity=normal