namespace StringCalculator;

public static class StringCalculator
{
    public static int Add(string numbers)
    {
        var split = numbers.Split(',');

        var a = split[0];
        var b = split[1];

        var result = int.Parse(a) + int.Parse(b);

        return result;
    }
}