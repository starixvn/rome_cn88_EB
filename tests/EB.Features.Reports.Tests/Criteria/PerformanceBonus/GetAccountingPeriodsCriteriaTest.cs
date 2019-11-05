using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Reports.Criteria.PerformanceBonus;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria.PerformanceBonus
{
    public class GetAccountingPeriodsCriteriaTest
    {

        [Test]
        public void IsValid_Always_ReturnTrue()
        {
            // Arrange
            var criteria = new GetAccountingPeriodsCriteria();

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetAccountingPeriodsCriteria();

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_AccountingPeriod_Sel", act);
        }
    }
}
