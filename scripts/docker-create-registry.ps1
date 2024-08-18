# docker-create-registry.ps1

param(
  [string]$RegistryHost       = "localhost",
  [string]$RegistryName       = "docker-registry",
  [int]   $RegistryPort       = "5000",
  [switch]$OverrideExisting   = $false,
  [switch]$Help               = $false
)

$InformationPreference = "Continue"

function Show-Help {
  Write-Information "Usage: .\docker-create-registry.ps1 [-Flags]"
  Write-Information ""
  Write-Information "Parameters:"
  Write-Information "[-RegistryHost]        Hose which should point to registry. Unused with local dockers. Default value is 'localhost'."
  Write-Information "[-RegistryName]        Name used by docker registry. Default value is 'docker-registry'."
  Write-Information "[-RegistryPort]        Port used by docker registry. Default value is 5000."
  Write-Information "[-OverrideExisting]    Specify if existing registry should be overriden. Default value is FALSE."
  Write-Information "[-Help]                Displays this help message."
  return
}

if ($Help) {
  Show-Help
  return
}

Write-Information "'docker-create-registry' configuration:"
Write-Information "Host: $RegistryHost"
Write-Information "Name: $RegistryName"
Write-Information "Port: $RegistryPort"
Write-Information "Should override existing: $OverrideExisting"

$RegistryIsRunning = docker container ls --filter "name=$RegistryName" --format "{{.Names}}" | Where-Object { $_ -eq $RegistryName }
$RegistryExists = docker container ls --all --filter "name=$RegistryName" --format "{{.Names}}" | Where-Object { $_ -eq $RegistryName }

if ($RegistryExists) {
  $RegistryStatusString = if ($RegistryIsRunning) { "Running" } else { "Exited" }
  Write-Information "Found '$RegistryName' container with '$RegistryStatusString' status."
}

if ($OverrideExisting -and $RegistryExists) {
  Write-Information "Removing existing container..."
  docker container rm --force $RegistryName
  $RegistryIsRunning = $false
  $RegistryExists = $false
}

if (-not $RegistryExists) {
  Write-Information "Creating '$RegistryName' container..."
  docker run -d -p ${RegistryPort}:5000 --name $RegistryName registry:2.7
  return
}

if (-not $RegistryIsRunning) {
  Write-Information "Starting '$RegistryName' container..."
  docker container start $RegistryName
}
