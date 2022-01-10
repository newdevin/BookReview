using Auth.Domain;
using Auth.Service.Commands;
using Auth.Service.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace Auth.Service.Tests
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IAuthConfiguration> _authConfigurationMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEncryptor> _encryptorMock;
        private readonly Mock<IPasswordValidator> _passwordValidatorMock;
        private readonly Mock<IEmailValidator> _emailValidatorMock;
        private readonly Mock<IEmailVerificationRepository> _emailVerificationRepositoryMock;

        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _authConfigurationMock = new Mock<IAuthConfiguration>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _encryptorMock = new Mock<IEncryptor>();
            _passwordValidatorMock = new Mock<IPasswordValidator>();
            _emailValidatorMock = new Mock<IEmailValidator>();
            _emailVerificationRepositoryMock = new Mock<IEmailVerificationRepository>();
            _handler = new RegisterCommandHandler(_authConfigurationMock.Object, _userRepositoryMock.Object, _encryptorMock.Object, _passwordValidatorMock.Object,_emailValidatorMock.Object, _emailVerificationRepositoryMock.Object);
        }


        [Fact]
        public async Task Handle_ShouldReturnErrorIfUserAlreadyExists()
        {
            _passwordValidatorMock.Setup(m => m.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Enumerable.Empty<string>());

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync(new User("someone@abc.com", "firstname", "lastname", false, "salt", "password", DateTime.UtcNow, DateTime.UtcNow));

            var command = GetRegisterCommandObject();
            
            var result = await _handler.Handle(command, GetCancellationToken());

            result.Should().NotBeNull();
            result.Value.Should().BeNull();
            result.ErrorMessages.Should().Contain(RegisterCommandHandler.User_Exists);

        }

        private static CancellationToken GetCancellationToken()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            return token;
        }

        [Fact]
        public async Task Handle_Should_RegisterUser()
        {
            var command = GetRegisterCommandObject();

            _passwordValidatorMock.Setup(m => m.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Enumerable.Empty<string>());

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(m => m.Insert(It.IsAny<User>()))
               .ReturnsAsync(new User("someone@abc.com", "firstname", "lastname", false, "salt", "password", DateTime.UtcNow, DateTime.UtcNow));

            var result = await _handler.Handle(command, GetCancellationToken());

            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.ErrorMessages.Should().BeEmpty();

        }

        private static RegisterCommand GetRegisterCommandObject()
        {
            return new("someone@abc.com", "firstname","lastname", "abc", "abc");
        }
    }

}