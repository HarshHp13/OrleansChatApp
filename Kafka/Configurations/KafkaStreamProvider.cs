using Kafka.Configurations.KafkaQueueAdapter;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kafka.Configurations
{
    public class KafkaStreamProvider : Orleans.Streams.IStreamProvider
    {
        private readonly KafkaStreamQueueAdapter _adapter;
        private readonly string _name;
        public KafkaStreamProvider(string name, KafkaStreamQueueAdapter adapter) {
            _adapter = adapter;
            _name = name;
        }
        public string Name => _name;

        public bool IsRewindable => true;

        public IAsyncStream<T> GetStream<T>(StreamId streamId)
        {
            return new KafkaAsyncStream<T>(_adapter,streamId);
        }
    }
}
