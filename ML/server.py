import grpc
from concurrent import futures
import numpy as np
import pickle
import tone_service_pb2
import tone_service_pb2_grpc


class ToneService(tone_service_pb2_grpc.ToneServiceServicer):
    def __init__(self):
        # Загрузите вашу модель машинного обучения
        with open('model.pkl', 'rb') as model_file:
            self.model = pickle.load(model_file)

    def GetTone(self, request, context):
        # Сделайте прогноз
        prediction = self.model.predict(request.text)
        return tone_service_pb2.GetToneResponse(tone=prediction[0])


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    tone_service_pb2_grpc.add_ToneServiceServicer_to_server(ToneService(), server)
    server.add_insecure_port('[::]:5002')
    server.start()
    server.wait_for_termination()


if __name__ == '__main__':
    serve()
