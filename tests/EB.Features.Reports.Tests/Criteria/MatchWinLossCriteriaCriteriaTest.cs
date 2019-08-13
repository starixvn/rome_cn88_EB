using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class MatchWinLossCriteriaCriteriaTest
    {
        [TestCase()]
        public void IsValid_ValidInput_ReturnTrue()
        {
            // Arrange
            var criteria = new MatchWinLossCriteria(new MatchWinLossParam
            {
                SportTypeGroup = 0,
                BetTypeGroup = 0,
                IsLive = 0
            });

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }
        [TestCase(-1, 1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(1, 1, -1)]
        public void IsValid_InvalidInput_ReturnFalse(int sportTypeGroup, int betTypeGroup, int isLive)
        {
            // Arrange
            var criteria = new MatchWinLossCriteria(new MatchWinLossParam
            {
                SportTypeGroup = sportTypeGroup,
                BetTypeGroup = betTypeGroup,
                IsLive = isLive
            });

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }


        [Test]
        public void WinLostDate_Always_SetInConstructor()
        {
            // Arrange
            var expectedWinLostDate = DateTime.Now;
            var criteia = new MatchWinLossCriteria(new MatchWinLossParam
            {
                WinLossDate = expectedWinLostDate
            });

            // Act
            var act = criteia.WinLostDate;

            // Assert
            Assert.AreEqual(expectedWinLostDate, act);
        }

        [Test]
        public void MoveBetsDay_Always_SetInConstructor()
        {
            // Arrange
            var expectedMoveBetsDayDate = DateTime.Now;
            var criteia = new MatchWinLossCriteria(new MatchWinLossParam
            {
                MoveBetsDay = expectedMoveBetsDayDate
            });

            // Act
            var act = criteia.MoveBetsDay;

            // Assert
            Assert.AreEqual(expectedMoveBetsDayDate, act);
        }


        [Test]
        public void IsExcludedTest_Always_SetInConstructor()
        {
            // Arrange
            var expectedIsExcludedTest = true;
            var criteia = new MatchWinLossCriteria(new MatchWinLossParam
            {
                IsExcludedTest = expectedIsExcludedTest
            });

            // Act
            var act = criteia.IsExcludedTest;

            // Assert
            Assert.AreEqual(expectedIsExcludedTest, act);
        }
                
        [Test]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new MatchWinLossCriteria(new MatchWinLossParam());

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_MatchWinLoss_Headline", act);
        }
    }
}
