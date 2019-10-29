using System;

namespace DpaHttpClient
{
    public class MachineStateRecord
    {
        public long Id { get; set; }
        public MachineStateType Type { get; set; }
        public int Number { get; set; }
        public Guid DriverIdentifier { get; set; }
        public Guid EventIdentifier { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset CreateOn { get; set; }
        public string EquipmentName { get; set; }
    }
}
