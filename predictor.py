import pandas as pd
from sklearn.preprocessing import scale
from sklearn.model_selection import train_test_split
from sklearn.svm import SVC, SVR, LinearSVR
from sklearn.metrics import max_error, explained_variance_score, r2_score

# CONSTS

days_back = 120

# CREATE DATA

df = pd.read_csv('training_data/11b.csv', sep=',')

df.drop(['<DATE>','<TICKER>','<PER>','<TIME>','<OPENINT>'], axis=1, inplace=True)
df.columns = ['open', 'high', 'low', 'close', 'vol']

for col_name in ['open', 'high', 'low', 'close']:
    for i in range(1, 1 + days_back):
        df[f'{col_name}-prev-{i}'] = df[col_name].shift(i)

df = df.iloc[days_back:, :]

# SPLIT INTO DATA AND RESULT

input_data = df.drop(['high', 'close'], axis=1)
output_data = df['close']

X_train, X_test, y_train, y_test = train_test_split(input_data, output_data, test_size=0.2, random_state=209)

# TRAIN AND TEST

svm_thing = LinearSVR()
svm_thing.fit(X_train, y_train)

pred = svm_thing.predict(X_test)

print(f'max error: {max_error(y_test, y_pred=pred)}')
print(f'evs: {explained_variance_score(y_test, y_pred=pred)}')
print(f'r2: {r2_score(y_test, y_pred=pred)}')

print(pd.DataFrame({'pred': pred, 'real': y_test}).head(30))