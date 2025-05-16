# Выбор базового образа с Python
FROM python:3.9-slim

# Установка рабочей директории
WORKDIR /app

# Копирование файлов проекта
COPY . /app

# Установка зависимостей
RUN pip install -r requirements.txt

# Открытие порта для взаимодействия с приложением
EXPOSE 5002

# Запуск приложения
CMD ["python", "server.py"]