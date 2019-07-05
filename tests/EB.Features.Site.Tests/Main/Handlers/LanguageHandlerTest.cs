using EB.Features.Site.Main.Handlers;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Main.ViewModel;
using EB.Features.Site.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Main.Handlers
{
    public class LanguageHandlerTest
    {
        private LanguageHandler handler;
        private UserSession userSession;

        [SetUp]
        public void Setup()
        {
            userSession = Substitute.For<UserSession>();
            handler = new LanguageHandler(userSession);
        }

        [Test]
        public void ChangeLanguage_Always_CallLanguageOfUserSession()
        {
            // Arrange


            // Act
            handler.Handle(new ChangeLanguageCommand("E"));

            // Assert
            userSession.Received().Language = "E";
        }


        [Test]
        public void ChangeLanguage_Always_ReturnLanguageCode()
        {
            // Arrange
            var expectedLanguageCode = "E";

            // Act
            var act = handler.Handle(new ChangeLanguageCommand("E"));

            // Assert
            Assert.AreEqual(expectedLanguageCode, act);
        }


        [Test]
        public void AllLanguagesQuery_Awlays_ReturnCorrectModel()
        {
            // Arrange
            var query = new AllLanguagesQuery();

            // Act
            var act = handler.Handle(query);

            // Assert
            Assert.IsInstanceOf(typeof(LanguageViewModel), act);
        }
    }
}
