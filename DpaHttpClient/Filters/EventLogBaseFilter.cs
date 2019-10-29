using System;
using System.Collections.Generic;

namespace DpaHttpClient
{
    public class EventLogBaseFilter
    {
        private DateTimeOffset dateTimeOffsetFrom;
        private DateTimeOffset dateTimeOffsetUntil;
        public long EquipmentId { get; set; }
        public int? Channel { get; set; }
        public int Count { get; set; }
        public List<long> ItemIds { get; set; } = new List<long>();
        public bool ShowUnclassified { get; set; }
        public bool GroupBy { get; set; }
        public bool NotTruncateDatePeriod { get; set; }
        public List<long> RecordIds { get; set; }
        public List<long> SelectedShiftIds { get; set; }
        public bool ConsiderWholeShift { get; set; }
        public List<long> SelectedTypes { get; set; }
        public bool ShowMaintenance { get; set; }
        public bool ShowRelease { get; set; }
        public FilterPeriod PeriodType { get; set; }
        public bool ShowServerState { get; set; }
        public DateTimeOffset ClientDateTimeOffsetUntil { get; private set; }
        public DateTimeOffset DateTimeFrom { get { return DateTimeOffsetFrom; } set { DateTimeOffsetFrom = value; } }
        public DateTimeOffset DateTimeUntil { get { return DateTimeOffsetUntil; } set { DateTimeOffsetUntil = value; } }
        public DateTimeOffset DateTimeOffsetFrom
        {
            get => dateTimeOffsetFrom.LocalDateTime;
            set => dateTimeOffsetFrom = value;
        }
        public DateTimeOffset DateTimeOffsetUntil
        {
            get => dateTimeOffsetUntil.LocalDateTime;
            set
            {
                ClientDateTimeOffsetUntil = value;
                dateTimeOffsetUntil = value;
            }
        }
    }
}
