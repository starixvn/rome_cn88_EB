namespace EB.Features.AccountManagement.Tests.Criteria.SubAccount
{
    using EB.Features.AccountManagement.Criteria.SubAccount;
    using EB.Features.AccountManagement.Params.SubAccount;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateSubAccountCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var param = new UpdateSubAccountParam(1, 2, "supertestsub01", "[D],[H],[B],[E]", "en-US", "::1", 3);
            var criteria = new UpdateSubAccountCommand(param);
            var expectedSettingKey = "CS_CustSubUpd";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(-1, -2, -3, false)]
        [TestCase(1, -2, -3, false)]
        [TestCase(-1, 2, -3, false)]
        [TestCase(1, 2, -3, false)]
        [TestCase(1, 2, 3, true)]
        public void IsValid_Always_ReturnsCorrectResult(int custId, int subAccId, int actorId, bool expectedResult)
        {
            // Arrange
            var param = new UpdateSubAccountParam(custId, subAccId, "supertestsub01", "[D],[H],[B],[E]", "en-US", "::1", actorId);
            var criteria = new UpdateSubAccountCommand(param);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void ErrCodeErrMsg_Always_AssignsCorrectValue()
        {
            // Arrange
            var param = new UpdateSubAccountParam(1, 2, "supertestsub01", "[D],[H],[B],[E]", "en-US", "::1", 3);

            // Act
            var criteria = new UpdateSubAccountCommand(param)
            {
                ErrCode = 102,
                ErrMsg = "Invalid sub account id."
            };

            // Assert
            Assert.IsTrue(criteria.ErrCode == 102 && criteria.ErrMsg == "Invalid sub account id.");
        }
    }
}
