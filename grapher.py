import matplotlib.pyplot as plt
import pandas as pd
from utils import get_date_from_str
from datetime import date, timedelta
import numpy as np
from consts import draw_data_column, draw_time_column
from predictor import get_training_data

def get_grahp_for_stock_with_code(stock_code: str, date_limit: str) -> np.array:
    dpi = 160
    side_size = 5

    data = get_training_data(stock_code)
    if data is None:
        return np.full(3*(side_size*dpi)**2, 255)

    return draw_graph(data[draw_time_column], data[draw_data_column], side_size, dpi, date_limit)



def limit_to_days_ago(xs, ys, days):
    df = pd.DataFrame({draw_time_column: xs, draw_data_column: ys})
    last_shown_date = date.today() - timedelta(days=days)
    mask = xs.apply(lambda row: get_date_from_str(row) >= last_shown_date)
    df = df.loc[mask]
    return df[draw_time_column], df[draw_data_column]

def draw_graph(xs: pd.Series, ys: pd.Series, side_size: int, dpi: int, type=None):
    if (type == "month"):
        xs, ys = limit_to_days_ago(xs, ys, 31)
    elif (type == "year"):
        xs, ys = limit_to_days_ago(xs, ys, 365)

    fig, ax = plt.subplots(figsize=(side_size,side_size), dpi=dpi)
    ax.set_xlim([get_date_from_str(xs.iloc[0]), get_date_from_str(xs.iloc[-1])])
    plt.xticks(rotation=35)
    ax.set_ylim([0, ys.max()])
    plt.tight_layout()

    num_of_points = 200
    num_of_xs = max(int(xs.shape[0] / num_of_points), 1)

    new_xs = xs.iloc[::num_of_xs]
    new_ys = ys.iloc[::num_of_xs]
    if new_xs.iloc[-1] != xs.iloc[-1]:
        new_xs = pd.concat([new_xs, xs.tail(1)])
        new_ys = pd.concat([new_ys, ys.tail(1)])

    new_xs = [get_date_from_str(x) for x in new_xs]

    ax.plot(new_xs, new_ys, "b-")

    # RETURN GRAPH AS IMG
    fig.canvas.draw()
    data = np.frombuffer(fig.canvas.tostring_rgb(), dtype=np.uint8)
    # print(len(data))
    # plt.show()
    return data
    

if __name__ == "__main__":
    df = pd.read_csv("training_data/11b.csv")
    # print(draw_graph(df[draw_time_column], df[draw_data_column], "all"))
    print(len(get_grahp_for_stock_with_code('11B', "all")))
    print(all(get_grahp_for_stock_with_code('11B', "all") == 255))
