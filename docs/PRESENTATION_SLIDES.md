% JWT Microservices System
% Dion Salihu
% 40 Point Demo

# Project Overview

- JWT microservices autentifikimi
- Auth Service + 2 Resource Services
- AI Module me Isolation Forest
- Dashboard demo

---

# Architecture

- `auth-service` : Regjistrim, login, JWT
- `resource-service-1` : API i mbrojtur me JWT
- `resource-service-2` : API i dytë i mbrojtur me JWT
- `ai-service` : Trajnim model AI
- `dashboard` : UI demonstrim

---

# Auth Service

- `/api/auth/register`
- `/api/auth/login`
- bcrypt për password
- JWT HMAC-SHA256 me expiry 30 min

---

# JWT Implementation

- Claims: `NameIdentifier`, `Email`
- Issuer: `auth-service`
- Audience: `auth-service-users`
- Secret: `THIS_IS_A_SUPER_SECRET_KEY_123456789`

---

# Resource Services

- Validim stateless të JWT
- `GET /secure` require authorization
- `401` pa token ose token invalid
- `200` me token valid

---

# AI Module

- `ai-service/train.py`
- Trajnim me Isolation Forest
- 250+ mostra
- Output: `models/isolation_forest.pkl`
- Threshold: `-0.1`

---

# Test & Evidence

- `./scripts/run-all-services.sh`
- `12 passed, 0 failed`
- `http://localhost:5080`
- Swagger auth: `http://localhost:5038/swagger`

---

# Screenshots to Capture

1. bcrypt + JWT në `AuthController.cs`
2. JWT validation në `resource-service-1` dhe `resource-service-2`
3. Terminali me `12 passed, 0 failed`
4. AI training dhe `models/isolation_forest.pkl`
5. `docs/PERGJIGJET_SHKRIMORE.md`

---

# Notes

- Versioni i këtij projekti është për 40 pikë
- Kërkesat e avancuara si TLS/Wireshark dhe SignalR nuk janë implementuar
- Platforma funksionon me `dotnet 10.0.107` dhe Docker
