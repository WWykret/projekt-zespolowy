from datetime import date, datetime
from os.path import isfile
import os
import pickle

data_file = 'data.dta'

def get_last_scan_date() -> date:
    if not isfile(data_file):
        return date.min

    with open(data_file, 'rb') as f:
        last_scan_date = pickle.load(f)
    
    return last_scan_date

def set_last_scan_date(date: date) -> None:
    if isfile(data_file):
        os.remove(data_file)

    with open(data_file, 'wb') as f:
        last_scan_date = pickle.dump(date, f)


def get_date_from_str(date_str: str) -> date:
    return datetime.strptime(date_str, "%Y-%m-%d").date()