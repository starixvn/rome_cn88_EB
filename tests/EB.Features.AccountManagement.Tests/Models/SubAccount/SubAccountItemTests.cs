namespace EB.Features.AccountManagement.Tests.Models.SubAccount
{
    using EB.Features.AccountManagement.Models.SubAccount;
    using NUnit.Framework;

    [TestFixture]
    public class SubAccountItemTests
    {
        [Test]
        public void Constructor_Default_ReturnsCorrectProperties()
        {
            // Arrange & Act
            var subAccountItem = new SubAccountItem();

            // Assert
            Assert.IsTrue(
                subAccountItem.SMAUsername == null &&
                subAccountItem.SMANickname == null &&
                subAccountItem.CustId == 0 &&
                subAccountItem.SubAccId == 0 &&
                subAccountItem.UserName == null &&
                subAccountItem.NickName == null &&
                subAccountItem.FirstName == null &&
                subAccountItem.PageAccess == null);
        }

        [Test]
        public void Constructor_Always_ReturnsCorrectProperties()
        {
            // Arrange & Act
            var subAccountItem = new SubAccountItem("stark", "starknn", 1, 2, "starksub01", "starksub01nn", "starix", "[D],[H],[B],[E]");

            // Assert
            Assert.IsTrue(
                subAccountItem.SMAUsername == "stark" &&
                subAccountItem.SMANickname == "starknn" &&
                subAccountItem.CustId == 1 &&
                subAccountItem.SubAccId == 2 &&
                subAccountItem.UserName == "starksub01" &&
                subAccountItem.NickName == "starksub01nn" &&
                subAccountItem.FirstName == "starix" &&
                subAccountItem.PageAccess == "[D],[H],[B],[E]" &&
                subAccountItem.Permissions.ViewReports &&
                subAccountItem.Permissions.ViewTotalBets &&
                subAccountItem.Permissions.ViewMemberInfo &&
                subAccountItem.Permissions.UpdateAccount);
        }
    }
}
