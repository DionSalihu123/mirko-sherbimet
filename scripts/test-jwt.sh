#!/usr/bin/env bash
# Demonstrates JWT validation on resource-service-1: 401 without token, 200 with token, 401 expired.
set -euo pipefail

AUTH_URL="${AUTH_URL:-http://localhost:5038}"
RESOURCE_URL="${RESOURCE_URL:-http://localhost:5226}"
EMAIL="${EMAIL:-demo@example.com}"
PASSWORD="${PASSWORD:-DemoPass123!}"

echo "=== 1) Register (ignore if exists) ==="
curl -s -X POST "$AUTH_URL/api/auth/register" \
  -H "Content-Type: application/json" \
  -d "{\"username\":\"demo\",\"email\":\"$EMAIL\",\"password\":\"$PASSWORD\"}" || true
echo ""

echo "=== 2) No token -> expect 401 ==="
curl -s -o /dev/null -w "HTTP %{http_code}\n" "$RESOURCE_URL/secure"

echo "=== 3) Login -> get JWT ==="
TOKEN=$(curl -s -X POST "$AUTH_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"$EMAIL\",\"password\":\"$PASSWORD\"}" | python3 -c "import sys,json; print(json.load(sys.stdin).get('token',''))")

if [ -z "$TOKEN" ]; then
  echo "Login failed - is auth-service running?"
  exit 1
fi
echo "Token received (${#TOKEN} chars)"

echo "=== 4) Valid token -> expect 200 ==="
curl -s -o /dev/null -w "HTTP %{http_code}\n" \
  -H "Authorization: Bearer $TOKEN" \
  "$RESOURCE_URL/secure"

echo "=== 5) Tampered token -> expect 401 ==="
curl -s -o /dev/null -w "HTTP %{http_code}\n" \
  -H "Authorization: Bearer ${TOKEN}x" \
  "$RESOURCE_URL/secure"

echo ""
echo "For expired token demo: wait 31 minutes after login, or change Jwt expiry to 1 min in appsettings for testing."
