using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.Events
{
    [GenerateSerializer]
    public record NewMessageEvent
    {
        [Id(0)]
        public Guid MessaageId { get; set; }

        [Id(1)]
        public Guid SendersId { get; set; }

        [Id(2)]
        public string Message { get; set; }
    }
}
