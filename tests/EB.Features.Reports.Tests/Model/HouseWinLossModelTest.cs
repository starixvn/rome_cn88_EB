using EB.Features.Reports.Model;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Model
{
    public class HouseWinLossModelTest
    {
        [Test]
        public void OperatorAdd_Always_ReturnExpectedTotal()
        {
            // Arrange
            var houseWinLoss1 = new HouseWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                SuperComm = 5,
                HouseWinLoss = 6,
                HouseComm = 7,
                HouseIncome = 8,
                HeadlineStock = 9
            };

            var houseWinLoss2 = new HouseWinLossModel
            {
                Wagers = 1,
                Stake = 2,
                ValidStake = 3,
                MemberIncome = 4,
                SuperComm = 5,
                HouseWinLoss = 6,
                HouseComm = 7,
                HouseIncome = 8,
                HeadlineStock = 9
            };

            var expectedTotal = new HouseWinLossModel
            {
                Wagers = 2,
                Stake =4,
                ValidStake = 6,
                MemberIncome = 8,
                SuperComm = 10,
                HouseWinLoss = 12,
                HouseComm = 14,
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
            Assert.AreEqual(expectedTotal.SuperComm, act.SuperComm);
            Assert.AreEqual(expectedTotal.HouseWinLoss, act.HouseWinLoss);
            Assert.AreEqual(expectedTotal.HouseComm, act.HouseComm);
            Assert.AreEqual(expectedTotal.HouseIncome, act.HouseIncome);
            Assert.AreEqual(expectedTotal.HeadlineStock, act.HeadlineStock);
        }
    }
}
