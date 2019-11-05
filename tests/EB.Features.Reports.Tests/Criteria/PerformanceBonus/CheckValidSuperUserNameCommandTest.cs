using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Reports.Criteria;
using NUnit.Framework;

namespace EB.Features.Reports.Tests.Criteria.PerformanceBonus
{
    public class CheckValidSuperUserNameCommandTest
    {

        [Test]
        public void IsValid_ValidInput_ReturnTrue()
        {
            // Arrange
            var criteria = new CheckValidSuperUserNameCommand("razor");

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }

        [Test]
        public void IsValid_InvalidInput_ReturnFalse()
        {
            // Arrange
            var criteria = new CheckValidSuperUserNameCommand(string.Empty);

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsFalse(act);
        }

        [Test]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new CheckValidSuperUserNameCommand("razor");

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("CS_Customer_CheckValidUsername", act);
        }
    }
}
