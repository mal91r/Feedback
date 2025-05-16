import grpc
from concurrent import futures
import numpy as np
import pickle
import tone_service_pb2
import tone_service_pb2_grpc
from sklearn.feature_extraction.text import TfidfVectorizer


class ToneService(tone_service_pb2_grpc.ToneServiceServicer):
    def __init__(self):
        # Загрузите вашу модель машинного обучения
        with open('model.pkl', 'rb') as model_file:
            self.model = pickle.load(model_file)

    def GetTone(self, request, context):
        # Загрузка
        with open('vectorizer.pkl', 'rb') as f:
            loaded_vectorizer = pickle.load(f)
            # Препроцессинг: преобразуйте текст в векторы
            text_vectorized = loaded_vectorizer.transform([request.text])
            # Сделайте прогноз
            prediction = self.model.predict(text_vectorized)
            if prediction[0] == 'positive':
                tone = tone_service_pb2.TONE_POSITIVE
            if prediction[0] == 'negative':
                tone = tone_service_pb2.TONE_NEGATIVE
            return tone_service_pb2.GetToneResponse(tone=tone)


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    tone_service_pb2_grpc.add_ToneServiceServicer_to_server(ToneService(), server)
    server.add_insecure_port('0.0.0.0:5002')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    serve()
