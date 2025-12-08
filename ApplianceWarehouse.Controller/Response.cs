namespace ApplianceWarehouse.Controller;

public class Response
{
    public bool Success { get; }
    public string Message { get; }

    public Response(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public override string ToString() => $"{(Success ? "OK" : "ERROR")}: {Message}";
}