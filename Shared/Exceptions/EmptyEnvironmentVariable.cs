namespace Shared.Exceptions;

public class EmptyEnvironmentVariable : Exception
{
    public EmptyEnvironmentVariable()
    {
    }

    public EmptyEnvironmentVariable(string variableName) : base($"Empty/invalid environment variable '{variableName}'.")
    {
    }
}