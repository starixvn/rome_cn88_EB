namespace EB.Features.AccountManagement.Tests.Criteria.SubAccount
{
    using EB.Features.AccountManagement.Criteria.SubAccount;
    using NUnit.Framework;

    [TestFixture]
    public class KickoutSubAccountCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new KickoutSubAccountCommand(1, 2);
            var expectedSettingKey = "Age_On_UserKickedout";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(-1, -2, false)]
        [TestCase(1, -2, false)]
        [TestCase(-1, 2, false)]
        [TestCase(1, 2, true)]
        public void IsValid_Always_ReturnsCorrectResult(int custId, int subAccId, bool expectedResult)
        {
            // Arrange
            var criteria = new KickoutSubAccountCommand(custId, subAccId);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
