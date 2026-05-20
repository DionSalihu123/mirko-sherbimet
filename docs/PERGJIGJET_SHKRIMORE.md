# Përgjigje me Shkrim – Kërkesa 10 (min. 1 faqe)

## (a) JWT vs Sesion

**Sesioni (session-based):** Serveri ruan gjendjen e përdoruesit (session ID në cookie). Çdo request kërkon lookup në DB/cache. E thjeshtë për monolit, por vështirë në mikroshërbime sepse session duhet të ndahet ose të “sticky” në një instancë.

**JWT (JSON Web Token):** Pas login-it, serveri kthen një token të nënshkruar që klienti dërgon në `Authorization: Bearer`. Çdo mikroshërbim e validon vetë me sekretin e përbashkët (HMAC-SHA256) pa pyetur auth-service-in. Është **stateless** – shkallëzohet mirë. Disavantazh: nuk mund ta “fshish” lehtë token-in para skadimit (duhet blacklist ose expiry i shkurtër – këtu 30 min).

Në këtë projekt, `auth-service` lëshon JWT; `resource-service-1/2` e verifikojnë me të njëjtin `Jwt:Key`.

---

## (b) bcrypt vs SHA-256

**SHA-256** është hash i shpejtë dhe deterministik. Sulmuesi mund të provojë miliarda fjalëkalime në sekondë me GPU (rainbow tables për fjalëkalime të zakonshme).

**bcrypt** është i projektuar për fjalëkalime: përdor **salt** unik për çdo përdorues dhe **work factor** (kosto e llogaritjes). Çdo verifikim zgjat disa ms – e padobishme për brute-force offline. Në `AuthController`, ruajmë `BCrypt.HashPassword` dhe `BCrypt.Verify`.

SHA-256 është i mirë për integritetin e skedarëve; **jo** për ruajtjen e password-eve.

---

## (c) Isolation Forest

Algoritëm **unsupervised** që ndërton pemë të rastësishme dhe mat sa “e izoluar” është një pikë. Outlier-at (login të pazakontë) izolohen me më pak ndarje → score i ulët.

**Pse për login security?** Nuk kemi gjithmonë 1000 etiketa “sulm” për trajnim supervizuar. Trajnojmë me ~200 login “normale” + disa shembuj anomalie në `train.py`. Në runtime, `auth-service` dërgon features (ora, failed attempts, geo, etj.) te `ai-service`; nëse `decision_function < threshold`, flagohet anomali.

---

## (d) Pse rate limit nuk mjafton

**Rate limiting (Token Bucket)** në `LoginRateLimiter` kufizon shpejtësinë e request-ve për IP/email – mirë kundër flood-it të thjeshtë.

Por nuk zbulon:
- **Credential stuffing** (password të vjedhura, shpejtësi e ulët)
- **Impossible travel** (geo e pamundur)
- **Orare/browser të pazakontë** për atë përdorues

AI analizon **kontekstin** të gjithë login-it, jo vetëm numrin e request-ve. Prandaj përdorim të dyja: rate limit + Isolation Forest.

---

## (e) Krahasim me Auth0 / Keycloak / Cognito

| | Auth0 | Keycloak | AWS Cognito | Ky projekt |
|---|-------|----------|-------------|------------|
| Lloji | SaaS / IdP | Open-source IdP | Cloud AWS | Custom mikroshërbime |
| JWT/OAuth | Po | Po | Po | Po (HMAC) |
| AI anomaly | Jo (shtesë) | Jo | Jo (GuardDuty tjetër) | Po (Isolation Forest) |
| Kontroll | I ulët | I lartë (self-host) | Mesatar | Plotë (edukativ) |
| Kompleksitet | I ulët | I mesëm | I mesëm | I lartë (mësim) |

**Auth0:** i shpejtë për startup, por kosto dhe varësi nga cloud. **Keycloak:** standard enterprise, SSO, por setup i rëndë. **Cognito:** integrim AWS. **Ky sistem:** demonstron konceptet (JWT, bcrypt, TLS, AI) për kurs – jo zëvendëson prodhimin pa hardening shtesë.

---

## Tabela False Positive (shembull – plotëso pas testit)

| Threshold | Login normale (50) | False positives | Shënim |
|-----------|-------------------|-----------------|--------|
| -0.05 (para) | 50 | ? | Më i ndjeshëm |
| -0.10 (pas) | 50 | ? | Default në `model_config.json` |

Ekzekuto: `python3 scripts/normal_login_tuning.py` dhe plotëso numrat në dokumentin final për profesorin.
