using Auth.Domain;
using Auth.Service.Commands;
using Auth.Service.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Auth.Service.Tests
{
    public class LogoutCommandHandlerTests
    {
        //user not found
        //happy path

        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        LogoutCommandHandler _handler;

        public LogoutCommandHandlerTests()
        {
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new LogoutCommandHandler(_userRepositoryMock.Object, _refreshTokenRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnErrorWhenUserIsNotFound()
        {
            var command = new LogoutCommand("someone@abc.com");

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _handler.Handle(command, GetCancellationToken());

            result.Should().NotBeNull();
            result.Value.Should().BeFalse();
            result.ErrorMessages.Should().Contain(LogoutCommandHandler.User_Invalid);

        }


        [Fact]
        public async Task Handle_Should_Succeed()
        {
            var user = new User("someone@abc.com", "firstname", "lastname", "salt", "password", DateTime.UtcNow, DateTime.UtcNow);
            var command = new LogoutCommand("someone@abc.com");

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            var result = await _handler.Handle(command, GetCancellationToken());

            result.Should().NotBeNull();
            result.Value.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();

        }

        private static CancellationToken GetCancellationToken()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            return token;
        }
    }
}
