
using Grains.Abstractions;
using Grains.Grains;
using Grpc.Core;
using Orleans;
using Proto;

namespace Silo.Services
{
    public class MessageService : ProtoMessageService.ProtoMessageServiceBase
    {
        private readonly IGrainFactory _grainFactory;
        public MessageService(IGrainFactory grainFactory) {
            _grainFactory = grainFactory;
        }
        public override async Task GetMessages(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    await responseStream.WriteAsync(new MessageResponse()
            //    {
            //        MessageId = $"{i + 1}",
            //        Message = $"Message {i + 1}",
            //        Timestamp = DateTime.Now.ToString()
            //    });
            //    Thread.Sleep(1000);
            //}

            var o = new GrpcObserver();
            await o.SetStream(responseStream);
            var observer = _grainFactory.CreateObjectReference<IGrpcObserver>(o);
            

            var userGrain = _grainFactory.GetGrain<IUserGrain>(Guid.Parse(request.UserId));
            await userGrain.Subscribe(observer);

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);  // Keep the method alive (or use a more efficient wait mechanism)
            }
        }
    }
}
