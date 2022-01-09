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
    public class ChangePasswordCommandHandlerTests
    {

        private readonly Mock<IAuthConfiguration> _authConfigurationMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEncryptor> _encryptorMock;
        private readonly Mock<IPasswordValidator> _passwordValidatorMock;
        private readonly Mock<IEmailValidator> _emailValidatorMock;

        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            _authConfigurationMock = new Mock<IAuthConfiguration>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _encryptorMock = new Mock<IEncryptor>();
            _passwordValidatorMock = new Mock<IPasswordValidator>();
            _emailValidatorMock = new Mock<IEmailValidator>();
            _handler = new ChangePasswordCommandHandler(_authConfigurationMock.Object, _userRepositoryMock.Object, _encryptorMock.Object, _passwordValidatorMock.Object, _emailValidatorMock.Object);
        }

        [Fact]
        public async Task Handle_Should_NotChangePasswordIfUserIsNotFound()
        {
            ChangePasswordCommand command = GetChangePasswordCommandHandlerObject();

            _passwordValidatorMock.Setup(m => m.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Enumerable.Empty<string>());

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            var result = await _handler.Handle(command, GetCancellationToken());

            result.Should().NotBeNull();
            result.Value.Should().BeNull();
            result.ErrorMessages.Should().Contain(ChangePasswordCommandHandler.User_Not_Found);
        }

        [Fact]
        public async Task Handle_Should_NotChangePasswordIfOriginalPasswordDoesNotMatch()
        {
            ChangePasswordCommand command = GetChangePasswordCommandHandlerObject();
            var user = new User("someone@abc.com", "firstname", "lastname", "salt", "password", DateTime.UtcNow, DateTime.UtcNow);

            _passwordValidatorMock.Setup(m => m.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Enumerable.Empty<string>());

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _encryptorMock.Setup(m => m.ComputeHash(command.OriginalPassword, It.IsAny<string>()))
                .Returns("new hash");

            var result = await _handler.Handle(command, GetCancellationToken());

            result.Should().NotBeNull();
            result.Value.Should().BeNull();
            result.ErrorMessages.Should().Contain(ChangePasswordCommandHandler.User_Password_Not_Matched);
        }

        [Fact]
        public async Task Handle_Should_ChangePassword()
        {
            ChangePasswordCommand command = GetChangePasswordCommandHandlerObject();
            var user = new User("someone@abc.com", "firstname", "lastname", "salt", "password", DateTime.UtcNow, DateTime.UtcNow);
            var newHash = "new hash";
            var newSalt = "new salt";
            _passwordValidatorMock.Setup(m => m.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Enumerable.Empty<string>());

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _encryptorMock.Setup(m => m.ComputeHash(command.OriginalPassword, It.IsAny<string>()))
                .Returns(user.PasswordHash);
            _encryptorMock.Setup(m => m.ComputeHash(command.Password, It.IsAny<string>()))
                .Returns(newHash);
            _encryptorMock.Setup(m => m.GenerateSalt()).Returns(newSalt);

            var result = await _handler.Handle(command, GetCancellationToken());

            result.Should().NotBeNull();
            result.ErrorMessages.Should().BeEmpty();
            result.Value.Should().NotBeNull();
            result.Message.Should().Be(ChangePasswordCommandHandler.Password_Updated);

        }


        private static ChangePasswordCommand GetChangePasswordCommandHandlerObject()
        {
            return new ChangePasswordCommand("someone@abc.com", "originalPassword", "password", "repeatPassword");
        }

        private static CancellationToken GetCancellationToken()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            return token;
        }

    }

}