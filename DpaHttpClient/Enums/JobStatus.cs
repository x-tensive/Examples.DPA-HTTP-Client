namespace DpaHttpClient
{
    public enum JobStatus
    {
        New = 0,
        Scheduled = 1,
        Started = 2,
        Completed = 3,
        Canceled = 4,
        Faulted = 5,
        Assigned = 6,
        Paused = 7
    }
}
