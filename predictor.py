import pandas as pd
import trainer
import pickle
from stock_svr import stockSVR
from os import listdir, mkdir
from os.path import isdir
import time

# CONSTS

days_back = 30

if (not isdir('trained')):
    mkdir('trained')

# all_svrs = list(filter(lambda x: x[-4:] == '.svr', listdir('trained')))
all_svrs = [file for file in listdir('trained') if file[-4:] == '.svr']

total_time = 0
for stock in ['11b', 'ale', 'cdr', 'pkn', 'pkp', 'xtb']:

    print(f'working on {stock}...')
    # CHECK IF STOCK ALREADY HAS SVR

    if f'{stock}.svr' in all_svrs:
        continue

    start = time.time()

    # LOAD RAW DATA

    df = pd.read_csv(f'training_data/{stock}.csv', sep=',')

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
        prediction_columns=['<CLOSE>','<HIGH>','<LOW>'],
        verbose=True,
    )

    with open(f'trained/{stock}.svr', 'wb') as file:
        pickle.dump(svr, file)

    end = time.time()

    print(f'time for {stock}: {end - start}')
    total_time += end - start

print(f'total time {total_time}')
    # with open('saved/pkp.svr', 'rb') as file:
    #     test = pickle.load(file)