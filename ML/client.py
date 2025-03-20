import grpc
import tone_service_pb2
import tone_service_pb2_grpc

def run():
    with grpc.insecure_channel('localhost:5002') as channel:
        stub = tone_service_pb2_grpc.ToneServiceStub(channel)
        response = stub.GetTone(tone_service_pb2.GetToneRequest(text="ужас просто ужасно не делайте так больше"))
        print("Прогноз:", response.tone)

if __name__ == '__main__':
    run()