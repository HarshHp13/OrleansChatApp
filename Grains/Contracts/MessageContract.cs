using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Contracts
{
    [DataContract]
    [GenerateSerializer]
    public record MessageContract
    {
        [DataMember]
        [Id(0)]
        public string Message { get; set; }

        [DataMember]
        [Id(1)]
        public Guid ChatroomId { get; set; }
    }
}
