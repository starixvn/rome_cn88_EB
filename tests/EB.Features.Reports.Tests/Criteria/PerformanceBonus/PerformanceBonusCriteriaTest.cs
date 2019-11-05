using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Reports.Criteria;
using EB.Features.Reports.Params;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria.PerformanceBonus
{
    public class PerformanceBonusCriteriaTest
    {
        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            // Arrange
            var criteria = new PerformanceBonusCriteria(new PerformanceBonusParam
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CurrencyId = 123
            });

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new PerformanceBonusCriteria(new PerformanceBonusParam());

            // Act
            var act = criteria.GetSettingKey();


            // Assert
            Assert.AreEqual("CS_EB_Rpt_PerformanceBonus", act);
        }

        [TestCase("2/2/2019","1/1/2019",21)]
        [TestCase("2/2/2019", "3/3/2019", 0)]
        public void IsValid_InvalidInput_ReturFalse(DateTime fromDate, DateTime toDate, int currencyId)
        {
            // Arrange
            var criteria = new PerformanceBonusCriteria(new PerformanceBonusParam { FromDate = fromDate, ToDate = toDate, CurrencyId = currencyId });

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }
    }
}
