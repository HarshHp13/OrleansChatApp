using Grains.Abstractions;
using Grains.Events;
using Grpc.Core;
using Orleans;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains
{
    public class GrpcObserver : Grain, IGrpcObserver
    {
        private IServerStreamWriter<MessageResponse> _responseStream;
        
        public async Task SetStream(IServerStreamWriter<MessageResponse> responseStream)
        {
            _responseStream = responseStream;
        }

        public async Task getMessages(NewMessageEvent item)
        {
            await _responseStream.WriteAsync(new MessageResponse
            {
                MessageId = item.MessaageId.ToString(),
                Message = item.Message,
                Timestamp = DateTime.Now.ToString()
            });
        }
    }
}
