using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Configurations.KafkaQueueAdapter
{
    public class KafkaQueueAdapterFactory : IQueueAdapterFactory
    {
        public Task<IQueueAdapter> CreateAdapter()
        {
            throw new NotImplementedException();
        }

        public Task<IStreamFailureHandler> GetDeliveryFailureHandler(QueueId queueId)
        {
            throw new NotImplementedException();
        }

        public IQueueAdapterCache GetQueueAdapterCache()
        {
            throw new NotImplementedException();
        }

        public IStreamQueueMapper GetStreamQueueMapper()
        {
            throw new NotImplementedException();
        }
    }
}
