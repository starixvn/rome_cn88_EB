namespace EB.Features.AccountManagement.Tests.Criteria.SubAccount
{
    using EB.Features.AccountManagement.Criteria.SubAccount;
    using NUnit.Framework;

    [TestFixture]
    public class GetSuperSubAccountCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetSuperSubAccountCriteria("supertestsub01");
            var expectedSettingKey = "CS_CustSubSelInfoSuper";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("     ", false)]
        [TestCase("supertestsub01", true)]
        public void IsValid_Always_ReturnsCorrectResult(string username, bool expectedResult)
        {
            // Arrange
            var criteria = new GetSuperSubAccountCriteria(username);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
