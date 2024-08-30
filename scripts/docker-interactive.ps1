# docker-interactive.ps1

param(
  [switch]$RunAll = $true,
  [switch]$CreateRegistry,
  [switch]$BuildImages,
  [switch]$PushImages,
  [switch]$DeployContainers,
  [switch]$TaskManagerClient,
  [switch]$TaskManagerService = $true,
  [switch]$Help,
  [string]$Tag,
  [string]$RegistryHost = "localhost",
  [int]   $RegistryPort = "5000"
)

# imports
. .\scripts\logger.ps1

$InformationPreference = "Continue"
$WorkflowStepsDefined = $CreateRegistry -or $BuildImages -or $PushImages -or $DeployContainers -or $RunAll
$WorkflowServicesDefined = $TaskManagerClient -or $TaskManagerService

function Show-Workflow-Steps {
  Log "Workflow steps:"
  Log "[-RunAll]                If you don't want to specify all steps separately, you can use default process."
  Log "[-CreateRegistry]        Create docker registry, if it doesn't exist."
  Log "[-BuildImages]           Build docker images."
  Log "[-PushImages]            Push docker images to registry."
  Log "[-DeployContainers]      Remove previous and deploy new docker containers."
}

function Show-Workflow-Parameters {
  Log "Workflow parameters:"
  Log "[-Tag]                   Specify tag for docker images. Default value is DEV."
}

function Show-Workflow-Services {
  Log "Workflow services:"
  Log "[-TaskManagerClient]     Include TaskManager.Client."
  Log "[-TaskManagerService]    Include TaskManager.Service."
}

function Show-Help {
  Log "Usage: .\docker-interactive.ps1 [-Flags]"
  Log ""
  Show-Workflow-Steps
  Log ""
  Show-Workflow-Parameters
  Log ""
  Show-Workflow-Services
  Log ""
  Log "Additional flags:"
  Log "[-Help]                  Displays this help message."
  Log "[-RegistryHost]          Host which should point to registry. Default value is 'localhost'."
  Log "[-RegistryPort]          Port on which registry should operate. Default value is '5000'."
  return
}

if ($Help) {
  Show-Help
  return
}

if (-not $WorkflowStepsDefined) {
  Log -Level Error "You need to specify at least one workflow step. Use [-Help] if you need more information."
  Show-Workflow-Steps
  return
}

if (-not $WorkflowServicesDefined) {
  Log -Level Error "You need to specify at leat one workflow service. Use [-Help] if you need more information."
  Show-Workflow-Services
  return
}

if (-not $Tag) {
  Log -Level Warn "Tag is not declared - DEV used."
  $Tag = "dev"
}

if ($RunAll -or $CreateRegistry) {
  .\scripts\docker-create-registry.ps1 -RegistryHost $RegistryHost -RegistryPort $RegistryPort
}

function Process-Service {
  param(
    [string]$ServiceName,
    [string]$DockerName,
    [string]$HttpPort,
    [string]$SslPort
  )

  Log ""
  Log -Level Warn "Processing $ServiceName..."

  if ($RunAll -or $BuildImages) {
    Log -Level Warn "Building docker image..."
    docker build . `
      --file .\$ServiceName\Dockerfile `
      --tag ${DockerName}:$Tag
    # TODO --build-arg "BUILD_CONFIGURATION=Debug"
  }

  if ($RunAll -or $PushImages) {
    Log -Level Warn "Pushing image to registry..."
    docker tag ${DockerName}:$Tag ${RegistryHost}:$RegistryPort/${DockerName}:$Tag
    docker push ${RegistryHost}:$RegistryPort/${DockerName}:$Tag
  }

  if ($RunAll -or $DeployContainers) {
    Log -Level Warn "Starting container..."

    $ContainerIsRunning = docker container ls --filter "name=$DockerName" --format "{{.Names}}" | Where-Object { $_ -eq $DockerName }
    if ($ContainerIsRunning) {
      Log "Stopping '$DockerName' container..."
      docker container stop $DockerName
    }

    $ContainerExists = docker container ls --all --filter "name=$DockerName" --format "{{.Names}}" | Where-Object { $_ -eq $DockerName }
    if ($ContainerExists) {
      Log "Removing '$DockerName' container..."
      docker container rm $DockerName
    }

    docker run -d `
      --name ${DockerName} `
      -p ${HttpPort}:80 `
      -p ${SslPort}:443 `
      ${DockerName}:$Tag
  }
}

if ($TaskManagerClient) {
  Process-Service `
    -ServiceName "TaskManager.Client" `
    -DockerName "task-manager-client" `
    -HttpPort 35020 `
    -SslPort 35021
}

if ($TaskManagerService) {
  Process-Service `
    -ServiceName "TaskManager.Service" `
    -DockerName "task-manager-service" `
    -HttpPort 35010 `
    -SslPort 35011
}