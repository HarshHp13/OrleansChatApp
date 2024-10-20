using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Contracts
{
    [DataContract]
    public record UserContract
    {
        [DataMember]
        public string Name { get; set; }

    }
}
