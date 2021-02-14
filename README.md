# Checkout

## Build and Test

This is a dotnet core application developed in VSCode. Build and run tests with `dotnet restore`, `dotnet build` and `dotnet test` as normal.

## Code Coverage

To generate code coverage in cobertura format, run  
`dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura`

In order to generate a formatted HTML report, you will need to install Report Generator if you don't have it already.  
`dotnet tool install -g dotnet-reportgenerator-globaltool`

Then run code coverage reports with  
`reportgenerator "-reports:Pricing.Tests\coverage.cobertura.xml" "-targetdir:Coverage" -reporttypes:Html`