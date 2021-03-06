using NUnit.Framework;
using Conzole;
using Moq;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.IO;

namespace Conzole.Tests
{
    public class ConzolePrintTests
    {
        private Mock<IConsole> _mockConsole;
        private TextWriter _fileWriter;

        [OneTimeSetUp]
        public void Setup()
        {
            _fileWriter = File.CreateText("text-output.txt");
            _mockConsole = new Mock<IConsole>(MockBehavior.Loose);
            _mockConsole.Setup(c => c.WriteLine()).Callback(() => _fileWriter.WriteLine());
            _mockConsole.Setup(c => c.Write(It.IsAny<string>())).Callback<string>(line => _fileWriter.WriteLine(line));
            _mockConsole.Setup(c => c.WriteLine(It.IsAny<string>())).Callback<string>(line => _fileWriter.WriteLine(line));

            ConzoleUtils.SetConsole(_mockConsole.Object);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _fileWriter.Flush();
            _fileWriter.Dispose();
        }

        [Test]
        public void PrintListTest()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };

            // Act
            ConzoleUtils.List(items);

            // Assert - N/A
        }

        [Test]
        public void PrintCountTest()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };

            // Act
            ConzoleUtils.Count(items);
            ConzoleUtils.Count(items);
            ConzoleUtils.Count(items);

            // Assert - N/A
        }

        [Test]
        public async Task PrintMenuTest()
        {
            // Arrange
            _mockConsole.SetupSequence(c => c.ReadLine())
                .Returns("3")
                .Returns("2")
                .Returns("0");

            var menu = new Menu("The menu");
            menu.MenuItemFormatter = (key, menuItem) => $"{key}::  !{menuItem.Title}!";
            menu.AddMenuItem("1", new MenuItem("i1", () => Task.CompletedTask));
            menu.AddMenuItem("2", new MenuItem("i2", () => Task.CompletedTask));
            menu.AddMenuItem("3", new MenuItem("i3", () => Task.CompletedTask));

            // Act
            await ConzoleUtils.MenuAsync(menu);

            // Assert - N/A
        }
    }
}