using Xunit;
using Xunit.Abstractions;

namespace AmazingCalculator.Tests.Unit;

public class Tests1
{
    private readonly ITestOutputHelper _output;
    private readonly Guid _id = Guid.NewGuid();

    public Tests1(ITestOutputHelper output)
    {
        _output = output;
        //Sync setup goes here
        //_output.WriteLine("Sync Setup");
    }

    [Fact]
    public void Test1()
    {
        _output.WriteLine($"Test1: {_id}");
    }
    
    [Fact]
    public void Test2()
    {
        _output.WriteLine($"Test2: {_id}");
    }

    // public void Dispose()
    // {
    //     //Sync cleanup goes here
    //     _output.WriteLine("Sync Cleanup");
    // }
}

public class Tests2
{
    [Fact]
    public async Task Test3()
    {
        await Task.Delay(5000);
    }
    
    [Fact]
    public async Task Test4()
    {
        await Task.Delay(5000);
    }
}
