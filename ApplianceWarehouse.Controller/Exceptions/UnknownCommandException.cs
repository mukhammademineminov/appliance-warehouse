namespace ApplianceWarehouse.Controller.Exceptions;

public class UnknownCommandException : Exception
{
    public UnknownCommandException() { }

    public UnknownCommandException(string message) : base(message) { }

    public UnknownCommandException(string message, Exception inner) : base(message, inner) { }
}
