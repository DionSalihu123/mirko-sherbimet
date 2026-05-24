from fastapi import FastAPI
from pydantic import BaseModel, ConfigDict, Field
from pathlib import Path
import numpy as np
import joblib
import json
import logging

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(title="AI Anomaly Detection Service")

ROOT_DIR = Path(__file__).resolve().parent.parent
MODEL_PATH = ROOT_DIR / "models" / "isolation_forest.pkl"
CONFIG_PATH = ROOT_DIR / "models" / "model_config.json"

model = None
threshold = -0.1

@app.on_event("startup")
async def load_model():
    global model, threshold
    try:
        if not MODEL_PATH.exists():
            logger.error("Model not found. Please run train.py first.")
            return
        
        model = joblib.load(MODEL_PATH)
        
        if CONFIG_PATH.exists():
            with open(CONFIG_PATH, "r") as f:
                config = json.load(f)
                threshold = float(config.get("threshold", -0.1))
        
        logger.info(f"✅ AI Model loaded successfully | Threshold: {threshold}")
    except Exception as e:
        logger.error(f"Error loading model: {e}")

class LoginFeatures(BaseModel):
    model_config = ConfigDict(populate_by_name=True)
    
    hour: int
    failed_attempts: int = Field(alias="failedAttempts", default=0)
    new_ip: int = Field(alias="newIp", default=0)
    success_rate: float = Field(alias="successRate", default=1.0)
    login_frequency: int = Field(alias="loginFrequency", default=1)
    country_changed: int = Field(alias="countryChanged", default=0)
    impossible_travel: int = Field(alias="impossibleTravel", default=0)
    distance_km: float = Field(alias="distanceKm", default=0.0)
    hours_since_last_login: float = Field(alias="hoursSinceLastLogin", default=24.0)
    user_agent_changed: int = Field(alias="userAgentChanged", default=0)

@app.get("/health")
def health():
    return {"status": "ok", "model_loaded": model is not None, "threshold": threshold}

@app.post("/score")
def score(features: LoginFeatures):
    if model is None:
        return {"anomaly": False, "riskScore": 0.0, "error": "Model not loaded"}
    
    X = np.array([[ 
        features.hour, features.failed_attempts, features.new_ip,
        features.success_rate, features.login_frequency, features.country_changed,
        features.impossible_travel, features.distance_km,
        features.hours_since_last_login, features.user_agent_changed
    ]])
    
    risk_score = model.decision_function(X)[0]
    is_anomaly = risk_score < threshold

    return {
        "anomaly": bool(is_anomaly),
        "riskScore": float(risk_score),
        "threshold": threshold,
        "riskLevel": "HIGH" if is_anomaly else "NORMAL",
        "blockAccount": bool(is_anomaly)   # Ky flag do të përdoret nga Auth Service
    }
