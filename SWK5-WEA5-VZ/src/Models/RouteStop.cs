using System;

public class RouteStop
{
    public int RouteStopId { get; set; }
    public int RouteId { get; set; }
    public int StopId { get; set; }
    public TimeSpan ScheduledTime { get; set; }
    public int StopSequence { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
