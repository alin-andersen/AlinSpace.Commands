using FluentAssertions;
using Moq;
using System;
using System.Windows.Input;
using Xunit;

namespace FluentCommands.Tests
{
    /// <summary>
    /// Single command tests.
    /// </summary>
    public class SingleCommand
    {
        /// <summary>
        /// Tests the single basic workflow.
        /// </summary>
        [Fact]
        public void TestSingle()
        {
            // Setup

            var commandMock = new Mock<ICommand>();
            commandMock
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);

            ICommand command = null;

            // Act

            FluentCommandManager
                .New()
                .AddGroup(group =>
                {
                    command = group.RegisterCommand(commandMock.Object);
                });

            // Assert
            
            command.Should().NotBeNull();
            command.CanExecute(null).Should().BeTrue();
            command.Execute(null);
        }

        /// <summary>
        /// Tests the single workflow where "ignoreIndividualCanExecute" is set to true.
        /// </summary>
        [Fact]
        public void TestSingleIgnoreIndivualCanExecute()
        {
            // Setup

            var commandMock = new Mock<ICommand>();
            commandMock
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(false);

            ICommand command = null;

            // Act

            FluentCommandManager
                .New(ignoreIndividualCanExecute: true)
                .AddGroup(group =>
                {
                    command = group.RegisterCommand(commandMock.Object);
                });

            // Assert

            command.Should().NotBeNull();
            command.CanExecute(null).Should().BeTrue();
            command.Execute(null);
        }
    }
}
