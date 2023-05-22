using System;

namespace LegacyApp;

public interface IClock
{
    DateTime Now { get; }
}

public class Clock : IClock
{
    public DateTime Now => DateTime.Now;
}