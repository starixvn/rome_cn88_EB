using EB.Features.Main.Params;
using EB.Features.Main.Services;
using EB.Features.Shared.Model;
using EB.Features.Site.Main.Handlers;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Main.Handlers
{
    public class UserInfoHandlerTest
    {
        private UserInfoHandler handler;
        private IUserInfoService userInfoService;
        public UserSession currentSession;

        [SetUp]
        public void SetUp()
        {
            currentSession = Substitute.For<UserSession>();
            userInfoService = Substitute.For<IUserInfoService>();
            handler = new UserInfoHandler(userInfoService);
        }


        [Test]
        public void Handle_Always_CallCorrectVariables()
        {
            // Arrange
            currentSession.UserId.Returns(1);
            var query = new ChangePasswordQuery(currentSession)
            {
                NewPassword = "newPassword",
                CurrentPassword = "currentPassword",
                ConfirmPassword = "confirmPassword"
            };

            // Act
            handler.Handle(query);

            // Assert
            userInfoService.Received().ChangePassword(Arg.Is<ChangePasswordParam>(i => i.UserId == query.CurrentSession.UserId));
            userInfoService.Received().ChangePassword(Arg.Is<ChangePasswordParam>(i => i.NewPassword == query.NewPassword));
            userInfoService.Received().ChangePassword(Arg.Is<ChangePasswordParam>(i => i.OldPassword == query.CurrentPassword));
            userInfoService.Received().ChangePassword(Arg.Is<ChangePasswordParam>(i => i.ConfirmedNewPassword == query.ConfirmPassword));
        }


        [Test]
        public void Handle_Always_ReturnCorrectMessage()
        {
            // Arrange
            var expectedMessage = new Result(0, "SU");
            userInfoService.ChangePassword(Arg.Any<ChangePasswordParam>()).Returns(expectedMessage);

            // Act
            var act = handler.Handle(new ChangePasswordQuery(currentSession));

            // Assert
            Assert.AreEqual(expectedMessage.Message, act.Result.Message);
        }
    }
}
