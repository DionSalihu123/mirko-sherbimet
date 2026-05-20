"""
Task 4 (10 pts): Train Isolation Forest on 250+ login feature rows and save the model.
Run: ai-service/.venv/bin/python ai-service/train.py
"""
import pandas as pd
from sklearn.ensemble import IsolationForest
from pathlib import Path
import joblib
import json

ROOT_DIR = Path(__file__).resolve().parent.parent
MODELS_DIR = ROOT_DIR / "models"
MODELS_DIR.mkdir(parents=True, exist_ok=True)

samples = []

# Normal login behavior
for i in range(180):
    samples.append([
        9 + (i % 8),      # hour of day in working hours
        0,                # failed attempts
        0,                # same IP
        1.0,              # success rate
        2 + (i % 4),      # login frequency
        0,                # country changed
        0,                # impossible travel
        0.0,              # distance km
        2.0 + (i % 6),    # hours since last login
        0                 # user agent changed
    ])

# Normal variation from different browser/location but still benign
for i in range(40):
    samples.append([
        18 + (i % 4),
        0,
        1,
        1.0,
        1 + (i % 3),
        0,
        0,
        50.0,
        24.0,
        1
    ])

# Brute-force / credential stuffing anomalies
for i in range(15):
    samples.append([
        2 + (i % 4),
        6 + (i % 5),
        1,
        0.0,
        15,
        1,
        0,
        100.0,
        0.5,
        1
    ])

# Impossible travel anomalies
for i in range(15):
    samples.append([
        10 + (i % 3),
        0,
        1,
        1.0,
        3,
        1,
        1,
        10000.0,
        0.2,
        1
    ])

columns = [
    "hour",
    "failed_attempts",
    "new_ip",
    "success_rate",
    "login_frequency",
    "country_changed",
    "impossible_travel",
    "distance_km",
    "hours_since_last_login",
    "user_agent_changed"
]

train_df = pd.DataFrame(samples, columns=columns)

model = IsolationForest(
    contamination=0.08,
    random_state=42
)

model.fit(train_df)

joblib.dump(model, MODELS_DIR / "isolation_forest.pkl")

config = {
    "threshold": -0.1
}

with open(MODELS_DIR / "model_config.json", "w", encoding="utf-8") as config_file:
    json.dump(config, config_file, indent=2)

print("Model trained and saved successfully!")
print("Threshold saved to models/model_config.json")
