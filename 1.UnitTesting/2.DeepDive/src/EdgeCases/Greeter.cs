namespace EdgeCases;

public interface IClock
{
    DateTime Now { get; }
}

public class Greeter
{
    private readonly IClock _clock;

    public Greeter(IClock clock)
    {
        _clock = clock;
    }

    public string GenerateGreetText()
    {
        // use timeprovider, dotnet 8
        var dateTimeNow = _clock.Now;
        return dateTimeNow.Hour switch
        {
            >= 5 and < 12 => "Good morning",
            >= 12 and < 18 => "Good afternoon",
            _ => "Good evening"
        };
    }
}