import pandas as pd
import trainer

# CONSTS

days_back = 30

# CREATE DATA

df = pd.read_csv("training_data/ale.csv", sep=",")

# TEST

predictable_data = trainer.get_predictible_data(
    df,
    columns_to_remove=["<DATE>", "<TICKER>", "<PER>", "<TIME>", "<OPENINT>"],
    columns_from_past_periods=["<OPEN>", "<HIGH>", "<LOW>", "<CLOSE>"],
)

svr = trainer.get_stock_predictor(
    "pkp",
    predictable_data,
    prediction_columns=['<CLOSE>','<HIGH>','<LOW>'],
    verbose=True,
)
