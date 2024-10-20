using Confluent.Kafka;
using Kafka.Configurations.KafkaQueueAdapter;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Configurations
{
    public class KafkaAsyncStream<T> : IAsyncStream<T>
    {
        private readonly KafkaStreamQueueAdapter _queueAdapter;
        private StreamId _streamId;
        public KafkaAsyncStream(KafkaStreamQueueAdapter queueAdapter, StreamId streamId) { 
            _queueAdapter = queueAdapter;
            _streamId = streamId;
        }
        public bool IsRewindable => throw new NotImplementedException();

        public string ProviderName => throw new NotImplementedException();

        public StreamId StreamId => _streamId;

        public int CompareTo(IAsyncStream<T>? other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IAsyncStream<T>? other)
        {
            throw new NotImplementedException();
        }

        public Task<IList<StreamSubscriptionHandle<T>>> GetAllSubscriptionHandles()
        {
            throw new NotImplementedException();
        }

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task OnNextAsync(T item, StreamSequenceToken? token = null)
        {
            throw new NotImplementedException();
        }

        public async Task OnNextAsync(T item, string topic, int partition, StreamSequenceToken? token = null)
        {
            var events = new[] { item };
            var requestContext = new Dictionary<string, object>();
            requestContext.Add("KafkaTopic", topic);
            requestContext.Add("KafkaPartition", partition);
            await _queueAdapter.QueueMessageBatchAsync(_streamId, events, token, requestContext);
        }

        public Task OnNextBatchAsync(IEnumerable<T> batch, StreamSequenceToken token = null)
        {
            throw new NotImplementedException();
        }

        public Task<StreamSubscriptionHandle<T>> SubscribeAsync(IAsyncObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        public Task<StreamSubscriptionHandle<T>> SubscribeAsync(IAsyncObserver<T> observer, string topic, int partition)
        {
            throw new NotImplementedException();
        }

        public Task<StreamSubscriptionHandle<T>> SubscribeAsync(IAsyncObserver<T> observer, StreamSequenceToken? token, string? filterData = null)
        {
            throw new NotImplementedException();
        }

        public Task<StreamSubscriptionHandle<T>> SubscribeAsync(IAsyncBatchObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        public Task<StreamSubscriptionHandle<T>> SubscribeAsync(IAsyncBatchObserver<T> observer, StreamSequenceToken? token)
        {
            throw new NotImplementedException();
        }
    }
}
