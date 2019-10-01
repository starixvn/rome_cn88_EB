using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class UnUnsettledHouseWinLossCriteriaCriteriaTest
    {
        [TestCase()]
        public void IsValid_ValidInput_ReturnTrue()
        {
            // Arrange
            var criteria = new UnsettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam
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
            var criteria = new UnsettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam
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
            var criteia = new UnsettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam
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
            var criteia = new UnsettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam
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
            var criteia = new UnsettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam
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
            var criteria = new UnsettledHouseWinLossOfHeadlineCriteria(new HouseWinLossParam());

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Rpt_EBOutstanding_Headline", act);
        }
    }
}
