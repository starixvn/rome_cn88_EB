namespace EB.Features.AccountManagement.Tests.Params.SubAccount
{
    using EB.Features.AccountManagement.Params.SubAccount;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateSubAccountParamTests
    {
        [Test]
        public void Constructor_Always_ReturnsCorrectProperties()
        {
            // Arrange & Act
            var subAccountItem = new UpdateSubAccountParam(1, 2, "starksub01", "[D],[H],[B],[E]", "EN", "::1", 3);

            // Assert
            Assert.IsTrue(
                subAccountItem.CustId == 1 &&
                subAccountItem.SubAccId == 2 &&
                subAccountItem.Username == "starksub01" &&
                subAccountItem.Name == null &&
                subAccountItem.PageAccess == "[D],[H],[B],[E]" &&
                subAccountItem.LanguageKey == "EN" &&
                subAccountItem.IP == "::1" &&
                subAccountItem.ActorId == 3);
        }
    }
}
