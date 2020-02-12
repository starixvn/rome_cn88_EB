using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using EB.Features.Shared.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class UnsettledHouseWinLossOfSMACriteriaTest
    {
        [TestCase()]
        public void IsValid_ValidInput_ReturnTrue()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                SportTypeGroups = "0",
                BetTypeGroup = 0,
                IsLive = 0,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            });

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }
        [TestCase(-1, 1, "1/1/2019", "1/1/2019")]
        [TestCase(1, -1, "1/1/2019", "1/1/2019")]
        [TestCase(1, 1, "2/2/2019", "1/1/2019")]
        public void IsValid_InvalidInput_ReturnFalse(int betTypeGroup, int isLive, DateTime fromDate, DateTime toDate)
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                BetTypeGroup = betTypeGroup,
                IsLive = isLive,
                FromDate = fromDate,
                ToDate = toDate
            });

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }

        [Test]
        public void FromDate_Always_SetInConstructor()
        {
            // Arrange
            var expectedFromDate = DateTime.Now;
            var criteia = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                FromDate = expectedFromDate
            });

            // Act
            var act = criteia.FromDate;

            // Assert
            Assert.AreEqual(expectedFromDate, act);
        }

        [Test]
        public void ToDate_Always_SetInConstructor()
        {
            // Arrange
            var expectedToDate = DateTime.Now;
            var criteia = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                ToDate = expectedToDate
            });

            // Act
            var act = criteia.ToDate;

            // Assert
            Assert.AreEqual(expectedToDate, act);
        }

        [Test]
        public void IsExcludedTest_Always_SetInConstructor()
        {
            // Arrange
            var expectedIsExcludedTest = true;
            var criteia = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                IsExcludedTest = expectedIsExcludedTest
            });

            // Act
            var act = criteia.IsExcludedTest;

            // Assert
            Assert.AreEqual(expectedIsExcludedTest, act);
        }
                
        [Test]
        public void GetSettingKey_SuperAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Super });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Outstanding_Super", act);
        }

        [Test]
        public void GetSettingKey_MasterAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Master });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Outstanding_Master", act);
        }

        [Test]
        public void GetSettingKey_AgentAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Agent });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Outstanding_Agent", act);
        }

        [Test]
        public void GetSettingKey_MemberAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Member });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_EB_Rpt_Outstanding_Member", act);
        }

        [Test]
        public void GetSpParams_Always_ExistSettingKey()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Member });

            // Act
            var act = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(act.ContainsKey("SettingKey"));
        }       
    }
}
