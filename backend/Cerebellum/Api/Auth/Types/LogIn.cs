using Core.Model;

namespace Cerebellum.Api.Auth.Types;

public sealed class LogInRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public sealed class LogInResponse
{
    public required string LoginToken { get; init; }
    public required UserModel User { get; init; }
}