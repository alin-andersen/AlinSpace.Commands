using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xunit;

namespace FluentCommands.Tests
{
    /// <summary>
    /// Tests for <see cref="FluentCommandManager"/>.
    /// </summary>
    public class FluentCommandManagerTests
    {
        /// <summary>
        /// Tests command manager with single command execution.
        /// </summary>
        [Fact]
        public void SingleCommandExecution()
        {
            // Setup

            var commandMock = new Mock<IFluentCommand>();
            commandMock
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMock
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.CompletedTask);

            ICommand command = null;

            FluentCommandManager
                .New()
                .AddGroup(eg =>
                {
                    command = eg.Register(commandMock.Object);
                });

            // Act

            command.Execute(null);

            // Assert

            command.Should().NotBeNull();
            command.CanExecute(null).Should().BeTrue();

            commandMock.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Once);
            commandMock.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);
        }

        /// <summary>
        /// Tests command manager with multiple command execution.
        /// </summary>
        [Fact]
        public void MultipleCommandExecution()
        {
            // Setup

            var commandMockA = new Mock<IFluentCommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockA
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.CompletedTask);

            var commandMockB = new Mock<IFluentCommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockB
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.CompletedTask);

            ICommand commandA = null;
            ICommand commandB = null;

            FluentCommandManager
                .New()
                .AddGroup(eg =>
                {
                    commandA = eg.Register(commandMockA.Object);
                    commandB = eg.Register(commandMockB.Object);
                });

            // Act

            commandA.Execute(null);
            commandB.Execute(null);

            // Assert

            commandA.Should().NotBeNull();
            commandA.CanExecute(null).Should().BeTrue();
            commandMockA.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Once);
            commandMockA.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);

            commandB.Should().NotBeNull();
            commandB.CanExecute(null).Should().BeTrue();
            commandMockB.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Once);
            commandMockB.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);
        }
    }
}
