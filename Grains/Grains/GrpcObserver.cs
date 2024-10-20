using Grains.Abstractions;
using Grains.Events;
using Grains.States;
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
        private readonly IPersistentState<GrpcObserverState> _state;
        private IServerStreamWriter<MessageResponse> _responseStream;
        private List<NewMessageEvent> liveMessages;
        private List<NewMessageEvent> dbMessages;

        public GrpcObserver([PersistentState("Observer","tableStorage")] IPersistentState<GrpcObserverState> state)
        {
            _state = state;
        }

        public async Task SetStream(IServerStreamWriter<MessageResponse> responseStream)
        {
            _responseStream = responseStream;
            liveMessages = new List<NewMessageEvent>();
            dbMessages = new List<NewMessageEvent>();
        }

        public async Task getMessages(NewMessageEvent item)
        {
            liveMessages.Add(item);
            await getMessagesFromDb();

            await writeMessages();
        }

        public async Task writeMessages()
        {
            foreach (var item in dbMessages)
            {
                if (item.Equals(liveMessages.First())) break;
                var ts = DateTime.Now.ToString();
                await _responseStream.WriteAsync(new MessageResponse
                {
                    MessageId = item.MessaageId.ToString(),
                    Message = item.Message,
                    Timestamp = ts
                });
                _state.State.lastRead = ts;
                await _state.WriteStateAsync();
            }
            dbMessages.Clear();
            foreach (var item in liveMessages)
            {
                var ts = DateTime.Now.ToString();
                await _responseStream.WriteAsync(new MessageResponse
                {
                    MessageId = item.MessaageId.ToString(),
                    Message = item.Message,
                    Timestamp = DateTime.Now.ToString()
                });
                _state.State.lastRead = ts;
                await _state.WriteStateAsync();
            }
            liveMessages.Clear();
            
        }

        public async Task getMessagesFromDb()
        {

            dbMessages = new List<NewMessageEvent>();
        }
    }
}
