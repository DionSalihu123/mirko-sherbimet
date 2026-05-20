#!/usr/bin/env python3
"""Tests for the 40-point scope only."""
from __future__ import annotations

import json
import sys
import time
from pathlib import Path

import urllib.error
import urllib.request

AUTH = "http://localhost:5038"
R1 = "http://localhost:5226"
R2 = "http://localhost:5295"
ROOT = Path(__file__).resolve().parent.parent

passed = 0
failed = 0


def check(name: str, ok: bool, detail: str = "") -> None:
    global passed, failed
    if ok:
        passed += 1
        print(f"  PASS  {name}" + (f" — {detail}" if detail else ""))
    else:
        failed += 1
        print(f"  FAIL  {name}" + (f" — {detail}" if detail else ""))


def http(method: str, url: str, body: dict | None = None, headers: dict | None = None) -> tuple[int, str]:
    data = None
    hdrs = {"Content-Type": "application/json", **(headers or {})}
    if body is not None:
        data = json.dumps(body).encode()
    req = urllib.request.Request(url, data=data, headers=hdrs, method=method)
    try:
        with urllib.request.urlopen(req, timeout=10) as resp:
            return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode()
    except Exception as e:
        return 0, str(e)


def wait_for(url: str, retries: int = 30) -> bool:
    for _ in range(retries):
        code, _ = http("GET", url)
        if code == 200:
            return True
        time.sleep(1)
    return False


def main() -> int:
    print("=== 40-point integration tests ===\n")

    print("[Task 1] Auth service — register, login, JWT")
    for name, url in [
        ("auth health", f"{AUTH}/api/auth/health"),
        ("resource-1 health", f"{R1}/health"),
        ("resource-2 health", f"{R2}/health"),
    ]:
        check(name, wait_for(url, retries=15), url)

    email = f"test_{int(time.time())}@example.com"
    password = "TestPass123!"

    code, _ = http("POST", f"{AUTH}/api/auth/register", {
        "username": "testuser",
        "email": email,
        "password": password,
    })
    check("register", code in (200, 201, 400), f"status={code}")

    code, body = http("POST", f"{AUTH}/api/auth/login", {"email": email, "password": password})
    token = ""
    if code == 200:
        token = json.loads(body).get("token", "")
    check("login returns JWT", code == 200 and len(token) > 20, f"status={code}")

    print("\n[Task 2] Resource APIs — JWT validation")
    code, _ = http("GET", f"{R1}/secure")
    check("no token -> 401", code == 401, f"status={code}")

    code, _ = http("GET", f"{R1}/secure", headers={"Authorization": f"Bearer {token}"})
    check("valid token -> 200 (service 1)", code == 200, f"status={code}")

    code, _ = http("GET", f"{R2}/secure", headers={"Authorization": f"Bearer {token}"})
    check("valid token -> 200 (service 2)", code == 200, f"status={code}")

    code, _ = http("GET", f"{R2}/secure", headers={"Authorization": f"Bearer {token}invalid"})
    check("invalid token -> 401", code == 401, f"status={code}")

    print("\n[Task 4] AI model training files")
    model_path = ROOT / "models" / "isolation_forest.pkl"
    config_path = ROOT / "models" / "model_config.json"
    check("isolation_forest.pkl exists", model_path.is_file(), str(model_path))
    check("model_config.json exists", config_path.is_file(), str(config_path))

    print("\n[Task 3] Written answers document")
    doc_path = ROOT / "docs" / "PERGJIGJET_SHKRIMORE.md"
    check("PERGJIGJET_SHKRIMORE.md exists", doc_path.is_file(), "export to PDF for submission")

    print(f"\n=== Results: {passed} passed, {failed} failed ===")
    return 0 if failed == 0 else 1


if __name__ == "__main__":
    sys.exit(main())
