import dataSource as ds
import predictor
from datetime import date
import utils

if __name__ == '__main__':
    avilable_stocks = ds.namesScraper()
    avilable_stocks.reset_index()

    today = date.today()
    last_scan_date = utils.get_last_scan_date()

    for index, stock in avilable_stocks.iterrows():
        if not predictor.has_training_data(stock['Symbol']) or (last_scan_date < today):
            stock_symbol = stock['Symbol']
            print(f'{index + 1} / {avilable_stocks.shape[0]}... {stock_symbol}')
            stock_data = ds.dataScraper(stock_symbol)
            predictor.save_training_data(stock_symbol, stock_data)

    utils.set_last_scan_date(today)
    