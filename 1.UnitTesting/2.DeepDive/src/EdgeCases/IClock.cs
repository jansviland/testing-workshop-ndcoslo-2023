namespace EdgeCases;

internal interface IClock
{
    public DateTime UtcNow { get; }
    
    public DateTime Now { get; }
}

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;
}
