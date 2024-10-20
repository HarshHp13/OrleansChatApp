using Grains.Abstractions;
using Grains.Contracts;
using Grains.Events;
using Grains.States;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains
{
    public class ChatroomGrain : Grain, IChatroomGrain
    {
        private readonly IPersistentState<ChatroomState> _chatroomState;
        private readonly IClusterClient _clusterClient;
        public ChatroomGrain(
            [PersistentState("Chatroom", "tableStorage")] IPersistentState<ChatroomState> chatroomState,
            IClusterClient clusterClient
        ) {
            _chatroomState = chatroomState;
            _clusterClient = clusterClient;
        }
        public async Task initialize(Guid id, ChatroomContract chatroom)
        {
            var streamProvider = this.GetStreamProvider("KafkaStreamProvider");
            var stream = streamProvider.GetStream<NewMessageEvent>("Message", id);

            _chatroomState.State.Id = id;
            _chatroomState.State.Name = chatroom.Name;

            foreach (var item in chatroom.Members)
            {
                _chatroomState.State.Members.Add(item);
                var grain = _clusterClient.GetGrain<IUserGrain>(item);
                await grain.subscribe(id);
            }

            await _chatroomState.WriteStateAsync();
        }

        public async Task joinChatroom(Guid member)
        {
            _chatroomState.State.Members.Add(member);

            await _chatroomState.WriteStateAsync();
        }

        public async Task leaveChatroom(Guid member)
        {
            _chatroomState.State.Members.Remove(member);

            await _chatroomState.WriteStateAsync();
        }
    }
}
