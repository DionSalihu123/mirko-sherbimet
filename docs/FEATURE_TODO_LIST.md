# Feature TODO List - JWT Microservices Project

Ky dokument është një checklist për screen recording dhe dokumentimin e plotë të projektit.

## 1. Auth Service

- [ ] Verifiko `auth-service/Controllers/AuthController.cs`
  - [ ] Shfaq kodin e `Register` dhe `Login`
  - [ ] Shfaq `BCrypt.Net.BCrypt.HashPassword(dto.Password)`
  - [ ] Shfaq `BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)`
  - [ ] Shfaq `GenerateJwtToken(user)`
  - [ ] Shfaq `SecurityAlgorithms.HmacSha256`
  - [ ] Shfaq `DateTime.UtcNow.AddMinutes(30)`

- [ ] Verifiko `auth-service/DTOs/LoginDto.cs`
  - [ ] Shfaq `[Required]` në `Email` dhe `Password`
  - [ ] Shfaq `[EmailAddress]` në `Email`

- [ ] Verifiko `auth-service/Models/User.cs`
  - [ ] Shfaq `[Required]` në `Email`

- [ ] Verifiko database constraints në `ApplicationDbContext.cs`
  - [ ] Shfaq `Email.IsRequired()`
  - [ ] Shfaq `HasIndex(u => u.Email).IsUnique()`

- [ ] Testet e login:
  1. [ ] Bad password → **401 Unauthorized**
     - Screenshot: `401_bad_password.png`
  2. [ ] Empty fields → **400 Bad Request**
     - Screenshot: `400_empty_fields.png`
  3. [ ] Correct credentials → **200 OK**
     - Screenshot: `200_valid_login.png`
  4. [ ] Whitespace-only → **400 Bad Request**
     - Screenshot: `400_whitespace_only.png`

## 2. Resource Services

- [ ] Verifiko `resource-service-1/Program.cs`
- [ ] Verifiko `resource-service-2/Program.cs`
- [ ] Shfaq `AddJwtBearer` dhe `TokenValidationParameters`
- [ ] Shfaq `ValidateIssuer`, `ValidateAudience`, `ValidateLifetime`, `ValidateIssuerSigningKey`
- [ ] Shfaq `/secure` dhe `.RequireAuthorization()`

- [ ] Testet e API-ve në UI:
  - [ ] Resource 1 pa token → **401**
  - [ ] Resource 1 me token valid → **200**
  - [ ] Resource 2 pa token → **401**
  - [ ] Resource 2 me token valid → **200**
  - [ ] Token i pavlefshëm → **401**

- [ ] Screenshot-et:
  - `2.1_jwt_validation_code.png`
  - `2.2_secure_endpoint_code.png`
  - `2.3_test_401_no_token.png`
  - `2.4_test_200_valid_token.png`
  - `2.5_test_401_invalid_token.png`

## 3. AI Module

- [ ] Verifiko `ai-service/train.py`
  - [ ] Shfaq listën e `columns`
  - [ ] Shfaq `IsolationForest(...)`
  - [ ] Shfaq `model.fit(train_df)`
  - [ ] Shfaq `joblib.dump(model, ...)`

- [ ] Ekzekuto trajnimin:
  - `ai-service/.venv/bin/python ai-service/train.py`

- [ ] Verifiko folderin `models/`
  - [ ] `models/isolation_forest.pkl`
  - [ ] `models/model_config.json`

- [ ] Screenshot-et:
  - `3.1_ai_features.png`
  - `3.2_isolation_forest_code.png`
  - `3.3_train_output.png`
  - `3.4_model_files.png`

## 4. Written Answers

- [ ] Verifiko `docs/DOKUMENTIMI_40_PIKE.md`
- [ ] Verifiko `docs/PERGJIGJET_SHKRIMORE.md`
- [ ] Sigurohu që të gjitha përgjigjet teorike të jenë të plotë dhe të referuara në dokument
- [ ] Screenshot-et:
  - `4.1_answers_cover.png`
  - `4.2_jwt_vs_session.png`
  - `4.3_bcrypt_vs_sha256.png`

## 5. Infrastrukturë dhe Docker

- [ ] Hap `docker-compose.yml`
- [ ] Verifiko PostgreSQL në Docker
- [ ] Verifiko portin `5433` në `auth-service/appsettings.json`
- [ ] Verifiko që `scripts/run-all-services.sh` kalon testet me **12 passed, 0 failed**

## 6. Screen Recording Flow

1. Starto projektin dhe trego `12 passed, 0 failed`
2. Trego `auth-service` dhe `AuthController.cs`
3. Regjistro një user dhe bëj login
4. Trego token-in JWT në UI
5. Trego `resource-service-1` dhe `resource-service-2`
6. Bëj testet 401/200/401 në dashboard
7. Trego `ai-service/train.py` dhe ekzekutimin e trajnimit
8. Trego `models/isolation_forest.pkl` dhe `models/model_config.json`
9. Trego dokumentet `DOKUMENTIMI_40_PIKE.md` dhe `PERGJIGJET_SHKRIMORE.md`
10. Përmbylle me përmbledhjen e 40 pikëve
