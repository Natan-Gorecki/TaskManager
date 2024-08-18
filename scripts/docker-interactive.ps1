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

$InformationPreference = "Continue"
$WorkflowStepsDefined = $CreateRegistry -or $BuildImages -or $PushImages -or $DeployContainers -or $RunAll
$WorkflowServicesDefined = $TaskManagerClient -or $TaskManagerService

function Show-Workflow-Steps {
  Write-Information "Workflow steps:"
  Write-Information "[-RunAll]                If you don't want to specify all steps separately, you can use default process."
  Write-Information "[-CreateRegistry]        Create docker registry, if it doesn't exist."
  Write-Information "[-BuildImages]           Build docker images."
  Write-Information "[-PushImages]            Push docker images to registry."
  Write-Information "[-DeployContainers]      Remove previous and deploy new docker containers."
}

function Show-Workflow-Parameters {
  Write-Information "Workflow parameters:"
  Write-Information "[-Tag]                   Specify tag for docker images. Default value is DEV."
}

function Show-Workflow-Services {
  Write-Information "Workflow services:"
  Write-Information "[-TaskManagerClient]     Include TaskManager.Client."
  Write-Information "[-TaskManagerService]    Include TaskManager.Service."
}

function Show-Help {
  Write-Information "Usage: .\docker-interactive.ps1 [-Flags]"
  Write-Information ""
  Show-Workflow-Steps
  Write-Information ""
  Show-Workflow-Parameters
  Write-Information ""
  Show-Workflow-Services
  Write-Information ""
  Write-Information "Additional flags:"
  Write-Information "[-Help]                  Displays this help message."
  Write-Information "[-RegistryHost]          Host which should point to registry. Default value is 'localhost'."
  Write-Information "[-RegistryPort]          Port on which registry should operate. Default value is '5000'."
  return
}

if ($Help) {
  Show-Help
  return
}

if (-not $WorkflowStepsDefined) {
  Write-Error "You need to specify at least one workflow step. Use [-Help] if you need more information."
  Show-Workflow-Steps
  return
}

if (-not $WorkflowServicesDefined) {
  Write-Error "You need to specify at leat one workflow service. Use [-Help] if you need more information."
  Show-Workflow-Services
  return
}

if (-not $Tag) {
  Write-Warning "Tag is not declared - DEV used."
  $Tag = "dev"
}

if ($RunAll -or $CreateRegistry) {
  .\scripts\docker-create-registry.ps1 -RegistryHost $RegistryHost -RegistryPort $RegistryPort
}

function Process-Service {
  param(
    [string]$ServiceName,
    [string]$DockerName
  )

  Write-Information ""
  Write-Warning "Processing $ServiceName..."

  if ($RunAll -or $BuildImages) {
    Write-Information "Building docker image..."
    docker build . `
    --file .\$ServiceName\Dockerfile `
    --tag ${DockerName}:$Tag
    # TODO --build-arg "BUILD_CONFIGURATION=Debug"
  }

  if ($RunAll -or $PushImages) {
    Write-Information "Pushing image to registry..."
    docker tag ${DockerName}:$Tag ${RegistryHost}:$RegistryPort/${DockerName}:$Tag
    docker push ${RegistryHost}:$RegistryPort/${DockerName}:$Tag
  }
}

if ($TaskManagerClient) {
  Process-Service `
    -ServiceName "TaskManager.Client" `
    -DockerName "task-manager-client"
}

if ($TaskManagerService) {
  Process-Service `
    -ServiceName "TaskManager.Service" `
    -DockerName "task-manager-service"
}