using System;

namespace DpaHttpClient
{
    public class ProcessingProgramRecord
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid DriverIdentifier { get; set; }
        public Guid EventIdentifier { get; set; }
        public string EquipmentName { get; set; }
        public int Number { get; set; }
        public string MainProgram { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public TimeSpan? TotalTime { get; set; }
        public TimeSpan? MachineTime { get; set; }
        public MachineProgramStatus? FinalStatus { get; set; }
        public TimeSpan? AssessmentTime { get; set; }
    }
}
