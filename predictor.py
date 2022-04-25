import pandas as pd
import trainer
import pickle
from stock_svr import stockSVR
from os import listdir, mkdir, remove
from os.path import isdir, isfile
import time
from consts import days_back, models_dir, data_dir, min_useful_size, not_important_columns, repeated_columns, prediction_columns

def train_model(stock_symbol: str, timed: bool=False, verbose: bool=False) -> bool:
    start = time.time()

    if not has_training_data(stock_symbol):
        return False

    stock_code = stock_symbol.lower()

    print(f"Trainig {stock_code}...")

    if not isdir(models_dir):
        mkdir(models_dir)

    # REMOVE OLD SVR IF EXISTS

    if isfile(f"{models_dir}/{stock_code}.svr"):
        remove(f"{models_dir}/{stock_code}.svr")

    # LOAD RAW DATA AND PROCESS ID

    df = pd.read_csv(f"{data_dir}/{stock_code}.csv", sep=",")

    predictable_data = trainer.get_predictible_data(
        df,
        columns_to_remove=not_important_columns,
        columns_from_past_periods=repeated_columns,
    )

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
    if (timed):
        print(f"training time for {stock_code}: {end-start}s")

    return True


def is_model_trained(stock_symbol: str) -> bool:
    stock_code = stock_symbol.lower()

    if not isdir(models_dir):
        mkdir(models_dir)

    return f"{stock_code}.svr" in listdir(models_dir)


def has_training_data(stock_symbol: str) -> bool:
    stock_code = stock_symbol.lower()

    if not isdir(data_dir):
        mkdir(data_dir)

    return f"{stock_code}.csv" in listdir(data_dir)


def save_training_data(stock_symbol: str, data: pd.DataFrame) -> None:
    stock_code = stock_symbol.lower()

    if not isdir(data_dir):
        mkdir(data_dir)

    data.to_csv(f"{data_dir}/{stock_code}.csv", index=False)
