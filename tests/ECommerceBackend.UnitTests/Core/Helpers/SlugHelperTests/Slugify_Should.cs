using System;
using ECommerceBackend.Core.Helpers;
using Moq;
using Xunit;

namespace ECommerceBackend.UnitTests.Core.Helpers.SlugHelperTests
{
    public class Slugify_Should
    {
        [Fact]
        public static void Throw_NullReferenceException_Given_An_Empty_String()
        {
            // Arrange
            // Act
            // Assert
            var exception = Assert.Throws<NullReferenceException>(() => SlugHelper.Slugify(string.Empty));
            Assert.Equal("The string to be slugified should not be null or empty", exception.Message);
        }


        [Fact]
        public static void Throw_NullReferenceException_Given_A_Null_String()
        {
            // Arrange
            // Act
            // Assert
            var exception = Assert.Throws<NullReferenceException>(() => SlugHelper.Slugify(null));
            Assert.Equal("The string to be slugified should not be null or empty", exception.Message);
        }

        [Theory()]
        [InlineData("διακριτικός")]
        [InlineData("😊😍✔")]
        public static void Throw_Exception_If_String_Cannot_Be_Slugified(string str)
        {
            // Arrange
            // Act
            // Assert
            var exception = Assert.Throws<Exception>(() => SlugHelper.Slugify(str));
            Assert.Equal($"{str} cannot be slugified", exception.Message);
        }

        [Fact]
        public static void Return_A_Non_Empty_String()
        {
            // Arrange
            // Act
            var result = SlugHelper.Slugify("Some String");

            // Assert
            Assert.IsType<string>(result);
            Assert.False(string.IsNullOrEmpty(result), "The slugify method should not return a null or empty string");
        }

        [Fact]
        public static void Return_A_String_With_No_Spaces()
        {
            // Arrange
            // Act
            var result = SlugHelper.Slugify("Some random string with spaces");

            // Assert
            Assert.False(result.Contains(" "), "The slugify method should not return a string with spaces");
        }

        [Theory()]
        [InlineData("My Super DUPER coMpany", "my-super-duper-company")]
        [InlineData("ALL UPPERCASE", "all-uppercase")]
        public static void Return_A_Hyphenated_Lowercase_String(string str, string expected)
        {
            // Arrange
            // Act
            var result = SlugHelper.Slugify(str);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory()]
        [InlineData("Diacritics διακριτικός", "diacritics")]
        [InlineData("Some διακριτικός data 👍", "some-data")]
        [InlineData("διακριτικός Some  data 😍", "some-data")]
        [InlineData("- Some Data -", "some-data")]
        public static void Remove_Diacritics_And_Starting_Or_Ending_Dashes(string str, string expected)
        {
            // Arrange
            // Act
            var result = SlugHelper.Slugify(str);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
