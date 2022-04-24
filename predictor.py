import pandas as pd
import trainer
import pickle
from stock_svr import stockSVR
from os import listdir, mkdir
from os.path import isdir
import time

# CONSTS

days_back = 30
models_dir = "trained"
data_dir = "training_data"
min_useful_size = 1500


def func():
    if not isdir(models_dir):
        mkdir(models_dir)

    # all_svrs = list(filter(lambda x: x[-4:] == '.svr', listdir(models_dir)))
    all_svrs = [file for file in listdir(models_dir) if file[-4:] == ".svr"]

    total_time = 0
    for stock in ["11b", "ale", "cdr", "pkn", "pkp", "xtb"]:

        print(f"working on {stock}...")
        # CHECK IF STOCK ALREADY HAS SVR

        if f"{stock}.svr" in all_svrs:
            continue

        start = time.time()

        # LOAD RAW DATA

        df = pd.read_csv(f"{data_dir}/{stock}.csv", sep=",")

        # MODIFY DATA

        predictable_data = trainer.get_predictible_data(
            df,
            columns_to_remove=["<DATE>", "<TICKER>", "<PER>", "<TIME>", "<OPENINT>"],
            columns_from_past_periods=["<OPEN>", "<HIGH>", "<LOW>", "<CLOSE>"],
        )

        # TRAIN MODEL

        svr = trainer.get_stock_predictor(
            stock,
            predictable_data,
            prediction_columns=["<CLOSE>", "<HIGH>", "<LOW>"],
            verbose=True,
        )

        with open(f"{models_dir}/{stock}.svr", "wb") as file:
            pickle.dump(svr, file)

        end = time.time()

        print(f"time for {stock}: {end - start}")
        total_time += end - start

    print(f"total time {total_time}")
    # with open('saved/pkp.svr', 'rb') as file:
    #     test = pickle.load(file)


def is_model_trained(stock_symbol: str) -> bool:
    pass


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
