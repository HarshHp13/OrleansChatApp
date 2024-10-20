using Confluent.Kafka;
using Newtonsoft.Json;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Configurations.KafkaQueueAdapter
{
    public class KafkaStreamQueueAdapter : IQueueAdapter
    {
        private readonly IProducer<Null, string> _producer;
        public KafkaStreamQueueAdapter(IProducer<Null, string> producer)
        {
            _producer = producer;
        }
        public string Name => throw new NotImplementedException();

        public bool IsRewindable => throw new NotImplementedException();

        public StreamProviderDirection Direction => throw new NotImplementedException();

        public IQueueAdapterReceiver CreateReceiver(QueueId queueId)
        {
            throw new NotImplementedException();
        }

        public async Task QueueMessageBatchAsync<T>(StreamId streamId, IEnumerable<T> events, StreamSequenceToken token, Dictionary<string, object> requestContext)
        {

            var topic = requestContext["KafkaTopic"].ToString();
            int partition = Convert.ToInt32(requestContext["KafkaPartition"]);
            var topicPartition = new TopicPartition(topic, new Partition(partition));
            foreach (var item in events)
            {
                var message = JsonConvert.SerializeObject(item);
                var kafkaMessage = new Message<Null, string> { Value = message };

                await _producer.ProduceAsync(topicPartition, kafkaMessage);
            }
        }
    }
}
