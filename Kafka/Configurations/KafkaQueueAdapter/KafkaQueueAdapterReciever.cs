using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Configurations.KafkaQueueAdapter
{
    public class KafkaQueueAdapterReciever : IQueueAdapterReceiver
    {
        public Task<IList<IBatchContainer>> GetQueueMessagesAsync(int maxCount)
        {
            throw new NotImplementedException();
        }

        public Task Initialize(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Task MessagesDeliveredAsync(IList<IBatchContainer> messages)
        {
            throw new NotImplementedException();
        }

        public Task Shutdown(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}
