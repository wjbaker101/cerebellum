using Cerebellum.Api.Auth.Types;
using Cerebellum.Middleware.Authentication;
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Cerebellum.Api.Auth;

[Route("api/auth")]
public sealed class AuthController : ApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;

    }

    [HttpGet]
    [Route("log-in")]
    [DoNotAuthenticate]
    public async Task<IActionResult> LogIn([FromBody] LogInRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LogIn(request, cancellationToken);

        return ToApiResponse(result);
    }
}