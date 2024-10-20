using Grains.Abstractions;
using Grains.Contracts;
using Grains.Events;
using Grains.States;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Orleans.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Grains
{
    public class UserGrain : Grain, IUserGrain, IAsyncObserver<NewMessageEvent>
    {
        private readonly IPersistentState<UserState> _userState;
        private readonly ObserverManager<IGrpcObserver> _manager;
        private readonly ILogger<UserGrain> _logger;
        public UserGrain(
            [PersistentState("User","tableStorage")]IPersistentState<UserState> userState,
            ILogger<UserGrain> logger
        ) {
            _logger = logger;
            _userState = userState;
            _manager = new ObserverManager<IGrpcObserver>(TimeSpan.FromMinutes(5), logger);
        }

        public Task Subscribe(IGrpcObserver observer)
        {
            _manager.Subscribe(observer, observer);

            return Task.CompletedTask;
        }

        //Clients use this to unsubscribe and no longer receive messages.
        public Task UnSubscribe(IGrpcObserver observer)
        {
            _manager.Unsubscribe(observer);

            return Task.CompletedTask;
        }

        public async Task addChatroom(Guid chatroomId)
        {
            _userState.State.Chatrooms.Add( chatroomId );

            await _userState.WriteStateAsync();
        }

        public async Task initialize(string name, Guid id)
        {
            _userState.State.Id = id;
            _userState.State.Name = name;

            await _userState.WriteStateAsync();
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            var streamProvider = this.GetStreamProvider("KafkaStreamProvider");
            foreach (var item in _userState.State.Chatrooms)
            {
                var stream = streamProvider.GetStream<NewMessageEvent>("Message", item);
                var handles = await stream.GetAllSubscriptionHandles();
                foreach (var handle in handles) {
                    await handle.ResumeAsync(this);
                }
            }
        }

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        public async Task OnNextAsync(NewMessageEvent item, StreamSequenceToken? token = null)
        {
            await _manager.Notify(s => s.getMessages(item));
        }

        public async Task sendMessage(MessageContract message)
        {
            var streamProvider = this.GetStreamProvider("KafkaStreamProvider");
            var stream = streamProvider.GetStream<NewMessageEvent>("Message", message.ChatroomId);

            await stream.OnNextAsync(new NewMessageEvent
            {
                MessaageId = Guid.NewGuid(),
                SendersId = _userState.State.Id,
                Message = message.Message,
            });
        }

        public async Task subscribe(Guid chatroomId)
        {
            var streamProvider = this.GetStreamProvider("KafkaStreamProvider");
            var stream = streamProvider.GetStream<NewMessageEvent>("Message", chatroomId);

            await stream.SubscribeAsync(this);
        }
    }
}
