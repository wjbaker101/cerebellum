﻿using Cerebellum.Api.Auth.Types;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace Cerebellum.Api.Auth;

[Route("api/auth")]
public sealed class AuthController : ApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;

    }

    [HttpPost]
    [Route("log-in")]
    public async Task<IActionResult> LogIn([FromBody] LogInRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LogIn(request, cancellationToken);

        return ToApiResponse(result);
    }
}