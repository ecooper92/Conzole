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
        public void RegularPromptTest()
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
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void NoPostNewlinePromptTest()
        {
            // Arrange
            var input = "test-input";
            var prompt = "prompt11";
            var options = new PromptOptions(prompt);
            options.PostNewLine = false;
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var result = ConzoleUtils.Prompt(options);

            // Assert
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
            mockConsole.Verify(c => c.WriteLine(), Times.Never);
        }

        [Test]
        public void EmptyPromptTest()
        {
            // Arrange
            var input = "test-input";
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var result = ConzoleUtils.Prompt(string.Empty);

            // Assert
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void NoPromptTest()
        {
            // Arrange
            var input = "test-input";
            mockConsole.Setup(c => c.ReadLine()).Returns(input);

            // Act
            var result = ConzoleUtils.Prompt();

            // Assert
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void EmptyPromptIntTest()
        {
            // Arrange
            var input = 33;
            mockConsole.Setup(c => c.ReadLine()).Returns(input.ToString());

            // Act
            var success = ConzoleUtils.PromptInt(out var result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void RegularPromptIntTest()
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
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void OptionsPromptIntTest()
        {
            // Arrange
            var input = 33;
            var prompt = "prompt11";
            var options = new PromptOptions(prompt);
            options.PostNewLine = false;
            mockConsole.Setup(c => c.ReadLine()).Returns(input.ToString());

            // Act
            var success = ConzoleUtils.PromptInt(options, out var result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
            mockConsole.Verify(c => c.WriteLine(), Times.Never);
        }

        [Test]
        public void PromptIntFailTest()
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
            mockConsole.Verify(c => c.Write(prompt), Times.Never);
        }

        [Test]
        public void EmptyPromptDoubleTest()
        {
            // Arrange
            var input = 33.55;
            mockConsole.Setup(c => c.ReadLine()).Returns(input.ToString());

            // Act
            var success = ConzoleUtils.PromptDouble(out var result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(It.IsAny<string>()), Times.Never);
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void RegularPromptDoubleTest()
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
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void OptionsPromptDoubleTest()
        {
            // Arrange
            var input = 33.55;
            var prompt = "prompt11";
            var options = new PromptOptions(prompt);
            options.PostNewLine = false;
            mockConsole.Setup(c => c.ReadLine()).Returns(input.ToString());

            // Act
            var success = ConzoleUtils.PromptDouble(options, out var result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(input, result);
            mockConsole.Verify(c => c.WriteLine(prompt), Times.Once);
            mockConsole.Verify(c => c.WriteLine(), Times.Never);
        }

        [Test]
        public void FailPromptDoubleTest()
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
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void CountTest()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var countOptions = new CountOptions();

            // Act
            ConzoleUtils.Count(items);

            // Assert
            mockConsole.Verify(c => c.WriteLine(countOptions.ResultFormatter(items.Length)));
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void CountCustomFormatTest()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var countOptions = new CountOptions();
            countOptions.PostNewLine = false;
            countOptions.ResultFormatter = count => $"aaa {count} bbb";

            // Act
            ConzoleUtils.Count(items, countOptions);

            // Assert
            mockConsole.Verify(c => c.WriteLine(countOptions.ResultFormatter(items.Length)));
            mockConsole.Verify(c => c.WriteLine(), Times.Never);
        }

        [Test]
        public void ListTest()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var listOptions = new ListOptions<int>();

            // Act
            ConzoleUtils.List(items);

            // Assert
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("1", items[0])), Times.Once);
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("2", items[1])), Times.Once);
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("3", items[2])), Times.Once);
            mockConsole.Verify(c => c.WriteLine(), Times.Once);
        }

        [Test]
        public void PostNewLineDisabledListTest()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var listOptions = new ListOptions<int>();
            listOptions.PostNewLine = false;

            // Act
            ConzoleUtils.List(items, listOptions);

            // Assert
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("1", items[0])), Times.Once);
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("2", items[1])), Times.Once);
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("3", items[2])), Times.Once);
            mockConsole.Verify(c => c.WriteLine(), Times.Never);
        }

        [Test]
        public void ListLineFormatterTest()
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
        public void ListIndexGeneratorTest()
        {
            // Arrange
            var items = new int[] { 3, 4, 2 };
            var listOptions = new ListOptions<int>();
            listOptions.IndexGenerator = index => ((char)('a' + index)).ToString();

            // Act
            ConzoleUtils.List(items, listOptions);

            // Assert
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("a", 3)));
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("b", 4)));
            mockConsole.Verify(c => c.WriteLine(listOptions.LineFormatter("c", 2)));
        }

        [Test]
        public async Task NestedMenuAsyncTest()
        {
            // Arrange
            var callCount = 0;
            mockConsole.SetupSequence(c => c.ReadLine())
                .Returns("Invalid Input")
                .Returns("I")
                .Returns("0")
                .Returns("a")
                .Returns("b")
                .Returns("c")
                .Returns("II")
                .Returns("MAGIC");

            var nestedMenu = new Menu("nested menu");
            nestedMenu.InputPrompt = "NESTED INPUT PROMPT";
            nestedMenu.InvalidInputPrompt = "NESTED INPUT ERROR";
            nestedMenu.RemoveExitMenuItem();
            nestedMenu.AddMenuItem("a", new MenuItem("a1", () => { callCount++; return Task.CompletedTask; }));
            nestedMenu.AddMenuItem("b", new MenuItem("b1", () => { callCount++; return Task.FromResult(true); }));
            nestedMenu.AddMenuItem("c", new MenuItem("c1", () => { callCount++; return Task.FromResult(false); }));

            var rootMenu = new Menu("root menu");
            rootMenu.SetExitMenuItem("EXIT OPTION", "MAGIC2");
            rootMenu.RemoveExitMenuItem();
            rootMenu.SetExitMenuItem("EXIT OPTION", "MAGIC");
            rootMenu.InputPrompt = "ROOT INPUT PROMPT";
            rootMenu.InvalidInputPrompt = "ROOT INPUT ERROR";
            rootMenu.AddMenuItem("I", nestedMenu);
            rootMenu.AddMenuItem("II", new MenuItem("i2", () => { callCount++; return Task.CompletedTask; }));

            // Act
            await ConzoleUtils.MenuAsync(rootMenu);

            // Assert
            Assert.AreEqual(4, callCount);
            mockConsole.Verify(c => c.WriteLine(nestedMenu.Title), Times.Exactly(4));
            mockConsole.Verify(c => c.WriteLine(nestedMenu.InputPrompt), Times.Exactly(4));
            mockConsole.Verify(c => c.WriteLine(nestedMenu.InvalidInputPrompt), Times.Once);
            mockConsole.Verify(c => c.WriteLine(rootMenu.Title), Times.Exactly(4));
            mockConsole.Verify(c => c.WriteLine(rootMenu.InputPrompt), Times.Exactly(4));
            mockConsole.Verify(c => c.WriteLine(rootMenu.InvalidInputPrompt), Times.Once);
        }

        [Test]
        public async Task RepeatUntilSuccessTest()
        {
            // Arrange
            var callCount = 0;
            var options = new RepeatUntilSuccessOptions();
            options.PositiveResponse = "GO";
            options.NegativeResponse = "NO GO";

            mockConsole.Setup(c => c.ReadLine()).Returns(options.PositiveResponse);

            // Act
            await ConzoleUtils.RepeatUntilSuccess(() =>
            {
                callCount++;
                if (callCount == 3)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }, options);

            // Assert
            Assert.AreEqual(3, callCount);
        }

        [Test]
        public async Task RepeatUntilSuccessAsyncTest()
        {
            // Arrange
            var callCount = 0;
            var options = new RepeatUntilSuccessOptions();
            options.PositiveResponse = "GO";
            options.NegativeResponse = "NO GO";

            mockConsole.Setup(c => c.ReadLine()).Returns(options.PositiveResponse);

            // Act
            await ConzoleUtils.RepeatUntilSuccessAsync(() =>
            {
                callCount++;
                if (callCount == 3)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }, options);

            // Assert
            Assert.AreEqual(3, callCount);
        }
    }
}