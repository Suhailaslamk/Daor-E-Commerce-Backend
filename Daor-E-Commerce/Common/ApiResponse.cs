namespace Daor_E_Commerce.Common
    { 
public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    public ApiResponse(int statusCode, string message, T? data = default)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        IsSuccess = statusCode >= 200 && statusCode < 300;
    }
}
}

