using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;
using MediatR;

namespace BankMin_API.Features.Auth.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, IResult>
    {
        private readonly IHashService _hashService;
        private readonly IUserService _userService;

        public SignUpCommandHandler(IHashService hashService, IUserService userService)
        {
            _hashService = hashService;
            _userService = userService;
        }

        public async Task<IResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Password = _hashService.Hash(request.Password),
            };
            try
            {
                await _userService.SignUpUser(user);
                return Results.Created();
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Sign up failed"
                );
            }
        }
    }
}
