using EB.Features.Site.Reports.ViewModel;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Reports.ViewModel.HouseWinLoss
{
    public class HouseWinLossFilterViewModelTest
    {

        [Test]
        public void MonthNames_Always_Return12Month()
        {
            // Arrange


            // Act


            // Assert
            Assert.AreEqual(12, HouseWinLossFilterViewModel.MonthNames.Length);
        }
    }
}
