#!/usr/bin/env bash
# Manual start instructions for 40-point scope.
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

docker compose up -d postgres-db 2>/dev/null || true
sleep 2
dotnet ef database update --project auth-service/auth-service.csproj

if [ ! -f models/isolation_forest.pkl ]; then
  ai-service/.venv/bin/python ai-service/train.py 2>/dev/null || python3 ai-service/train.py
fi

echo ""
echo "Open 4 terminals:"
echo "  1) dotnet run --project auth-service"
echo "  2) dotnet run --project resource-service-1"
echo "  3) dotnet run --project resource-service-2"
echo "  4) dotnet run --project dashboard"
echo ""
echo "Dashboard: http://localhost:5080"
echo "Or use: ./scripts/run-all-services.sh"
