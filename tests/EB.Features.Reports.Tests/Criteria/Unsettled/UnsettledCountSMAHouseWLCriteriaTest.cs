using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using EB.Features.Shared.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class UnsettledCountSMAHouseWLCriteriaTest
    {
        [Test]
        public void GetSettingKey_SuperRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Super, IsCountUser = true });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBOutstanding_Super_CustCount", act);
        }

        [Test]
        public void GetSettingKey_MasterRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Master, IsCountUser = true });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBOutstanding_Master_CustCount", act);
        }

        [Test]
        public void GetSettingKey_AgentRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Agent, IsCountUser = true });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBOutstanding_Agent_CustCount", act);
        }

        [Test]
        public void GetSettingKey_MemberRole_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Member, IsCountUser = true });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBOutstanding_Member_CustCount", act);
        }

        [Test]
        public void IsValid_CorrectInput_ReturnTrue()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria
            (
                new HouseWinLossParam
                {
                    SportTypeGroups = "0",
                    BetTypeGroup = 0,
                    IsLive = 0,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    HeadlineId = 0,
                    CustId = 1,
                    IsCountUser = true
                }
            );

            // Action
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        public void IsValid_IncorrectInput_ReturnFalse(int headlineId, int custId)
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria
            (
                new HouseWinLossParam
                {
                    SportTypeGroups = "0",
                    BetTypeGroup = 0,
                    IsLive = 0,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    HeadlineId = headlineId,
                    CustId = custId,
                    IsCountUser = true
                }
            );

            // Action
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }
    }
}
