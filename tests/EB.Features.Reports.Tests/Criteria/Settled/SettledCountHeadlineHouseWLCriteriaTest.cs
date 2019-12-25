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
            var criteria = new SettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam { IsCountUser = true });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBWinloss_Headline_CustCount", act);
        }

        [Test]
        public void GetSpParams_Always_ExistSettingKey()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam { IsCountUser = true });

            // Act
            var act = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(act.ContainsKey("SettingKey"));
        }
    }
}
