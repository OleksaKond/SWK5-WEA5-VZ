using System;

public class Route
{
    public int RouteId { get; set; }
    public string RouteNumber { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public DateTime ValidFromDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
    public bool OperatesOnWeekdays { get; set; }
    public bool OperatesOnWeekends { get; set; }
    public bool OperatesOnHolidays { get; set; }
    public bool OperatesDuringSchoolBreaks { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}