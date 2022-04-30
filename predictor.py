import pandas as pd
import trainer
import pickle
from stock_svr import stockSVR
from os import listdir, makedirs, remove
from os.path import isdir, isfile
import time
import datetime
from utils import get_date_from_str
from consts import (
    days_back,
    models_dir,
    data_dir,
    min_useful_size,
    not_important_columns,
    repeated_columns,
    prediction_columns,
    open_col,
    close_col,
    date_col
)


def turn_data_predictible(original: pd.DataFrame) -> pd.DataFrame:
    return trainer.get_predictible_data(
        original.copy(),
        columns_to_remove=not_important_columns,
        columns_from_past_periods=repeated_columns,
        how_many_days_back=days_back
    )


def train_model(stock_symbol: str, timed: bool = False, verbose: bool = False) -> bool:
    start = time.time()

    if not has_training_data(stock_symbol):
        return False

    stock_code = stock_symbol.lower()

    print(f"Trainig {stock_code}...")

    if not isdir(models_dir):
        makedirs(models_dir)

    # REMOVE OLD SVR IF EXISTS

    if isfile(f"{models_dir}/{stock_code}.svr"):
        remove(f"{models_dir}/{stock_code}.svr")

    # LOAD RAW DATA AND PROCESS ID

    df = pd.read_csv(f"{data_dir}/{stock_code}.csv", sep=",")

    predictable_data = turn_data_predictible(df)

    print(predictable_data)

    # TRAIN MODEL

    svr = trainer.get_stock_predictor(
        predictable_data,
        prediction_columns=prediction_columns,
        verbose=verbose,
    )

    # SAVE MODEL

    with open(f"{models_dir}/{stock_code}.svr", "wb") as file:
        pickle.dump(svr, file)

    end = time.time()
    if timed:
        print(f"training time for {stock_code}: {end-start}s")

    return True


def load_model(stock_symbol: str) -> stockSVR:
    stock_code = stock_symbol.lower()

    if not is_model_trained(stock_symbol):
        return

    with open(f"{models_dir}/{stock_code}.svr", "rb") as f:
        svr = pickle.load(f)

    return svr


def is_model_trained(stock_symbol: str) -> bool:
    stock_code = stock_symbol.lower()

    if not isdir(models_dir):
        makedirs(models_dir)

    return f"{stock_code}.svr" in listdir(models_dir)


def has_training_data(stock_symbol: str) -> bool:
    stock_code = stock_symbol.lower()

    if not isdir(data_dir):
        makedirs(data_dir)

    return f"{stock_code}.csv" in listdir(data_dir)


def get_training_data(stock_symbol: str) -> pd.DataFrame:
    stock_code = stock_symbol.lower()

    if not has_training_data(stock_symbol):
        return

    return pd.read_csv(f"{data_dir}/{stock_code}.csv")


def save_training_data(stock_symbol: str, data: pd.DataFrame) -> None:
    stock_code = stock_symbol.lower()

    if not isdir(data_dir):
        makedirs(data_dir)

    data.to_csv(f"{data_dir}/{stock_code}.csv", index=False)


def generate_next_row_input(prev_row: pd.DataFrame) -> pd.DataFrame:
    pd.options.mode.chained_assignment = None
    next_input = prev_row

    for i in range(days_back - 1, 0, -1):
        for col in repeated_columns:
            next_input[f"{col}-prev-{i+1}"] = next_input[f"{col}-prev-{i}"]

    if days_back > 0:
        for col in repeated_columns:
            next_input[f"{col}-prev-1"] = next_input[col]

    next_input[open_col] = prev_row[close_col]

    return next_input.drop(prediction_columns, axis=1)

def add_predicted_row_to_data(svr: stockSVR, original: pd.DataFrame) -> pd.DataFrame:
    predictible_data = turn_data_predictible(original)
    last_row = predictible_data.tail(1)
    next_row_input = generate_next_row_input(last_row)
    prediction = pd.DataFrame(svr.predict(next_row_input))
    next_row_input = next_row_input.reset_index(drop=True)
    new_last_row = next_row_input.join(prediction)

    next_date = get_date_from_str(original[date_col].iloc[-1]) + datetime.timedelta(days=1)
    while next_date.isoweekday() in [6,7]:
        next_date += datetime.timedelta(days=1)
    new_last_row[date_col] = next_date

    new_data = pd.concat([original, new_last_row]).reset_index(drop=True)
    new_data = new_data.dropna(axis=1)
    return new_data

def get_predicted_rows_from_stock(stock_symbol: str, pred_days: int, include_original: bool) -> pd.DataFrame:
    stock_code = stock_symbol.lower()

    if not has_training_data(stock_code) or not is_model_trained(stock_code):
        return

    svr = load_model(stock_symbol)
    original_data = get_training_data(stock_symbol)

    for i in range(pred_days):
        original_data = add_predicted_row_to_data(svr, original_data)
    
    if include_original:
        return original_data
    
    return original_data.tail(pred_days)

if __name__ == '__main__':
    print(get_training_data('11b').tail(10))
    print(get_predicted_rows_from_stock('11b', 7, True).tail(10))
    print(get_predicted_rows_from_stock('11b', 7, False).tail(10))