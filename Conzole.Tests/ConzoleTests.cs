using NUnit.Framework;
using Conzole;
using Moq;

namespace Conzole.Tests
{
    public class Tests
    {
        private Mock<IConsole> mockConsole;

        [SetUp]
        public void Setup()
        {
            mockConsole = new Mock<IConsole>(MockBehavior.Loose);
            ConzoleUtils.SetConsole(mockConsole.Object);
        }

        [Test]
        public void TestCount()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };

            // Act
            ConzoleUtils.Count(items);

            // Assert
            mockConsole.Verify(c => c.WriteLine(string.Format(ConzoleUtils.DEFAULT_COUNT_FORMAT, items.Length)));
        }

        [Test]
        public void TestCountCustomFormat()
        {
            // Arrange
            var format = "aaa {0} bbb";
            var items = new int[] { 3, 4, 2 };

            // Act
            ConzoleUtils.Count(items, format);

            // Assert
            mockConsole.Verify(c => c.WriteLine(string.Format(format, items.Length)));
        }
    }
}