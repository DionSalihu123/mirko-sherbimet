# AI Module (Task 4 only)

Train Isolation Forest on 250+ synthetic login rows and save the model.

```bash
python3 -m venv .venv
.venv/bin/pip install -r requirements.txt
.venv/bin/python train.py
```

Output:

- `../models/isolation_forest.pkl`
- `../models/model_config.json`

`main.py` is optional (inference API). For 40 points you only need **training + saved model**.
