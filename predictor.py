import pandas as pd
from sklearn.preprocessing import scale
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.svm import SVC, SVR, LinearSVR
from sklearn.metrics import max_error, explained_variance_score, r2_score
from sklearn.utils import resample
from sklearn.preprocessing import StandardScaler

# CONSTS

days_back = 30

# CREATE DATA

df = pd.read_csv('training_data/pkp.csv', sep=',')

df.drop(['<DATE>','<TICKER>','<PER>','<TIME>','<OPENINT>'], axis=1, inplace=True)
df.columns = ['open', 'high', 'low', 'close', 'vol']

for col_name in ['open', 'high', 'low', 'close']:
    for i in range(1, 1 + days_back):
        df[f'{col_name}-prev-{i}'] = df[col_name].shift(i)

df = df.iloc[days_back:, :]

# SPLIT INTO DATA AND RESULT

input_data = df.drop(['high', 'close'], axis=1)
output_data = df['close']

X_train, X_test, y_train, y_test = train_test_split(input_data, output_data, test_size=0.1, random_state=209)

# FIND BEST PARAMETERS

tuned_parameters = [
    {
        'epsilon': [1e-4,1e-3,1e-2,1e-1,1,1e1,1e2,1e3,1e4],
        'C': [1e-4, 1e-3, 1e-2, 1e-1,1, 1e1, 1e2, 1e3, 1e4]
    }
]

clf = GridSearchCV(LinearSVR(dual=False, loss='squared_epsilon_insensitive'), tuned_parameters, scoring='r2')
clf.fit(X_train, y_train)

print("Best parameters set found on development set:")
print()
best_model_parameters = clf.best_params_
print(best_model_parameters)

# TRAIN AND TEST

svm_thing = LinearSVR(C=best_model_parameters['C'], epsilon=best_model_parameters['epsilon'], dual=False, loss='squared_epsilon_insensitive')
svm_thing.fit(X_train, y_train)

pred = svm_thing.predict(X_test)

print(f'max error: {max_error(y_test, y_pred=pred)}')
print(f'evs: {explained_variance_score(y_test, y_pred=pred)}')
print(f'r2: {r2_score(y_test, y_pred=pred)}')

print(pd.DataFrame({'pred': pred, 'real': y_test}).head(30))