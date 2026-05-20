# Demo checklist — 40 points only

## Before presentation

```bash
cd /home/dion/mikro_sherbimet_jo_gati
docker compose up -d
dotnet ef database update --project auth-service/auth-service.csproj
./scripts/run-all-services.sh
```

## What to show (screenshots per section)

| Points | What to do | Evidence |
|--------|------------|----------|
| **10** | Register + login on http://localhost:5080/login | JWT on screen; mention bcrypt in DB/code |
| **10** | API Test page or `./scripts/test-jwt.sh` | 401 without token, 200 with token, 401 invalid |
| **10** | Export `docs/PERGJIGJET_SHKRIMORE.md` to PDF | One section per topic (a–e) |
| **10** | Run `ai-service/.venv/bin/python ai-service/train.py` | Screenshot + `models/isolation_forest.pkl` |

## Screen recording (10+ min)

1. Start `./scripts/run-all-services.sh`
2. Walk through dashboard: Overview → Login → API Test → AI Training
3. Open Swagger http://localhost:5038/swagger (optional)
4. Run `train.py` in terminal
5. Show PDF written answers

## Automated test

```bash
python3 scripts/run-integration-tests.py
```

Expected: all tests pass.
