namespace EB.Features.Site.Tests.AccountManagement.Handlers
{
    using System.Threading.Tasks;
    using EB.Features.AccountManagement.Params.SubAccount;
    using EB.Features.AccountManagement.Services;
    using EB.Features.Site.AccountManagement.Handlers;
    using EB.Features.Site.AccountManagement.Requests.SubAccount;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Web.Session;
    using NSubstitute;
    using NUnit.Framework;

    public class SubAccountHandlerTests
    {
        UserSession currentSession;
        private SubAccountService subAccountService;
        private SubAccountHandler subAccountHandler;

        [SetUp]
        public void SetUp()
        {
            currentSession = Substitute.For<UserSession>();
            currentSession.UserId.Returns(8);

            subAccountService = Substitute.For<SubAccountService>();
            subAccountHandler = new SubAccountHandler(subAccountService);
        }

        [Test]
        public async Task HandleSearchQuery_Always_CallsGetSuperSubAccountAsync()
        {
            // Arrange
            var message = new SearchQuery(currentSession) { Name = "starksub01" };

            // Act
            await subAccountHandler.Handle(message);

            // Assert
            await subAccountService
                .Received()
                .GetSuperSubAccountAsync(Arg.Is<string>(cmd => cmd == message.Name));
        }


        [Test]
        public async Task HandleUpdateCommand_Always_CallServiceUpdate()
        {
            // Arrange
            var message = new UpdateCommand(currentSession);

            // Act
            await subAccountHandler.Handle(message);

            // Assert
            await subAccountService.UpdateSubAccountAsync(Arg.Any<UpdateSubAccountParam>());
        }
    }
}
