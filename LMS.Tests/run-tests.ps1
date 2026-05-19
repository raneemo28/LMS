# ==============================================================
#  run-tests.ps1   (PowerShell – Windows)
#
#  PURPOSE: Runs all LMS integration tests and produces:
#    1. A terminal summary (pass / fail per test)
#    2. A .trx XML file for CI tools (Azure DevOps, GitHub Actions)
#    3. An HTML coverage report  → TestResults\Coverage\html\index.html
#
#  USAGE (from solution root in PowerShell):
#    .\LMS.Tests\run-tests.ps1
# ==============================================================

$ErrorActionPreference = "Stop"

$ResultsDir  = "TestResults"
$CoverageDir = "$ResultsDir\Coverage"

Write-Host ""
Write-Host "┌─────────────────────────────────────────────────┐" -ForegroundColor Cyan
Write-Host "│           LMS Integration Test Runner           │" -ForegroundColor Cyan
Write-Host "└─────────────────────────────────────────────────┘" -ForegroundColor Cyan
Write-Host ""

# ── 1. Run tests ─────────────────────────────────────────────────────────────
Write-Host "▶  Running tests..." -ForegroundColor Yellow

dotnet test LMS.Tests\LMS.Tests.csproj `
    --configuration Release `
    --logger "trx;LogFileName=TestResults.trx" `
    --logger "console;verbosity=detailed" `
    --results-directory $ResultsDir `
    /p:CollectCoverage=true `
    /p:CoverageOutputFormat=cobertura `
    /p:CoverageOutput="$CoverageDir\coverage.cobertura.xml" `
    /p:Exclude="[*Tests*]*" `
    --no-restore

# ── 2. Generate HTML coverage report ─────────────────────────────────────────
Write-Host ""
Write-Host "▶  Generating HTML coverage report..." -ForegroundColor Yellow

try {
    dotnet tool run reportgenerator `
        -reports:"$CoverageDir\coverage.cobertura.xml" `
        -targetdir:"$CoverageDir\html" `
        -reporttypes:"Html;Badges"
} catch {
    Write-Host "⚠  ReportGenerator not found as local tool – skipping." -ForegroundColor DarkYellow
}

Write-Host ""
Write-Host "┌─────────────────────────────────────────────────┐" -ForegroundColor Green
Write-Host "│                   DONE                          │" -ForegroundColor Green
Write-Host "├─────────────────────────────────────────────────┤" -ForegroundColor Green
Write-Host "│  TRX (CI) report :  $ResultsDir\TestResults.trx"
Write-Host "│  Coverage HTML   :  $CoverageDir\html\index.html"
Write-Host "└─────────────────────────────────────────────────┘" -ForegroundColor Green
Write-Host ""
