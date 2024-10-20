using Grains.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.States
{
    [GenerateSerializer]
    public record ChatroomState
    {
        [Id(0)]
        public Guid Id { get; set; }

        [Id(1)]
        public string Name { get; set; }

        [Id(2)]
        public List<NewMessageEvent> Messages { get; set; } = new List<NewMessageEvent>();

        [Id(3)]
        public List<Guid> Members { get; set; } = new List<Guid> ();
    }
}
