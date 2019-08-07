using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Main.Criteria.InternalLogin;
using NUnit.Framework;

namespace EB.Features.Main.Tests.Criteria
{
    public class GetDBTimeCriteriaTest
    {

        [Test]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetDBTimeCriteria();

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_LoginAdminVerifyTicket", act);
        }

        [Test]
        public void IsValid_Always_ReturnTrue()
        {
            // Arrange
            var criteria = new GetDBTimeCriteria();

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.IsTrue(act);
        }
    }
}
