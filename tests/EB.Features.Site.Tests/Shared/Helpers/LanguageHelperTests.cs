namespace EB.Features.Site.Tests.Shared.Helpers
{
    using EB.Features.Site.Shared.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class LanguageHelperTests
    {
        [TestCase("en-US", "E")]
        [TestCase("zh-Hans", "CN")]
        [TestCase("zh-TW", "CT")]
        public void GetLanguageKey_ValidLanguage_ReturnsCorrectLanguageKey(string inputLanguage, string expectedLanguageCode)
        {
            // Arrange & Act
            var actualLanguageCode = LanguageHelper.GetLanguageKey(inputLanguage);

            // Assert
            Assert.AreEqual(expectedLanguageCode, actualLanguageCode);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase("null")]
        public void GetLanguageKey_InvalidLanguage_ReturnsDefaultLanguageKey(string inputLanguage)
        {
            // Arrange
            var expectedLanguageCode = "CN";

            // Act
            var actualLanguageCode = LanguageHelper.GetLanguageKey(inputLanguage);

            // Assert
            Assert.AreEqual(expectedLanguageCode, actualLanguageCode);
        }
    }
}
