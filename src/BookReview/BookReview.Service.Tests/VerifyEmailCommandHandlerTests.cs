using BookReview.Domain;
using BookReview.Service.Commands;
using BookReview.Service.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookReview.Service.Tests
{
    public class VerifyEmailCommandHandlerTests
    {
        //user not found
        //user eamil already verified
        //code missing/does not match
        //happy path

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEmailVerificationRepository> _emailVerificationRepositoryMock;
        private readonly Mock<IEmailValidator> _emailValidatorMock;

        VerifyEmailCommandHandler _handler;

        public VerifyEmailCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailVerificationRepositoryMock = new Mock<IEmailVerificationRepository>();
            _emailValidatorMock = new Mock<IEmailValidator>();

            _handler = new VerifyEmailCommandHandler(_userRepositoryMock.Object, _emailVerificationRepositoryMock.Object, _emailValidatorMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnErrorIfUserNotFound()
        {

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);
                        
            var command = new VerifyEmailCommand("someone@abc.com", "code");

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Value.Should().BeFalse();
            result.ErrorMessages.Should().Contain(VerifyEmailCommandHandler.Unable_To_Verify);

        }

        [Fact]
        public async Task Handle_Should_SucceedForVerifiedUser()
        {
            var user = new User("someone@abc.com", "firstname", "lastname", true, "salt", "password", DateTime.UtcNow, DateTime.UtcNow);
            
            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            var command = new VerifyEmailCommand("someone@abc.com", "code");

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Value.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Theory]
        [InlineData("different code")]
        [InlineData(null)]
        public async Task Handle_Should_ReturnErrorWhenCodeDoNotIsIncorrect(string code)
        {
            var user = new User("someone@abc.com", "firstname", "lastname", false, "salt", "password", DateTime.UtcNow, DateTime.UtcNow);

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _emailVerificationRepositoryMock.Setup(m => m.GetCode(It.IsAny<string>()))
                .ReturnsAsync(code);

            var command = new VerifyEmailCommand("someone@abc.com", "code");

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Value.Should().BeFalse();
            result.ErrorMessages.Should().Contain(VerifyEmailCommandHandler.Unable_To_Verify);
        }

        [Fact]
        public async Task Handle_Should_Succeed()
        {
            var code = "verification code";
            var user = new User("someone@abc.com", "firstname", "lastname", false, "salt", "password", DateTime.UtcNow, DateTime.UtcNow);

            _emailValidatorMock.Setup(m => m.Validate(It.IsAny<string>()))
                .Returns(true);

            _userRepositoryMock.Setup(m => m.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _emailVerificationRepositoryMock.Setup(m => m.GetCode(It.IsAny<string>()))
                .ReturnsAsync(code);

            var command = new VerifyEmailCommand("someone@abc.com", code);

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Value.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }
    }
}
