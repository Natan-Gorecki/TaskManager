# docker-create-registry.ps1

param(
  [string]$RegistryHost       = "localhost",
  [string]$RegistryName       = "docker-registry",
  [int]   $RegistryPort       = "5000",
  [switch]$OverrideExisting   = $false,
  [switch]$Help               = $false
)

# imports
. .\scripts\logger.ps1

$InformationPreference = "Continue"

function Show-Help {
  Log "Usage: .\docker-create-registry.ps1 [-Flags]"
  Log ""
  Log "Parameters:"
  Log "[-RegistryHost]        Hose which should point to registry. Unused with local dockers. Default value is 'localhost'."
  Log "[-RegistryName]        Name used by docker registry. Default value is 'docker-registry'."
  Log "[-RegistryPort]        Port used by docker registry. Default value is 5000."
  Log "[-OverrideExisting]    Specify if existing registry should be overriden. Default value is FALSE."
  Log "[-Help]                Displays this help message."
  return
}

if ($Help) {
  Show-Help
  return
}

Log "'docker-create-registry' configuration:"
Log "Host: $RegistryHost"
Log "Name: $RegistryName"
Log "Port: $RegistryPort"
Log "Should override existing: $OverrideExisting"

$RegistryIsRunning = docker container ls --filter "name=$RegistryName" --format "{{.Names}}" | Where-Object { $_ -eq $RegistryName }
$RegistryExists = docker container ls --all --filter "name=$RegistryName" --format "{{.Names}}" | Where-Object { $_ -eq $RegistryName }

if ($RegistryExists) {
  $RegistryStatusString = if ($RegistryIsRunning) { "Running" } else { "Exited" }
  Log "Found '$RegistryName' container with '$RegistryStatusString' status."
}

if ($OverrideExisting -and $RegistryExists) {
  Log "Removing existing container..."
  docker container rm --force $RegistryName
  $RegistryIsRunning = $false
  $RegistryExists = $false
}

if (-not $RegistryExists) {
  Log "Creating '$RegistryName' container..."
  docker run -d -p ${RegistryPort}:5000 --name $RegistryName registry:2.7
  return
}

if (-not $RegistryIsRunning) {
  Log "Starting '$RegistryName' container..."
  docker container start $RegistryName
}
