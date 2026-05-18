from fastapi import FastAPI
from pydantic import BaseModel
import numpy as np
import joblib
import os

app = FastAPI()

MODEL_PATH = "models/isolation_forest.pkl"
model = None


@app.on_event("startup")
def load_model():
    global model

    if not os.path.exists(MODEL_PATH):
        raise RuntimeError("Model file not found. Run train.py first.")

    model = joblib.load(MODEL_PATH)
    print("✅ AI model loaded successfully")


class LoginFeatures(BaseModel):
    hour: int
    failed_attempts: int
    new_ip: int
    success_rate: float
    login_frequency: int


@app.post("/score")
def score(features: LoginFeatures):

    X = np.array([[
        features.hour,
        features.failed_attempts,
        features.new_ip,
        features.success_rate,
        features.login_frequency
    ]])

    prediction = model.predict(X)[0]
    score = model.decision_function(X)[0]

    return {
        "anomaly": bool(prediction == -1),
        "riskScore": float(score)
    }
