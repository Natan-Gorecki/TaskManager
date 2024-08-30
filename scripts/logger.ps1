# logger.ps1

function Log {
  param(
    [string]$Message,
    [string]$Level = "Info"
  )

  $foregroundColor = [Console]::ForegroundColor
  $backgroundColor = [Console]::BackgroundColor

  $logForegroundColor = $foregroundColor
  $logBackgroundColor = $backgroundColor

  if ($Level -eq "Verb" -or $Level -eq "Verbose") {
    $logForegroundColor = "DarkGray"
    $logBackgroundColor = "Black"
  } elseif ($Level -eq "Debug") {
    $logForegroundColor = "DarkGray"
    $logBackgroundColor = "Black"
  } elseif ($Level -eq "Info" -or $Level -eq "Information") {
    $logForegroundColor = "White"
    $logBackgroundColor = "Black"
  } elseif ($Level -eq "Warn" -or $Level -eq "Warning") {
    $logForegroundColor = "Yellow"
    $logBackgroundColor = "Black"
  } elseif ($Level -eq "Error") {
    $logForegroundColor = "Red"
    $logBackgroundColor = "Black"
  } elseif ($Level -eq "Fatal") {
    $logForegroundColor = "White"
    $logBackgroundColor = "Red"
  }

  [Console]::ForegroundColor = $logForegroundColor
  [Console]::BackgroundColor = $logBackgroundColor
  Write-Output $Message

  [Console]::ForegroundColor = $foregroundColor
  [Console]::BackgroundColor = $backgroundColor
}