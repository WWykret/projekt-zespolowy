import matplotlib.pyplot as plt
import pandas as pd
from utils import get_date_from_str
from datetime import date, timedelta
import numpy as np
from consts import draw_data_column, draw_time_column
from predictor import get_training_data, get_predicted_rows_from_stock


def get_grahp_for_stock_with_code(stock_code: str, date_limit: str) -> np.array:
    dpi = 160
    side_size = 5

    data = get_training_data(stock_code)
    if data is None:
        return np.full(3 * (side_size * dpi) ** 2, 255)

    if date_limit != "predicted":
        return draw_graph(
            data[draw_time_column], data[draw_data_column], side_size, dpi, date_limit
        )

    predicted_data = get_predicted_rows_from_stock(stock_code, 7, False)
    # print(predicted_data)
    return draw_graph(
        data[draw_time_column], data[draw_data_column], side_size, dpi, "month", pred_xs=predicted_data[draw_time_column], pred_ys=predicted_data[draw_data_column]
    )


def limit_to_days_ago(xs, ys, days):
    df = pd.DataFrame({draw_time_column: xs, draw_data_column: ys})
    last_shown_date = date.today() - timedelta(days=days)
    mask = xs.apply(lambda row: get_date_from_str(row) >= last_shown_date)
    df = df.loc[mask]
    return df[draw_time_column], df[draw_data_column]


def draw_graph(
    xs: pd.Series,
    ys: pd.Series,
    side_size: int,
    dpi: int,
    date_limit: str = None,
    **kwargs
):
    if date_limit == "month":
        xs, ys = limit_to_days_ago(xs, ys, 31)
    elif date_limit == "year":
        xs, ys = limit_to_days_ago(xs, ys, 365)

    fig, ax = plt.subplots(figsize=(side_size, side_size), dpi=dpi)
    ax.set_xlim([get_date_from_str(xs.iloc[0]), get_date_from_str(xs.iloc[-1])])
    plt.xticks(rotation=35)
    ax.set_ylim([0, ys.max()])
    plt.tight_layout()

    if "pred_xs" in kwargs and "pred_ys" in kwargs:
        k_xs, k_ys = kwargs["pred_xs"], kwargs["pred_ys"]
        ax.set_xlim([get_date_from_str(xs.iloc[0]), get_date_from_str(k_xs.iloc[-1])])
        ax.set_ylim([0, max(k_ys.max(), ys.max())])
        ax.plot([get_date_from_str(x) for x in k_xs], k_ys, "r-")
        join_xs = pd.concat([xs.tail(1), k_xs.head(1)])
        join_ys = pd.concat([ys.tail(1), k_ys.head(1)])
        ax.plot([get_date_from_str(x) for x in join_xs], join_ys, "r-")

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
    data = np.frombuffer(fig.canvas.tostring_rgb(), dtype=np.uint8).tolist()
    # print(len(data))
    # plt.show()
    return data


if __name__ == "__main__":
    # df = pd.read_csv("training_data/11b.csv")
    # print(draw_graph(df[draw_time_column], df[draw_data_column], "all"))
    # print(len(get_grahp_for_stock_with_code("11B", "all")))
    # print(all(get_grahp_for_stock_with_code("11B", "all") == 255))
    get_grahp_for_stock_with_code('11b', "predicted")
