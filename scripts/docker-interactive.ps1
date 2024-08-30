# docker-interactive.ps1

param(
  [switch]$RunAll,
  [switch]$CreateRegistry,
  [switch]$BuildImages,
  [switch]$PushImages,
  [switch]$DeployContainers,
  [switch]$TaskManagerClient,
  [switch]$TaskManagerService,
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

function ShowWorkflowSteps {
  Log "Workflow steps:"
  Log "[-RunAll]                If you don't want to specify all steps separately, you can use default process."
  Log "[-CreateRegistry]        Create docker registry, if it doesn't exist."
  Log "[-BuildImages]           Build docker images."
  Log "[-PushImages]            Push docker images to registry."
  Log "[-DeployContainers]      Remove previous and deploy new docker containers."
}

function ShowWorkflowParameters {
  Log "Workflow parameters:"
  Log "[-Tag]                   Specify tag for docker images. Default value is DEV."
}

function ShowWorkflowServices {
  Log "Workflow services:"
  Log "[-TaskManagerClient]     Include TaskManager.Client."
  Log "[-TaskManagerService]    Include TaskManager.Service."
}

function ShowHelp {
  Log "Usage: .\docker-interactive.ps1 [-Flags]"
  Log ""
  ShowWorkflowSteps
  Log ""
  ShowWorkflowParameters
  Log ""
  ShowWorkflowServices
  Log ""
  Log "Additional flags:"
  Log "[-Help]                  Displays this help message."
  Log "[-RegistryHost]          Host which should point to registry. Default value is 'localhost'."
  Log "[-RegistryPort]          Port on which registry should operate. Default value is '5000'."
  return
}

if ($Help) {
  ShowHelp
  return
}

if (-not $WorkflowStepsDefined) {
  Log -Level Error "You need to specify at least one workflow step. Use [-Help] if you need more information."
  ShowWorkflowSteps
  return
}

if (-not $WorkflowServicesDefined) {
  Log -Level Error "You need to specify at leat one workflow service. Use [-Help] if you need more information."
  ShowWorkflowServices
  return
}

if (-not $Tag) {
  Log -Level Warn "Tag is not declared - DEV used."
  $Tag = "dev"
}

if ($RunAll -or $CreateRegistry) {
  .\scripts\docker-create-registry.ps1 -RegistryHost $RegistryHost -RegistryPort $RegistryPort
}

function ProcessService {
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

    $response = Invoke-RestMethod -Uri "http://${RegistryHost}:${RegistryPort}/v2/${DockerName}/tags/list"
    Log -Level Debug "Found the following versions in the docker registry: "
    Log -Level Debug $response.tags

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
      --restart unless-stopped `
      --name ${DockerName} `
      -p ${HttpPort}:80 `
      -p ${SslPort}:443 `
      ${DockerName}:$Tag
  }
}

if ($TaskManagerClient) {
  ProcessService `
    -ServiceName "TaskManager.Client" `
    -DockerName "task-manager-client" `
    -HttpPort 35020 `
    -SslPort 35021
}

if ($TaskManagerService) {
  ProcessService `
    -ServiceName "TaskManager.Service" `
    -DockerName "task-manager-service" `
    -HttpPort 35010 `
    -SslPort 35011
}