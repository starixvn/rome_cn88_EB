using EB.Features.Main.Criteria.InternalLogin;
using NUnit.Framework;

namespace EB.Features.Main.Tests.Criteria
{
    public class CheckIPOfficeCommandTest
    {

        [Test]
        public void GetSettingKey_Always_ReturnExpectedKey()
        {
            // Arrange
            var expectedKey = "CS_LoginAdminCheckGrantAccess";
            var criteria = new CheckIPOfficeCommand("razor", "127.0.0.1");

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedKey, act);
        }

        [TestCase("razor","127.0.0.1")]
        public void IsValid_CorrectParameter_ReturTrue(string userName, string loginIp)
        {
            // Arrange
            var criteria = new CheckIPOfficeCommand(userName, loginIp);

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }

        [TestCase("", "127.0.0.1")]
        [TestCase("razor", "")]
        [TestCase("", "")]
        public void IsValid_IncorrectParameter_ReturFalse(string userName, string loginIp)
        {
            // Arrange
            var criteria = new CheckIPOfficeCommand(userName, loginIp);

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }
    }
}
