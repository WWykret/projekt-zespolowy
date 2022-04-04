import pandas as pd
import trainer

# CONSTS

days_back = 30

# CREATE DATA

df = pd.read_csv("training_data/pkp.csv", sep=",")

# TEST

svr = trainer.get_stock_predictor(
    "pkp",
    df,
    "<CLOSE>",
    columns_to_remove=["<DATE>", "<TICKER>", "<PER>", "<TIME>", "<OPENINT>"],
    columns_to_remove_training=["<HIGH>", "<CLOSE>",'<LOW>'],
    columns_from_past_periods=["<OPEN>", "<HIGH>", "<LOW>", "<CLOSE>"],
    verbose=True
)