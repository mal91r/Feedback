import pandas as pd
import pickle
from sklearn.model_selection import train_test_split
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.svm import SVC
from sklearn.metrics import classification_report, accuracy_score

df = pd.read_csv('reviews.csv', delimiter='\t')

df = df[df['sentiment'] != 'neautral']

# Разделим данные на обучающую и тестовую выборки

X = df['review']
y = df['sentiment']
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

df.head()

# Функция для обучения и тестирования модели
def train_and_evaluate(model, X_train, X_test, y_train, y_test):
    model.fit(X_train, y_train)
    y_pred = model.predict(X_test)
    print(classification_report(y_test, y_pred))
    print("Accuracy:", accuracy_score(y_test, y_pred))

# Метод опорных векторов
svm_model = SVC()

# TF-IDF для SVM
vectorizer_tfidf = TfidfVectorizer(analyzer='char', ngram_range=(3, 5))
X_train_tfidf = vectorizer_tfidf.fit_transform(X_train)
X_test_tfidf = vectorizer_tfidf.transform(X_test)

print("SVM with TF-IDF Char 3-4-5-grams:")
train_and_evaluate(svm_model, X_train_tfidf, X_test_tfidf, y_train, y_test)

# Сохранение модели
with open('model.pkl', 'wb') as model_file:
    pickle.dump(svm_model, model_file)

print("Модель обучена и сохранена.")