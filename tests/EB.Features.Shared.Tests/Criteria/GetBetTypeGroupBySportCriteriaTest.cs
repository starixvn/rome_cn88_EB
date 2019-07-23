using EB.Features.Shared.Criteria;
using NUnit.Framework;

namespace EB.Features.Shared.Tests.Criteria
{
    public class GetBetTypeGroupBySportCriteriaTest
    {

        [Test]
        public void GetSettingKey_Always_CorrectKey()
        {
            // Arrange
            var criteria = new GetBetTypeGroupBySportCriteria();

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_BetType_Sel", act);
        }


        [Test]
        public void IsValid_Always_ReturnTrue()
        {
            // Arrange
            var criteria = new GetBetTypeGroupBySportCriteria();

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }
    }
}
