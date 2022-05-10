from typing import List

import pandas as pd
from sklearn.metrics import max_error, explained_variance_score, r2_score
from sklearn.svm import LinearSVR


class stockSVR:
    def __init__(self) -> None:
        self.columns_predictors = {}

    def add_column_predictor(self, col: str, svr: LinearSVR) -> None:
        self.columns_predictors[col] = svr

    def predict(self, X: pd.DataFrame) -> pd.DataFrame:
        raw_results = {
            col: svr.predict(X) for col, svr in self.columns_predictors.items()
        }
        return pd.DataFrame(raw_results)

    def get_predictable_columns(self) -> List[str]:
        return self.columns_predictors.keys()

    def show_column_score(self, col: str, X_test: pd.DataFrame, y_test: pd.DataFrame) -> None:
        print(f"{col}:")
        pred = self.columns_predictors[col].predict(X_test)
        print(f"max error: {max_error(y_test, y_pred=pred)}")
        print(f"evs: {explained_variance_score(y_test, y_pred=pred)}")
        print(f"r2: {r2_score(y_test, y_pred=pred)}")
