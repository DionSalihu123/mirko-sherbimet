# Screenshots Placeholder

Këtu duhet të vendosen screenshots për dokumentimin DOKUMENTIMI_40_PIKE.md

## 1. Auth Service Screenshots (10 pikë)

### Screenshot 1.1 - Kodi i bcrypt
**Lokacioni në kod:** [auth-service/Controllers/AuthController.cs](../auth-service/Controllers/AuthController.cs#L40-L80)

Duhet të shfaqet:
- Rreshta me `BCrypt.Net.BCrypt.HashPassword()` në `Register`
- Rreshta me `BCrypt.Net.BCrypt.Verify()` në `Login`

**Emri i fajllit:** `1.1_bcrypt_code.png`

---

### Screenshot 1.2 - Kodi i JWT
**Lokacioni në kod:** [auth-service/Controllers/AuthController.cs](../auth-service/Controllers/AuthController.cs#L85-L115)

Duhet të shfaqet:
- `HmacSha256`
- `AddMinutes(30)`
- Claims me `NameIdentifier` dhe `Email`

**Emri i fajllit:** `1.2_jwt_code.png`

---

### Screenshot 1.3 - UI Login/Register
**Lokacioni:** http://localhost:5080 - Login/Register page

Duhet të shfaqet:
- Form-i me email dhe password
- Butoni Register/Login

**Emri i fajllit:** `1.3_ui_login_register.png`

---

### Screenshot 1.4 - JWT Token në UI
**Lokacioni:** http://localhost:5080 - pas login-it të suksesshëm

Duhet të shfaqet:
- Token-i JWT
- Expiry time (30 minuta)
- Email-i i përdoruesit

**Emri i fajllit:** `1.4_jwt_response.png`

---

## 2. Resource Services Screenshots (10 pikë)

### Screenshot 2.1 - JWT Validation Code
**Lokacioni në kod:** [resource-service-1/Program.cs](../resource-service-1/Program.cs)

Duhet të shfaqet:
- `AddJwtBearer`
- `ValidateIssuer = true`
- `ValidateAudience = true`
- `ValidateLifetime = true`
- `ValidateIssuerSigningKey = true`

**Emri i fajllit:** `2.1_jwt_validation_code.png`

---

### Screenshot 2.2 - Secure Endpoint
**Lokacioni në kod:** [resource-service-1/Program.cs](../resource-service-1/Program.cs)

Duhet të shfaqet:
- Endpoint `/secure`
- `.RequireAuthorization()`

**Emri i fajllit:** `2.2_secure_endpoint_code.png`

---

### Screenshot 2.3 - Test 401 (No Token)
**Lokacioni:** http://localhost:5080 - Testimi i API-ve - "401-resource-1 pa token"

Duhet të shfaqet:
- HTTP 401 response
- Error message "Unauthorized"

**Emri i fajllit:** `2.3_test_401_no_token.png`

---

### Screenshot 2.4 - Test 200 (Valid Token)
**Lokacioni:** http://localhost:5080 - Testimi i API-ve - "200-resource-1 valid"

Duhet të shfaqet:
- HTTP 200 response
- Data nga /secure endpoint

**Emri i fajllit:** `2.4_test_200_valid_token.png`

---

### Screenshot 2.5 - Test 401 (Invalid Token)
**Lokacioni:** http://localhost:5080 - Testimi i API-ve - "401-token-i-pavleffshem"

Duhet të shfaqet:
- HTTP 401 response
- Tampered token në Authorization header

**Emri i fajllit:** `2.5_test_401_invalid_token.png`

---

## 3. AI Module Screenshots (10 pikë)

### Screenshot 3.1 - Features në train.py
**Lokacioni në kod:** [ai-service/train.py](../ai-service/train.py)

Duhet të shfaqet:
- Kolona/features si: hour, failed_attempts, new_ip, success_rate, etc.

**Emri i fajllit:** `3.1_ai_features.png`

---

### Screenshot 3.2 - IsolationForest Code
**Lokacioni në kod:** [ai-service/train.py](../ai-service/train.py)

Duhet të shfaqet:
- `IsolationForest(...)`
- `model.fit()`
- `joblib.dump()`

**Emri i fajllit:** `3.2_isolation_forest_code.png`

---

### Screenshot 3.3 - Terminal Output
**Komanda:**
```bash
cd /home/bits_destroyer/mirko-sherbimet
ai-service/.venv/bin/python ai-service/train.py
```

Duhet të shfaqet:
- Output: "Model trained and saved successfully!"

**Emri i fajllit:** `3.3_train_output.png`

---

### Screenshot 3.4 - Model Files
**Lokacioni:** Folderi `models/`

Duhet të shfaqet:
- `models/isolation_forest.pkl`
- `models/model_config.json`

**Emri i fajllit:** `3.4_model_files.png`

---

## 4. Written Answers Screenshots (10 pikë)

### Screenshot 4.1 - Document Cover
**Lokacioni:** [PERGJIGJET_SHKRIMORE.md](../PERGJIGJET_SHKRIMORE.md)

Duhet të shfaqet:
- Fronta e dokumentit me titullin dhe qëllimin

**Emri i fajllit:** `4.1_answers_cover.png`

---

### Screenshot 4.2 - JWT vs Session
**Lokacioni:** [PERGJIGJET_SHKRIMORE.md](../PERGJIGJET_SHKRIMORE.md#jwt-vs-session)

Duhet të shfaqet:
- Krahasim JWT vs Session

**Emri i fajllit:** `4.2_jwt_vs_session.png`

---

### Screenshot 4.3 - bcrypt vs SHA256
**Lokacioni:** [PERGJIGJET_SHKRIMORE.md](../PERGJIGJET_SHKRIMORE.md#bcrypt-vs-sha256)

Duhet të shfaqet:
- Krahasim bcrypt vs SHA256

**Emri i fajllit:** `4.3_bcrypt_vs_sha256.png`

---

## Mënyra për të marr screenshots

### 1. Për kod (screenshots 1.1, 1.2, 2.1, 2.2, 3.1, 3.2)
- Hape file-in në VS Code
- Shfaqe rreshtat relevantë
- Bëj screenshot me `Shift + PrintScreen` ose përdor tool për screenshot
- Ruaje në këtë folder

### 2. Për UI (screenshots 1.3, 1.4, 2.3, 2.4, 2.5)
- Hape browser në `http://localhost:5080`
- Navigone në seksionin relevant
- Bëj screenshot
- Ruaje në këtë folder

### 3. Për terminal (screenshot 3.3)
- Ekzekuto komandën e specifikuar
- Bëj screenshot e output-it
- Ruaje në këtë folder

---

## Përfundim

Kur të jenë bërë të gjithë screenshots, folderi `screenshots/` duhet të ketë 14 PNG files:
- 1.1 deri 1.4 (4 screenshots)
- 2.1 deri 2.5 (5 screenshots)
- 3.1 deri 3.4 (4 screenshots)
- 4.1 deri 4.3 (3 screenshots)

Atëherë dokumentimi DOKUMENTIMI_40_PIKE.md mund të linkojë këto screenshots në placeholders e tyre.
