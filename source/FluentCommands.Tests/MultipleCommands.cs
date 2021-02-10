using FluentAssertions;
using Moq;
using System.Windows.Input;
using Xunit;

namespace FluentCommands.Tests
{
    public class MultipleCommands
    {
        /// <summary>
        /// Tests the basic workflow for two commands.
        /// </summary>
        [Fact]
        public void TestTwoCommands()
        {
            // Setup

            var commandMockA = new Mock<ICommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);

            var commandMockB = new Mock<ICommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);

            ICommand commandA = null;
            ICommand commandB = null;

            // Act

            CommandManager
                .Create()
                .AddGroup(group =>
                {
                    commandA = group.RegisterCommand(commandMockA.Object);
                    commandB = group.RegisterCommand(commandMockB.Object);
                });

            // Assert

            commandA.Should().NotBeNull();
            commandA.CanExecute(null).Should().BeTrue();
            commandA.Execute(null);

            commandB.Should().NotBeNull();
            commandB.CanExecute(null).Should().BeTrue();
            commandB.Execute(null);
        }
    }
}
