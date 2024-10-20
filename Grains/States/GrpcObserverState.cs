using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.States
{
    [GenerateSerializer]
    public record GrpcObserverState
    {
        [Id(0)]
        public string lastRead {  get; set; }
    }
}
