
function CheckError {
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error: $LASTEXITCODE"
        exit $LASTEXITCODE
    }
}
#Builds and pushes the docker image to the registry

$tag = 'jeremysv/azure-service-bus-emulator-healthcheck:latest'

try {
    Push-Location -Path $PSScriptRoot -StackName "BuildAndPush"

    dotnet test -c Release "ServiceBusEmulator.Healthcheck.Tests/ServiceBusEmulator.Healthcheck.Tests.csproj" ; CheckError
    docker build -t $tag -f "ServiceBusEmulator.Healthcheck/Dockerfile" "ServiceBusEmulator.Healthcheck" ; CheckError
    docker push $tag ; CheckError
}
finally {
    Pop-Location -StackName "BuildAndPush"
}