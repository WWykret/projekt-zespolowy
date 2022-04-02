import pandas as pd
from sklearn.preprocessing import scale
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.svm import SVC, SVR, LinearSVR
from sklearn.metrics import max_error, explained_variance_score, r2_score
from sklearn.utils import resample

# CONSTS

days_back = 30

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

X_train, X_test, y_train, y_test = train_test_split(input_data, output_data, test_size=0.1, random_state=209)

# FIND BEST PARAMETERS

tuned_parameters = [
    {
        'epsilon': [1e-4,1e-3,1e-2,1e-1,0,1,1e1,1e2,1e3,1e4],
        'C': [0.0001, 0.001, 0.01, 0.1, 1, 10, 100, 1000, 1000],
        # 'loss': ['epsilon_insensitive', 'squared_epsilon_insensitive']
    }
]

# scores = ['r2']
# ['accuracy', 'adjusted_mutual_info_score', 'adjusted_rand_score', 'average_precision', 'balanced_accuracy', 'completeness_score', 'explained_variance', 'f1', 'f1_macro', 'f1_micro', 'f1_samples', 'f1_weighted', 'fowlkes_mallows_score', 'homogeneity_score', 'jaccard', 'jaccard_macro', 'jaccard_micro', 'jaccard_samples', 'jaccard_weighted', 'max_error', 'mutual_info_score', 'neg_brier_score', 'neg_log_loss', 'neg_mean_absolute_error', 'neg_mean_absolute_percentage_error', 'neg_mean_gamma_deviance', 'neg_mean_poisson_deviance', 'neg_mean_squared_error', 'neg_mean_squared_log_error', 'neg_median_absolute_error', 'neg_root_mean_squared_error', 'normalized_mutual_info_score', 'precision', 'precision_macro', 'precision_micro', 'precision_samples', 'precision_weighted', 'r2', 'rand_score', 'recall', 'recall_macro', 'recall_micro', 'recall_samples', 'recall_weighted', 'roc_auc', 'roc_auc_ovo', 'roc_auc_ovo_weighted', 'roc_auc_ovr', 'roc_auc_ovr_weighted', 'top_k_accuracy', 'v_measure_score']

for score in ['r2']:
    print("# Tuning hyper-parameters for %s" % score)
    print()

    clf = GridSearchCV(SVR(), tuned_parameters, scoring='r2')
    clf.fit(X_train, y_train)

    print("Best parameters set found on development set:")
    print()
    print(clf.best_params_)
    print()
    print("Grid scores on development set:")
    print()
    means = clf.cv_results_["mean_test_score"]
    stds = clf.cv_results_["std_test_score"]
    for mean, std, params in zip(means, stds, clf.cv_results_["params"]):
        print("%0.3f (+/-%0.03f) for %r" % (mean, std * 2, params))
    print()

    print("Detailed classification report:")
    print()
    print("The model is trained on the full development set.")
    print("The scores are computed on the full evaluation set.")
    print()
    y_true, y_pred = y_test, clf.predict(X_test)
    print(classification_report(y_true, y_pred))
    print()

# TRAIN AND TEST

# svm_thing = LinearSVR(C=1000, epsilon=0.01)
# svm_thing.fit(X_train, y_train)

# pred = svm_thing.predict(X_test)

# print(f'max error: {max_error(y_test, y_pred=pred)}')
# print(f'evs: {explained_variance_score(y_test, y_pred=pred)}')
# print(f'r2: {r2_score(y_test, y_pred=pred)}')

# print(pd.DataFrame({'pred': pred, 'real': y_test}).head(30))