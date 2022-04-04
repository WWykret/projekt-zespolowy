from typing import List, Dict
import pandas as pd
from sklearn.svm import LinearSVR
from sklearn.preprocessing import scale
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.metrics import max_error, explained_variance_score, r2_score


def get_stock_predictor(
    stock: str,
    data: pd.DataFrame,
    prediction_column: str,
    columns_to_remove: List[str] = None,
    columns_to_remove_training: List[str] = None,
    columns_from_past_periods: List[str] = None,
    how_many_days_back: int = 30,
    verbose: bool = False,
) -> LinearSVR: # TODO zapisywanie parametrow uczenia i calych modeli
    if not columns_to_remove:
        columns_to_remove = []
    if not columns_to_remove_training:
        columns_to_remove_training = []

    data.drop(columns_to_remove, axis=1, inplace=True)

    if columns_from_past_periods:
        data = create_columns_from_pref_dates(
            data, columns_from_past_periods, how_many_days_back
        )

    # SPLIT INTO DATA AND RESULT

    input_data = data.drop(
        list(set(columns_to_remove_training + [prediction_column])), axis=1
    )
    output_data = data[prediction_column]

    X_train, X_test, y_train, y_test = train_test_split(
        input_data, output_data, test_size=0.1, random_state=209
    )

    # FIND BEST PARAMETERS

    best_model_parameters = find_best_parameters(X_train, y_train, "r2")

    # TRAIN AND TEST

    svr = LinearSVR(
        C=best_model_parameters["C"],
        epsilon=best_model_parameters["epsilon"],
        dual=False,
        loss="squared_epsilon_insensitive",
    )
    svr.fit(X_train, y_train)

    if verbose:
        display_info(best_model_parameters, svr, X_test, y_test)

    return svr


def find_best_parameters(
    X_train: pd.DataFrame, y_train: pd.DataFrame, scoring: str
) -> Dict[str, float]:
    tuned_parameters = [
        {
            "epsilon": [
                1e-6,
                1e-5,
                1e-4,
                1e-3,
                1e-2,
                1e-1,
                1,
                1e1,
                1e2,
                1e3,
                1e4,
                1e5,
                1e6,
            ],
            "C": [1e-6, 1e-5, 1e-4, 1e-3, 1e-2, 1e-1, 1, 1e1, 1e2, 1e3, 1e4, 1e5, 1e6],
        }
    ]

    clf = GridSearchCV(
        LinearSVR(dual=False, loss="squared_epsilon_insensitive"),
        tuned_parameters,
        scoring=scoring,
    )
    clf.fit(X_train, y_train)

    return clf.best_params_


def create_columns_from_pref_dates(
    data: pd.DataFrame, columns_from_past_periods: List[str], how_many_days_back: int
) -> pd.DataFrame:
    columns_to_add = []
    for col_name in columns_from_past_periods:
        for i in range(1, 1 + how_many_days_back):
            series = data[col_name].shift(i)
            col_to_add = series.to_frame()
            col_to_add.columns = columns = [f"{col_name}-prev-{i}"]
            columns_to_add.append(col_to_add)

    data = pd.concat([data, *columns_to_add], axis=1)
    data = data.iloc[how_many_days_back:, :]
    return data


def display_info(
    best_model_parameters: Dict[str, float],
    svr: LinearSVR,
    X_test: pd.DataFrame,
    y_test: pd.DataFrame,
) -> None:
    print("Best parameters set found on development set: \n")
    print(best_model_parameters)

    pred = svr.predict(X_test)

    print(f"max error: {max_error(y_test, y_pred=pred)}")
    print(f"evs: {explained_variance_score(y_test, y_pred=pred)}")
    print(f"r2: {r2_score(y_test, y_pred=pred)}")

    print(pd.DataFrame({"pred": pred, "real": y_test}).head(30))
