using Grains.Contracts;
using Grains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Abstractions
{
    public interface IUserGrain : IGrainWithGuidKey
    {
        public Task initialize(string name, Guid id);
        public Task sendMessage(MessageContract message);
        public Task addChatroom(Guid chatroomId);

        public Task Subscribe(IGrpcObserver observer);

        public Task subscribe(Guid chatroomId);
    }
}
