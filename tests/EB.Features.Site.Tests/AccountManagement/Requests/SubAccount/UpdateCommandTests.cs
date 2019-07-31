namespace EB.Features.Site.Tests.AccountManagement.Requests.SubAccount
{
    using EB.Features.Site.AccountManagement.Requests.SubAccount;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Web.Session;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateCommandTests
    {
        private UserSession currentSession;

        [SetUp]
        public void SetUp()
        {
            currentSession = Substitute.For<UserSession>();
        }

        [Test]
        public void Constructor_Always_ReturnsCorrectActorValue()
        {
            // Arrange
            currentSession.UserId.Returns(8);

            // Act
            var actualResult = new UpdateCommand(currentSession);

            // Assert
            Assert.AreEqual(8, actualResult.ActorId);
        }

        [Test]
        public void Properties_Always_ReturnsCorrectValues()
        {
            // Arrange
            currentSession.UserId.Returns(8);

            // Act
            var actualResult = new UpdateCommand(currentSession)
            {
                CustId = 1,
                SubAccId = 2,
                PageAccess = "[D],[H],[B],[E]",
                Language = "en-US",
                Username = "starksub01"
            };

            // Assert
            Assert.IsTrue(1 == actualResult.CustId &&
                2 == actualResult.SubAccId &&
                "[D],[H],[B],[E]" == actualResult.PageAccess &&
                "en-US" == actualResult.Language &&
                "starksub01" == actualResult.Username);
        }

        [TestCase(0, 2, 8, "starksub01", false)]
        [TestCase(1, 0, 8, "starksub01", false)]
        [TestCase(1, 2, 0, "starksub01", false)]
        [TestCase(1, 2, 8, null, false)]
        [TestCase(1, 2, 8, "starksub01", true)]
        public void IsValid_Always_ReturnsCorrectValue(int custId, int subAccId, int actorId, string username, bool expectedResult)
        {
            // Arrange
            currentSession.UserId.Returns(actorId);
            
            var command = new UpdateCommand(currentSession)
            {
                CustId = custId,
                SubAccId = subAccId,
                Username = username
            };

            // Act
            var actualResult = command.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
