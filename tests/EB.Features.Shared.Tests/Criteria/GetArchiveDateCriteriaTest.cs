using EB.Features.Shared.Criteria;
using NUnit.Framework;

namespace EB.Features.Shared.Tests.Criteria
{
    public class GetArchiveDateCriteriaTest
    {

        [Test]
        public void GetSettingKey_Always_CorrectKey()
        {
            // Arrange
            var criteria = new GetArchiveDateCriteria();

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_SelArchiveDate", act);
        }


        [Test]
        public void IsValid_Always_ReturnTrue()
        {
            // Arrange
            var criteria = new GetArchiveDateCriteria();

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }
    }
}
