using MediatR;

namespace BankMin_API.Features.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, IResult>
    {
        private readonly IUserService _userService;
        public LoginCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.LoginUser(request.Email, request.Password);
                return Results.NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Unauthorized();
            }
        }
    }
}
