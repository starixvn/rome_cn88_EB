using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using EB.Features.Shared.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class UnsettledCountHeadlineHouseWLCriteriaTest
    {
        [Test]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledCountHeadlineHouseWLCriteria(new HouseWinLossParam { Role = UserLevel.Super });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Outstanding_Headline_CustCount", act);
        }
    }
}
