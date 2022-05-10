from typing import List, Dict

import pandas as pd
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.svm import LinearSVR

from stock_svr import stockSVR


def get_predictible_data(
        data: pd.DataFrame,
        columns_to_remove: List[str] = None,
        columns_from_past_periods: List[str] = None,
        how_many_days_back: int = 30,
) -> pd.DataFrame:
    if not columns_to_remove:
        columns_to_remove = []

    data.drop(columns_to_remove, axis=1, inplace=True)

    if columns_from_past_periods:
        data = create_columns_from_pref_dates(
            data, columns_from_past_periods, how_many_days_back
        )

    return data


def get_stock_predictor(
        stock: str,
        data: pd.DataFrame,
        prediction_columns: List[str],
        verbose: bool = False,
) -> LinearSVR:  # TODO zapisywanie parametrow uczenia i calych modeli
    input_data = data.drop(prediction_columns, axis=1)
    output_data = {col: data[col] for col in prediction_columns}

    best_parameters = {}
    ys_test = {}
    final_svr = stockSVR()
    for col in prediction_columns:
        X_train, X_test, y_train, y_test = train_test_split(
            input_data, output_data[col], test_size=0.1, random_state=209
        )
        ys_test[col] = y_test
        # FIND BEST PARAMETERS
        best_parameters[col] = find_best_parameters(X_train, y_train, "r2")

        # TRAIN AND TEST

        svr = LinearSVR(
            C=best_parameters[col]["C"],
            epsilon=best_parameters[col]["epsilon"],
            dual=False,
            loss="squared_epsilon_insensitive",
        )
        svr.fit(X_train, y_train)
        final_svr.add_column_predictor(col, svr)

    if verbose:
        display_info(best_parameters, final_svr, X_test, ys_test)

    return final_svr


def find_best_parameters(
        X_train: pd.DataFrame, y_train: pd.DataFrame, scoring: str
) -> Dict[str, float]:
    tuned_parameters = [
        {
            "epsilon": [10 ** i for i in range(-7, 8)],
            "C": [10 ** i for i in range(-7, 8)],
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
        best_model_parameters: Dict[str, Dict[str, float]],
        svr: stockSVR,
        X_test: pd.DataFrame,
        y_test: Dict[str, pd.DataFrame],
) -> None:
    print("Best parameters set found on development set: \n")
    for col, params in best_model_parameters.items():
        print(f'{col}: {params}')

    for col in svr.get_predictable_columns():
        svr.show_column_score(col, X_test, y_test[col])

    # pred = svr.predict(X_test)

    # print(pred.head(30))
