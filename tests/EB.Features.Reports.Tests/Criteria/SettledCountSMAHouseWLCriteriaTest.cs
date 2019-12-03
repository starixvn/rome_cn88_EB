using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using EB.Features.Shared.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class SettledCountSMAHouseWLCriteriaTest
    {
        [Test]
        public void GetSettingKey_SuperRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledCountSMAHouseWLCriteria(new HouseWinLossParam { Role = UserLevel.Super });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Winloss_Super_CustCount", act);
        }

        [Test]
        public void GetSettingKey_MasterRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledCountSMAHouseWLCriteria(new HouseWinLossParam { Role = UserLevel.Master });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Winloss_Master_CustCount", act);
        }

        [Test]
        public void GetSettingKey_AgentRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledCountSMAHouseWLCriteria(new HouseWinLossParam { Role = UserLevel.Agent });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Winloss_Agent_CustCount", act);
        }

        [Test]
        public void GetSettingKey_MemberRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledCountSMAHouseWLCriteria(new HouseWinLossParam { Role = UserLevel.Member });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Winloss_Member_CustCount", act);
        }

        [Test]
        public void IsValid_CorrectInput_ReturnTrue()
        {
            // Arrange
            var criteria = new SettledCountSMAHouseWLCriteria
            (
                new HouseWinLossParam
                {
                    SportTypeGroups = "0",
                    BetTypeGroup = 0,
                    IsLive = 0,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    HeadlineId = 0,
                    CustId = 1
                }
            );

            // Action
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }

        [TestCase(-1,0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        public void IsValid_IncorrectInput_ReturnFalse(int headlineId, int custId)
        {
            // Arrange
            var criteria = new SettledCountSMAHouseWLCriteria
            (
                new HouseWinLossParam
                {
                    SportTypeGroups = "0",
                    BetTypeGroup = 0,
                    IsLive = 0,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    HeadlineId = headlineId,
                    CustId = custId
                }
            );

            // Action
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }
    }
}
