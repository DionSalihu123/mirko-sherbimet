import pandas as pd
from sklearn.ensemble import IsolationForest
import joblib
import os

os.makedirs("models", exist_ok=True)

# --------------------------------------------------
# SIMULATED NORMAL LOGIN DATA (for training)
# --------------------------------------------------

data = []

for i in range(200):
    data.append([
        12,      # hour of day (normal working hours)
        0,       # failed attempts
        0,       # new IP flag
        1.0,     # login success rate
        3        # logins per day
    ])

df = pd.DataFrame(data, columns=[
    "hour",
    "failed_attempts",
    "new_ip",
    "success_rate",
    "login_frequency"
])

# --------------------------------------------------
# TRAIN MODEL
# --------------------------------------------------

model = IsolationForest(
    contamination=0.1,
    random_state=42
)

model.fit(df)

# save model
joblib.dump(model, "models/isolation_forest.pkl")

print("Model trained and saved successfully!")
