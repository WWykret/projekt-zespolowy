from datetime import date, datetime
from os.path import isfile
import os
import pickle

data_file = "data.dta"
last_scan_file = "lastScans.dta"


def get_last_scan_date() -> date:
    if not isfile(data_file):
        return date.min

    with open(data_file, "rb") as f:
        last_scan_date = pickle.load(f)

    return last_scan_date


def set_last_scan_date(date: date) -> None:
    if isfile(data_file):
        os.remove(data_file)

    with open(data_file, "wb") as f:
        last_scan_date = pickle.dump(date, f)


def get_date_from_str(date_str: str) -> date:
    if isinstance(date_str, date):
        return date_str

    return datetime.strptime(date_str, "%Y-%m-%d").date()


def update_last_scan_date_for_stock(stock_symbol: str) -> None:
    stock_code = stock_symbol.lower()

    last_scan_dict = {}
    if isfile(last_scan_file):
        with open(last_scan_file, "rb") as f:
            last_scan_dict = pickle.load(f)

    last_scan_dict[stock_code] = date.today()

    with open(last_scan_file, "wb") as f:
        last_scan_dict = pickle.dump(last_scan_dict, f)


def is_stock_data_up_to_date(stock_symbol: str) -> bool:
    stock_code = stock_symbol.lower()

    if not isfile(last_scan_file):
        return False

    with open(last_scan_file, "rb") as f:
        last_scan_dict = pickle.load(f)

    last_scan_date = last_scan_dict.get(stock_symbol, date.min)

    return date.today() <= last_scan_date
