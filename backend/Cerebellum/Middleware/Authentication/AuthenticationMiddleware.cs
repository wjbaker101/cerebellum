using Cerebellum.Api.Auth;
using Data.Repositories;
using System.Net;

namespace Cerebellum.Middleware.Authentication;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AuthenticateAttribute : Attribute
{
}

public sealed class AuthenticationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var attribute = context.GetEndpoint()?.Metadata.GetMetadata<AuthenticateAttribute>();
        if (attribute == null)
        {
            await next(context);
            return;
        }

        var cancellationToken = context.RequestAborted;

        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            Unauthorized(context);
            //await next(context);
            return;
        }

        if (!authHeader.ToString().StartsWith("Bearer "))
        {
            Unauthorized(context);
            return;
        }

        var bearer = authHeader.ToString().Split("Bearer ")[1];

        var loginTokenService = context.RequestServices.GetRequiredService<ILoginTokenService>();

        var userReferenceResult = loginTokenService.GetUserReferenceByToken(bearer);
        if (userReferenceResult.IsFailure)
        {
            Unauthorized(context);
            return;
        }

        var userRepository = context.RequestServices.GetRequiredService<IUserRepository>();

        var userResult = await userRepository.GetByReference(userReferenceResult.Value, cancellationToken);
        if (!userResult.TrySuccess(out var user))
        {
            Unauthorized(context);
            return;
        }

        context.Items[RequestHelper.REQUEST_USER_ITEM_KEY] = new RequestUser
        {
            Id = user.Id,
            Reference = user.Reference
        };

        await next(context);
    }

    private static void Unauthorized(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}