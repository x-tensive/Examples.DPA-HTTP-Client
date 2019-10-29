using System;

namespace DpaHttpClient
{
    public class GridOrder
    {
            public long Id { get; set; }
            public bool IsMaintenance { get; set; }
            public string ProductionTypeName { get; set; }
            public string Order { get; set; }
            public string ProductName { get; set; }
            public long? ProductVersion { get; set; }
            public decimal? PlannedCount { get; set; }
            public decimal? ActualCount { get; set; }
            public decimal? RejectedCount { get; set; }
            public string Equipment { get; set; }
            public string Technology { get; set; }
            public string Stage { get; set; }
            public DateTimeOffset? PlannedStart { get; set; }
            public DateTimeOffset? PlannedEnd { get; set; }
            public DateTimeOffset? ActualStart { get; set; }
            public DateTimeOffset? ActualEnd { get; set; }
            public JobStatus Status { get; set; }
    }
}
