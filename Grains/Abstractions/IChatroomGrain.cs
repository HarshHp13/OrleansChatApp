using Grains.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Abstractions
{
    public interface IChatroomGrain : IGrainWithGuidKey
    {
        public Task initialize(Guid id, ChatroomContract chatroom);
        public Task joinChatroom(Guid member);
        public Task leaveChatroom(Guid member);

    }
}
