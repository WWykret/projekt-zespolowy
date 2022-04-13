import quandl
import pandas as pd


def historicalData(name):
    mydata = quandl.get('WSE/%s' % name)
    return pd.DataFrame(mydata)