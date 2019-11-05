using System;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria
{
    public class HouseWinLossBaseCriteriaTest
    {
        [Test]
        public void GetSettingKey_Always_ThrowException()
        {
            // Arrange
            var criteria = new HouseWinLossBaseCriteria(new HouseWinLossParam());

            // Act


            // Assert
            Assert.Throws<NotImplementedException>(() => criteria.GetSettingKey());
        }


        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            // Arrange
            var criteria = new HouseWinLossBaseCriteria(
                new HouseWinLossParam
                {
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
        [TestCase(1, -2, "1/1/2019", "1/1/2019")]
        [TestCase(-1, -2, "1/1/2019", "1/1/2019")]
        [TestCase(-1, -2, "12/1/2019", "1/1/2019")]
        public void IsValid_InvalidInput_ReturnFalse(int bettypeGroup, int isLive, DateTime fromDate, DateTime toDate)
        {
            // Arrange
            var criteria = new HouseWinLossBaseCriteria(
                new HouseWinLossParam
                {
                    BetTypeGroup = bettypeGroup,
                    IsLive = isLive,
                    FromDate = fromDate,
                    ToDate = toDate
                });

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }
    }
}
