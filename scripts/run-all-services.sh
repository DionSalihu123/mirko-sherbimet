#!/usr/bin/env bash
# Start services for 40-point scope (auth + 2 resources + dashboard).
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

mkdir -p logs

docker compose up -d postgres-db 2>/dev/null || true
sleep 2

dotnet ef database update --project auth-service/auth-service.csproj >/dev/null 2>&1 || true

if [ ! -d ai-service/.venv ]; then
  python3 -m venv ai-service/.venv
  ai-service/.venv/bin/pip install -q -r ai-service/requirements.txt
fi

if [ ! -f models/isolation_forest.pkl ]; then
  ai-service/.venv/bin/python ai-service/train.py
fi

fuser -k 5038/tcp 5226/tcp 5295/tcp 5080/tcp 2>/dev/null || true
sleep 1

dotnet build JwtAiSystem.sln -m:1 -v q

nohup dotnet run --project auth-service --no-build > logs/auth.log 2>&1 &
nohup dotnet run --project resource-service-1 --no-build > logs/resource1.log 2>&1 &
nohup dotnet run --project resource-service-2 --no-build > logs/resource2.log 2>&1 &
nohup dotnet run --project dashboard --no-build > logs/dashboard.log 2>&1 &

echo "Starting services... wait ~8 seconds"
sleep 8
python3 scripts/run-integration-tests.py
echo ""
echo "Dashboard: http://localhost:5080"
echo "Auth API:  http://localhost:5038/swagger"
echo "Train AI:  ai-service/.venv/bin/python ai-service/train.py"
