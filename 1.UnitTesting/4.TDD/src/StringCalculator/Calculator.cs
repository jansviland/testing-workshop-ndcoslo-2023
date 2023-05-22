namespace StringCalculator;

public class Calculator
{
    public int Add(string numbers)
    {
        if (numbers == string.Empty)
        {
            return 0;
        }

        return numbers
            .Split(',')
            .Select(int.Parse)
            .Sum();
    }
}
