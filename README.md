# JWT Microservices — 40 Points (Simplified)

University project scoped to **4 tasks = 40 points**. Everything else was removed so the codebase stays easy to follow.

| Points | Task | What you demo |
|--------|------|----------------|
| 10 | Auth Service | Register, login, **bcrypt**, **JWT** (30 min, HMAC-SHA256) |
| 10 | 2 Resource APIs | No token → **401**, valid → **200**, bad token → **401** |
| 10 | Written answers | `docs/PERGJIGJET_SHKRIMORE.md` → export PDF |
| 10 | AI training | `ai-service/train.py` → `models/isolation_forest.pkl` |

**Not in this version:** TLS/Wireshark, live AI blocking, brute-force sim, impossible travel, SignalR dashboard, false-positive tuning.

---

## Quick start

```bash
cd /home/dion/mikro_sherbimet_jo_gati
docker compose up -d
dotnet ef database update --project auth-service/auth-service.csproj
chmod +x scripts/run-all-services.sh
./scripts/run-all-services.sh
```

Open **http://localhost:5080** — use the sidebar for each task.

Train AI (Task 4):

```bash
python3 -m venv ai-service/.venv
ai-service/.venv/bin/pip install -r ai-service/requirements.txt
ai-service/.venv/bin/python ai-service/train.py
```

---

## Services

| Service | Port | Role |
|---------|------|------|
| auth-service | 5038 | Register, login, JWT |
| resource-service-1 | 5226 | Protected `/secure` |
| resource-service-2 | 5295 | Protected `/secure` |
| dashboard | 5080 | Web UI for demo |
| PostgreSQL | 5432 | Docker — users only |

---

## Project layout

```
├── auth-service/           # Task 1
├── resource-service-1/     # Task 2
├── resource-service-2/     # Task 2
├── ai-service/train.py     # Task 4
├── models/                 # Trained .pkl after train.py
├── dashboard/              # UI for all demos
├── docs/PERGJIGJET_SHKRIMORE.md   # Task 3
└── scripts/run-all-services.sh
```

---

## Tests

```bash
python3 scripts/run-integration-tests.py
./scripts/test-jwt.sh
```

---

## Submission checklist

See [docs/DEMO_CHECKLIST.md](docs/DEMO_CHECKLIST.md).
