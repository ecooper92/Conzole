using NUnit.Framework;
using Conzole;
using Moq;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Conzole.Tests
{
    public class ConzoleTests
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

        [Test]
        public void TestList()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var listOptions = new ListOptions<int>();

            // Act
            ConzoleUtils.List(items);

            // Assert
            for (int i = 0; i < items.Length; i++)
            {
                mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter((i + 1).ToString(), items[i].ToString())));
            }
        }

        [Test]
        public void TestListCustomFormatter()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var listOptions = new ListOptions<int>();
            listOptions.LineFormatter = (index, item) => $"{item}::{index}";
            listOptions.ItemFormatter = item => $"aaa{item}bbb";

            // Act
            ConzoleUtils.List(items, listOptions);

            // Assert
            for (int i = 0; i < items.Length; i++)
            {
                mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter((i + 1).ToString(), listOptions.ItemFormatter(items[i]))));
            }
        }

        [Test]
        public async Task TestBackListMenuAsync()
        {
            // Arrange
            mockConsole.Setup(c => c.ReadLine()).Returns("0");
            var menu = new Menu("The menu", new MenuItem[]
            {
                new MenuItem("i1", () => Task.CompletedTask),
                new MenuItem("i2", () => Task.CompletedTask),
                new MenuItem("i3", () => Task.CompletedTask)
            });

            // Act
            await ConzoleUtils.ListMenuAsync(menu);

            // Assert - N/A
        }

        [Test]
        public async Task TestActionThenBackListMenuAsync()
        {
            // Arrange
            mockConsole.Setup(c => c.ReadLine()).Returns("3");
            var wasCalled = false;
            var menu = new Menu("The menu", new MenuItem[]
            {
                new MenuItem("i1", () => Task.CompletedTask),
                new MenuItem("i2", () => Task.CompletedTask),
                new MenuItem("i3", () =>
                {
                    wasCalled = true;
                    mockConsole.Setup(c => c.ReadLine()).Returns("0");
                    return Task.CompletedTask;
                })
            });

            // Act
            await ConzoleUtils.ListMenuAsync(menu);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        [Test]
        public async Task TestActionContinueThenBackListMenuAsync()
        {
            // Arrange
            mockConsole.Setup(c => c.ReadLine()).Returns("3");
            var wasCalled = false;
            var menu = new Menu("The menu", new MenuItem[]
            {
                new MenuItem("i1", () => Task.CompletedTask),
                new MenuItem("i2", () => Task.CompletedTask),
                new MenuItem("i3", () =>
                {
                    wasCalled = true;
                    mockConsole.Setup(c => c.ReadLine()).Returns("0");
                    return Task.FromResult(true);
                })
            });

            // Act
            await ConzoleUtils.ListMenuAsync(menu);

            // Assert
            Assert.IsTrue(wasCalled);
        }

        [Test]
        public async Task TestActionStopListMenuAsync()
        {
            // Arrange
            mockConsole.Setup(c => c.ReadLine()).Returns("3");
            var wasCalled = false;
            var menu = new Menu("The menu", new MenuItem[]
            {
                new MenuItem("i1", () => Task.CompletedTask),
                new MenuItem("i2", () => Task.CompletedTask),
                new MenuItem("i3", () =>
                {
                    wasCalled = true;
                    return Task.FromResult(false);
                })
            });

            // Act
            await ConzoleUtils.ListMenuAsync(menu);

            // Assert
            Assert.IsTrue(wasCalled);
        }
    }
}