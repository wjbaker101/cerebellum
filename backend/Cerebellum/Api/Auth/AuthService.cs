using Cerebellum.Api.Auth.Types;
using Core.Model;
using Data.Repositories;
using DotNetLibs.Core.Types;

namespace Cerebellum.Api.Auth;

public interface IAuthService
{
    Task<Result<LogInResponse>> LogIn(LogInRequest request, CancellationToken cancellationToken);
}

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ILoginTokenService _loginTokenService;

    public AuthService(IUserRepository userRepository, IPasswordService passwordService, ILoginTokenService loginTokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _loginTokenService = loginTokenService;
    }

    public async Task<Result<LogInResponse>> LogIn(LogInRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByUsername(request.Username, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<LogInResponse>.FromFailure(userResult);

        var isMatch = _passwordService.IsMatch(user.Password, request.Password, user.PasswordSalt);
        if (!isMatch)
            return Result<LogInResponse>.Failure("Unable to log in, password is incorrect. Please try again.");

        var userModel = new UserModel
        {
            Reference = user.Reference,
            CreatedAt = user.CreatedAt,
            Username = user.Username
        };

        var loginToken = _loginTokenService.Create(userModel);

        return new LogInResponse
        {
            LoginToken = loginToken,
            User = userModel
        };
    }
}