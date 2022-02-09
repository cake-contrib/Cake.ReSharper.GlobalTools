namespace Sample;

public sealed class SomeClass : IDisposable
{
    public void DoNothing()
    {

    }

    public void ResultError(int count)
    {
#pragma warning disable CA2241 // Provide correct arguments to formatting methods
        Console.WriteLine("{0} {1}", count);
#pragma warning restore CA2241 // Provide correct arguments to formatting methods
    }


    public void Dispose()
    {
    }
}
