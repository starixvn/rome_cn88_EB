using System;
using System.Collections;
using EB.Features.Reports.Constants;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using EB.Features.Shared.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class SettledHouseWinLossOfSMACriteriaTest
    {
        [TestCase()]
        public void IsValid_ValidInput_ReturnTrue()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam
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
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam
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
        public void MoveBetsDay_Always_SetInConstructor()
        {
            // Arrange
            var expectedMoveBetsDayDate = DateTime.Now;
            var criteia = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                MoveBetsDay = expectedMoveBetsDayDate
            });

            // Act
            var act = criteia.MoveBetsDay;

            // Assert
            Assert.AreEqual(expectedMoveBetsDayDate, act);
        }

        [Test]
        public void FromDate_Always_SetInConstructor()
        {
            // Arrange
            var expectedFromDate = DateTime.Now;
            var criteia = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                FromDate = expectedFromDate
            });

            // Act
            var act = criteia.FromDate;

            // Assert
            Assert.AreEqual(expectedFromDate, act);
        }

        [Test]
        public void IsExcludedTest_Always_SetInConstructor()
        {
            // Arrange
            var expectedIsExcludedTest = true;
            var criteia = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                IsExcludedTest = expectedIsExcludedTest
            });

            // Act
            var act = criteia.IsExcludedTest;

            // Assert
            Assert.AreEqual(expectedIsExcludedTest, act);
        }


        [TestCase(true, ReportMode.History)]
        [TestCase(false, ReportMode.Current)]
        public void ReportMode_Always_SetInConstructor(bool isHistory, int expectedReportMode)
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam
            {
                IsHistoricReport = isHistory
            });

            // Act
            var act = criteria.ReportMode;

            // Assert
            Assert.AreEqual(expectedReportMode, act);
        }

        [Test]
        public void GetSettingKey_SuperAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Super });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBWinloss_Super", act);
        }

        [Test]
        public void GetSettingKey_MasterAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Master });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBWinloss_Master", act);
        }

        [Test]
        public void GetSettingKey_AgentAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Agent });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBWinloss_Agent", act);
        }

        [Test]
        public void GetSettingKey_MemberAlways_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Member });

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBWinloss_Member", act);
        }

        [Test]
        public void GetSpParams_Always_NotExistHeadlineId()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Member });

            // Act
            var act = criteria.GetSpParams();

            // Assert
            Assert.IsFalse(act.ContainsKey("HeadlineId"));
        }

        [Test]
        public void GetSpParams_Always_ExistSettingKey()
        {
            // Arrange
            var criteria = new SettledHouseWinLossOfSMACriteria(new HouseWinLossParam { Role = UserLevel.Member });

            // Act
            var act = criteria.GetSpParams();

            // Assert
            Assert.IsTrue(act.ContainsKey("SettingKey"));
        }
    }
}
