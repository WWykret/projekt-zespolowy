import dataSource as ds
import predictor
# import pandas as pd

if __name__ == '__main__':
    avilable_stocks = ds.namesScraper()
    avilable_stocks.reset_index()

    for index, stock in avilable_stocks.iterrows():
        if not predictor.has_enough_training_data(stock['Symbol']):
            stock_symbol = stock['Symbol']
            print(f'{index + 1} / {avilable_stocks.shape[0]}... {stock_symbol}')
            stock_data = ds.dataScraper(stock_symbol)
            predictor.save_training_data(stock_symbol, stock_data)