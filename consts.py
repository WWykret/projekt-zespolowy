# PREDICTOR

days_back = 30
models_dir = "trained"
data_dir = "training_data"
min_useful_size = 1500
not_important_columns = ["Data"]
repeated_columns = ["Otwarcie", "Najwyzszy", "Najnizszy", "Zamkniecie"]#, "Wolumen"]
prediction_columns = ["Zamkniecie", "Najwyzszy", "Najnizszy"]

# UTILS

# GRAPHING

draw_data_column = "Zamkniecie"
draw_time_column = "Data"