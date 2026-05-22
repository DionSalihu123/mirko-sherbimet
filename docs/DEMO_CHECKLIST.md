# Demo Checklist - 40 Pikë

**Punuar nga:** Dion Salihu

Ky checklist përdoret për screen recording dhe për screenshot-et që vendosen në dokumentin Word/PDF.

---

## 1. Përgatitja

Nga folderi kryesor i projektit:

```bash
docker compose up -d
dotnet ef database update --project auth-service/auth-service.csproj
./scripts/run-all-services.sh
```

Në fund duhet të shihet:

```text
=== Results: 12 passed, 0 failed ===
Dashboard: http://localhost:5080
Auth API:  http://localhost:5038/swagger
```

---

## 2. Auth Service - 10 pikë

Trego në kod:

- `auth-service/Controllers/AuthController.cs`
- `BCrypt.Net.BCrypt.HashPassword`
- `BCrypt.Net.BCrypt.Verify`
- `SecurityAlgorithms.HmacSha256`
- `DateTime.UtcNow.AddMinutes(30)`

Trego në UI:

- `http://localhost:5080/login`
- regjistro një user
- bëj login
- trego JWT token-in në ekran

Screenshot-et:

- bcrypt në kod
- JWT HMAC-SHA256 + expiry 30 min
- register/login në UI
- JWT i kthyer në UI

---

## 3. Resource Services - 10 pikë

Trego në kod:

- `resource-service-1/Program.cs`
- `resource-service-2/Program.cs`
- `AddJwtBearer`
- `ValidateIssuer`
- `ValidateAudience`
- `ValidateLifetime`
- `ValidateIssuerSigningKey`
- `/secure` me `.RequireAuthorization()`

Trego në UI:

- `http://localhost:5080/api-test`
- Resource 1 pa token -> 401
- Resource 1 me JWT valid -> 200
- Resource 2 pa token -> 401
- Resource 2 me JWT valid -> 200
- token i pavlefshëm -> 401

Screenshot-et:

- JWT validation në kod
- `/secure` me authorization
- 401 pa token
- 200 me token valid
- 401 me token të pavlefshëm

---

## 4. AI Module Training - 10 pikë

Trego në kod:

- `ai-service/train.py`
- lista `columns`
- `IsolationForest`
- `model.fit(train_df)`
- `joblib.dump`

Ekzekuto në terminal:

```bash
ai-service/.venv/bin/python ai-service/train.py
```

Trego në terminal:

```bash
ls -la models
cat models/model_config.json
```

Screenshot-et:

- features në `train.py`
- `IsolationForest`, `model.fit`, `joblib.dump`
- output-i i trajnimit
- `models/isolation_forest.pkl`
- `models/model_config.json`

---

## 5. Përgjigjet me shkrim - 10 pikë

Trego dokumentin:

- `docs/DOKUMENTIMI_40_PIKE.md`
- `docs/PERGJIGJET_SHKRIMORE.md`

Pikat që duhet të shihen:

- JWT vs Session
- bcrypt vs SHA-256
- Isolation Forest
- pse rate limiting nuk mjafton
- Auth0 / Keycloak / Cognito

Screenshot-et:

- dokumenti Word/PDF final
- seksioni i përgjigjeve teorike

---

## Rendi i rekomanduar për video

1. Hap README dhe trego që projekti synon 40 pikë.
2. Nis projektin me `./scripts/run-all-services.sh`.
3. Trego `12 passed, 0 failed`.
4. Hap kodin e Auth Service.
5. Bëj register/login në UI.
6. Trego JWT token-in.
7. Hap kodin e Resource Services.
8. Testo 401/200/401 në UI.
9. Hap `ai-service/train.py`.
10. Ekzekuto training në terminal.
11. Trego model files në `models/`.
12. Hap dokumentin me përgjigje teorike.

---

## Kontroll final

Para dorëzimit, sigurohu që në dokument ke vendosur screenshot-et për:

- Auth Service
- Resource Services
- AI training
- Përgjigjet teorike
- Testet `12 passed, 0 failed`
