namespace Shared.DTO;

public class TimeTableDto
{
    public List<string> Headers { get; set; } = [];
    public List<Row>? Rows { get; set; }
}

public class Row
{
    public Dictionary<string, string> Cell { get; set; } = new();
}