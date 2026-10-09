# Requires PowerShell 7 on Windows. Runs the WotR Homebrew offline mechanics tests with the wotr-testing runner
# (external/wotr-testing). Initialize it first: git submodule update --init -- external/wotr-testing
[CmdletBinding()]
param(
    # Game root: a local installation or a snapshot. Defaults to vendor/wotr, then GamePath.props.
    [string]$WotrInputRoot,
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Debug',
    [int]$TimeoutMinutes = 15,
    # Optional dotnet test filter, e.g. "FullyQualifiedName~MainAttribute".
    [string]$Filter
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$runner = Join-Path $root 'external/wotr-testing/scripts/Invoke-WotrOfflineTests.ps1'
if (-not (Test-Path -LiteralPath $runner)) {
    throw '[ENV_MISSING] external/wotr-testing is not initialized. Run: git submodule update --init -- external/wotr-testing'
}
$arguments = @{
    Project = Join-Path $root 'tests/OfflineMechanics.Tests/OfflineMechanics.Tests.csproj'
    RepositoryRoot = $root
    ResultsDirectory = Join-Path $root 'artifacts/test-results/offline-mechanics'
    Configuration = $Configuration
    TimeoutMinutes = $TimeoutMinutes
}
if ($WotrInputRoot) { $arguments.WotrInputRoot = $WotrInputRoot }
if ($Filter) { $arguments.Filter = $Filter }
& $runner @arguments
exit $LASTEXITCODE
