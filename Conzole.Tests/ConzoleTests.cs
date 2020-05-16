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
        public void TestEmptyPrompt()
        {
            // Arrange
            var input = "test-input";
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var result = ConzoleUtils.Prompt(string.Empty);

            // Assert
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TestNullPrompt()
        {
            // Arrange
            var input = "test-input";
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var result = ConzoleUtils.Prompt(null);

            // Assert
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void TestPrompt()
        {
            // Arrange
            var input = "test-input";
            var prompt = "prompt11";
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var result = ConzoleUtils.Prompt(prompt);

            // Assert
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
        }

        [Test]
        public void TestPromptInt()
        {
            // Arrange
            var input = 33;
            var prompt = "prompt11";
            mockConsole.Setup(c => c.ReadLine()).Returns(input.ToString());

            // Act
            var success = ConzoleUtils.PromptInt(prompt, out var result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
        }

        [Test]
        public void TestPromptIntFail()
        {
            // Arrange
            var input = "notanumber33";
            var prompt = "prompt11";
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var success = ConzoleUtils.PromptInt(prompt, out var result);

            // Assert
            Assert.IsFalse(success);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
        }

        [Test]
        public void TestPromptDouble()
        {
            // Arrange
            var input = 33.55;
            var prompt = "prompt11";
            mockConsole.Setup(c => c.ReadLine()).Returns(input.ToString());

            // Act
            var success = ConzoleUtils.PromptDouble(prompt, out var result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
        }

        [Test]
        public void TestPromptDoubleFail()
        {
            // Arrange
            var input = "notanumber33";
            var prompt = "prompt11";
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var success = ConzoleUtils.PromptDouble(prompt, out var result);

            // Assert
            Assert.IsFalse(success);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
        }

        [Test]
        public void TestCount()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var countOptions = new CountOptions();

            // Act
            ConzoleUtils.Count(items);

            // Assert
            mockConsole.Verify(c => c.WriteLine(countOptions.ResultFormatter(items.Length)));
        }

        [Test]
        public void TestCountCustomFormat()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var countOptions = new CountOptions();
            countOptions.ResultFormatter = count => $"aaa {count} bbb";

            // Act
            ConzoleUtils.Count(items, countOptions);

            // Assert
            mockConsole.Verify(c => c.WriteLine(countOptions.ResultFormatter(items.Length)));
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
                mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter((i + 1).ToString(), items[i])));
            }
        }

        [Test]
        public void TestListCustomFormatter()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var listOptions = new ListOptions<int>();
            listOptions.LineFormatter = (index, item) => $"{item}::{index}";

            // Act
            ConzoleUtils.List(items, listOptions);

            // Assert
            for (int i = 0; i < items.Length; i++)
            {
                mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter((i + 1).ToString(), items[i])));
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