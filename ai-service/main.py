from fastapi import FastAPI
from pydantic import BaseModel, ConfigDict, Field
from pathlib import Path
import numpy as np
import joblib
import json

app = FastAPI()

ROOT_DIR = Path(__file__).resolve().parent.parent
MODEL_PATH = ROOT_DIR / "models" / "isolation_forest.pkl"
CONFIG_PATH = ROOT_DIR / "models" / "model_config.json"
model = None
threshold = 0.0


@app.on_event("startup")
def load_model():
    global model, threshold

    if not MODEL_PATH.exists():
        raise RuntimeError(f"Model file not found at {MODEL_PATH}. Run train.py first.")

    model = joblib.load(MODEL_PATH)

    if CONFIG_PATH.exists():
        with open(CONFIG_PATH, "r", encoding="utf-8") as fh:
            config = json.load(fh)
            threshold = float(config.get("threshold", 0.0))

    print(f"✅ AI model loaded successfully, anomaly threshold={threshold}")


class LoginFeatures(BaseModel):
    model_config = ConfigDict(populate_by_name=True)

    hour: int
    failed_attempts: int = Field(alias="failedAttempts")
    new_ip: int = Field(alias="newIp")
    success_rate: float = Field(alias="successRate")
    login_frequency: int = Field(alias="loginFrequency")
    country_changed: int = Field(alias="countryChanged")
    impossible_travel: int = Field(alias="impossibleTravel")
    distance_km: float = Field(alias="distanceKm")
    hours_since_last_login: float = Field(alias="hoursSinceLastLogin")
    user_agent_changed: int = Field(alias="userAgentChanged")


@app.get("/health")
def health():
    return {"status": "ok", "service": "ai-service", "modelLoaded": model is not None}


@app.post("/score")
def score(features: LoginFeatures):
    X = np.array([[
        features.hour,
        features.failed_attempts,
        features.new_ip,
        features.success_rate,
        features.login_frequency,
        features.country_changed,
        features.impossible_travel,
        features.distance_km,
        features.hours_since_last_login,
        features.user_agent_changed
    ]])

    score = model.decision_function(X)[0]
    anomaly = score < threshold

    return {
        "anomaly": bool(anomaly),
        "riskScore": float(score)
    }
