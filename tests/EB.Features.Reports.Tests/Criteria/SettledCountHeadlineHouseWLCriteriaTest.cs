using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class SettledCountHeadlineHouseWLCriteriaTest
    {
        [Test]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledCountHeadlineHouseWLCriteria(new HouseWinLossParam());

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Winloss_Headline_CustCount", act);
        }
    }
}
