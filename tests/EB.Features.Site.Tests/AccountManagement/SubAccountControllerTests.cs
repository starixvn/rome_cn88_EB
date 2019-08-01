namespace EB.Features.Site.Tests.AccountManagement
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EB.Features.Shared.Model;
    using EB.Features.Site.AccountManagement;
    using EB.Features.Site.AccountManagement.Requests.SubAccount;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubAccountControllerTests
    {
        private IMediator mediator;
        private SubAccountController subAccountController;
        private ICurrentSession currentSession;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            subAccountController = new SubAccountController(mediator);
            currentSession = Substitute.For<ICurrentSession>();
            currentSession.UserId.Returns(8);
        }

        [Test]
        public void Index_Always_ReturnsIndexView()
        {
            // Arrange & Act
            var actualView = subAccountController.Index() as ViewResult;

            // Assert
            Assert.AreEqual("~/AccountManagement/Views/SubAccount/Index.cshtml", actualView.ViewName);
        }

        [Test]
        public async Task Search_Always_CallsMediatorSendSearchQuery()
        {
            // Arrange
            var message = new SearchQuery(currentSession) { Name = "starksub01" };

            // Act
            await subAccountController.Search(message);

            // Assert
            await mediator
                .Received()
                .Send(Arg.Is<SearchQuery>(query => query.Name == message.Name));
        }

        [Test]
        public async Task Update_IsValid_CallsMediatorSendUpdateCommand()
        {
            // Arrange
            var expectedCommand = new UpdateCommand(currentSession)
            {
                CustId = 1,
                SubAccId = 2,
                PageAccess = "[D],[H],[B],[E]",
                Language = "en-US",
                Username = "starksub01"
            };

            // Act
            await subAccountController.Update(expectedCommand);

            // Assert
            await mediator
                .Received()
                .Send(Arg.Is<UpdateCommand>(actualCommand =>
                    expectedCommand.CustId == actualCommand.CustId &&
                    expectedCommand.SubAccId == actualCommand.SubAccId &&
                    expectedCommand.PageAccess == actualCommand.PageAccess &&
                    expectedCommand.Language == actualCommand.Language &&
                    expectedCommand.Username == actualCommand.Username &&
                    expectedCommand.ActorId == actualCommand.ActorId));
        }

        [Test]
        public async Task Update_Invalid_CallsMediatorSendUpdateCommand()
        {
            // Arrange
            var expectedCommand = new UpdateCommand(currentSession)
            {
                Language = "en-US"
            };

            // Act
            var actualResult = await subAccountController.Update(expectedCommand) as JsonResult;
            var actualData = actualResult.Data as Result;

            // Assert
            Assert.IsTrue(403 == actualData.Code && "You have no authorization" == actualData.Message);
        }
    }
}
