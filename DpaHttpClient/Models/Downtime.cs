using System;

namespace DpaHttpClient
{
    public class Downtime
    {
        public long Id { get; set; }
        public long RecordId { get; set; }
        public long ReasonId { get; set; }
        public long EquipmentId { get; set; }
        public Reason Reason { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string OperatorComment { get; set; }
    }
}
