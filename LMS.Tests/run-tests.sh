#!/usr/bin/env bash
# ==============================================================
#  run-tests.sh
#  
#  PURPOSE: Runs all LMS integration tests and produces:
#    1. A terminal summary (pass/fail per test)
#    2. A machine-readable TRX file  (XML, consumed by CI tools)
#    3. A beautiful HTML test report  → TestResults/Report/index.html
#    4. A code-coverage HTML report   → TestResults/Coverage/index.html
#
#  USAGE (from the solution root):
#    bash LMS.Tests/run-tests.sh
# ==============================================================

set -e   # exit immediately on any failure

RESULTS_DIR="TestResults"
COVERAGE_DIR="$RESULTS_DIR/Coverage"
REPORT_DIR="$RESULTS_DIR/Report"

echo ""
echo "┌─────────────────────────────────────────────────┐"
echo "│           LMS Integration Test Runner           │"
echo "└─────────────────────────────────────────────────┘"
echo ""

# ── 1. Run tests with coverage collection ──────────────────────────────────
echo "▶  Running tests..."
dotnet test LMS.Tests/LMS.Tests.csproj \
  --configuration Release \
  --logger "trx;LogFileName=TestResults.trx" \
  --logger "console;verbosity=detailed" \
  --results-directory "$RESULTS_DIR" \
  /p:CollectCoverage=true \
  /p:CoverageOutputFormat=cobertura \
  /p:CoverageOutput="$COVERAGE_DIR/coverage.cobertura.xml" \
  /p:Exclude="[*Tests*]*" \
  --no-restore

echo ""
echo "▶  Generating HTML coverage report..."

# ── 2. Generate HTML coverage report using ReportGenerator ─────────────────
dotnet tool run reportgenerator \
  -reports:"$COVERAGE_DIR/coverage.cobertura.xml" \
  -targetdir:"$COVERAGE_DIR/html" \
  -reporttypes:"Html;Badges" \
  || echo "⚠  ReportGenerator not found as a local tool – skipping coverage HTML."

echo ""
echo "┌─────────────────────────────────────────────────┐"
echo "│                   DONE                          │"
echo "├─────────────────────────────────────────────────┤"
echo "│  TRX (CI) report:   $RESULTS_DIR/TestResults.trx"
echo "│  Coverage HTML:     $COVERAGE_DIR/html/index.html"
echo "└─────────────────────────────────────────────────┘"
echo ""
