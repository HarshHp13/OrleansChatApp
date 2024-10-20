using Grains.Events;
using Grpc.Core;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Abstractions
{
    public interface IGrpcObserver : IGrainWithGuidKey, IGrainObserver
    {
        public Task getMessages(NewMessageEvent item);
        public Task SetStream(IServerStreamWriter<MessageResponse> responseStream);

        public Task writeMessages();

        public Task getMessagesFromDb();
    }
}
