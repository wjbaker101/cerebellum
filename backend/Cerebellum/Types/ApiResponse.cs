namespace Cerebellum.Types;

public abstract class ApiResponse
{
}

public sealed class ApiResultResponse<TContent> : ApiResponse
{
    public TContent Result { get; }

    public ApiResultResponse(TContent result)
    {
        Result = result;
    }
}

public sealed class ApiErrorResponse : ApiResponse
{
    public string ErrorMessage { get; }

    public ApiErrorResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}