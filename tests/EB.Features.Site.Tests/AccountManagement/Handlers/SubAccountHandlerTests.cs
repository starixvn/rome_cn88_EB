namespace EB.Features.Site.Tests.AccountManagement.Handlers
{
    using System.Threading.Tasks;
    using EB.Features.AccountManagement.Services;
    using EB.Features.Site.AccountManagement.Handlers;
    using EB.Features.Site.AccountManagement.Requests.SubAccount;
    using Fanex.NAP.Core.Web.Session;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SubAccountHandlerTests
    {
        ICurrentSession currentSession;
        private SubAccountService subAccountService;
        private SubAccountHandler subAccountHandler;

        [SetUp]
        public void SetUp()
        {
            currentSession = Substitute.For<ICurrentSession>();
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
    }
}
