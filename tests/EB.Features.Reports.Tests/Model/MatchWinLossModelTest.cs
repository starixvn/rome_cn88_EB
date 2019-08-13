using EB.Features.Reports.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Model
{
    public class MatchWinLossModelTest
    {
        [Test]
        public void OperatorAdd_Always_ReturnExpectedTotal()
        {
            // Arrange
            var houseWinLoss1 = new MatchWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                HouseIncome = 8,
                HeadlineStock = 9
            };

            var houseWinLoss2 = new MatchWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                HouseIncome = 8,
                HeadlineStock = 9
            };

            var expectedTotal = new MatchWinLossModel
            {
                Wagers = 2,
                Stake =4,
                ValidStake = 6,
                MemberIncome = 8,
                HouseIncome = 16,
                HeadlineStock = 18
            };

            // Act
            var act = houseWinLoss1 + houseWinLoss2;

            // Assert
            Assert.AreEqual(expectedTotal.Wagers, act.Wagers);
            Assert.AreEqual(expectedTotal.Stake, act.Stake);
            Assert.AreEqual(expectedTotal.ValidStake, act.ValidStake);
            Assert.AreEqual(expectedTotal.MemberIncome, act.MemberIncome);
            Assert.AreEqual(expectedTotal.HouseIncome, act.HouseIncome);
            Assert.AreEqual(expectedTotal.HeadlineStock, act.HeadlineStock);
        }
    }
}
