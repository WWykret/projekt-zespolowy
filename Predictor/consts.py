# COLUMNS NAMES
open_col = "Otwarcie"
close_col = "Zamkniecie"
lowest_col = "Najnizszy"
highest_col = "Najwyzszy"
date_col = "Data"

# PREDICTOR

days_back = 30#30
models_dir = "trained"
data_dir = "training_data"
min_useful_size = 1500
not_important_columns = [date_col]
repeated_columns = [open_col, highest_col, lowest_col, close_col]#, "Wolumen"]
prediction_columns = [close_col, highest_col, lowest_col]

# UTILS

# GRAPHING

draw_data_column = "Zamkniecie"
draw_time_column = "Data"