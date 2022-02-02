using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xunit;

namespace AlinSpace.Command.Tests
{
    /// <summary>
    /// Tests for <see cref="AsyncCommandManager"/>.
    /// </summary>
    public class AsyncCommandManagerTests
    {
        /// <summary>
        /// Tests that the command manager will block commands as expected.
        /// </summary>
        /// <remarks>
        /// This test will create the following configuration:
        /// 
        ///     - ExecutionGroup A with <see cref="GroupLockBehavior.LockAllGroups"/>
        ///         - Command A
        ///         
        /// Command A will be executed.
        /// Command A is expected be executed.
        /// </remarks>
        [Fact]
        public async void BlockCommands()
        {
            // Setup

            var commandMock = new Mock<ICommand>();
            commandMock
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMock
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.CompletedTask);

            ICommand command = null;

            Manager
                .New()
                .AddGroup(eg => command = eg.Register(commandMock.Object));

            // Act

            await command.ExecuteAsync(null);

            // Assert

            command.Should().NotBeNull();

            commandMock.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMock.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);

            command.CanExecute(null).Should().BeTrue();
        }

        /// <summary>
        /// Tests that the command manager will block commands as expected.
        /// </summary>
        /// <remarks>
        /// This test will create the following configuration:
        /// 
        ///     - ExecutionGroup A with <see cref="GroupLockBehavior.LockAllGroups"/>
        ///         - Command A
        ///         - Command B
        ///         
        /// Command A will be executed.
        /// Command B is expected to return <c>false</c> when calling CanExecute while Command A is executing.
        /// </remarks>
        [Fact]
        public void BlockCommands2()
        {
            // Setup

            var tcs = new TaskCompletionSource<object>();

            var commandMockA = new Mock<ICommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockA
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(tcs.Task);

            var commandMockB = new Mock<ICommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockB
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            ICommand commandA = null;
            ICommand commandB = null;

            Manager
                .New()
                .AddGroup(eg =>
                {
                    commandA = eg.Register(commandMockA.Object);
                    commandB = eg.Register(commandMockB.Object);
                });

            // Act

            var _ = commandA.ExecuteAsync(null);

            // Assert

            commandA.Should().NotBeNull();
            commandMockA.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockA.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);

            commandB.Should().NotBeNull();
            commandMockB.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockB.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandB.CanExecute(null).Should().BeFalse("Command A should block Command B.");

            tcs.SetResult(null);
        }

        /// <summary>
        /// Tests that the command manager will block commands as expected.
        /// </summary>
        /// <remarks>
        /// This test will create the following configuration:
        /// 
        ///     - ExecutionGroup A with <see cref="GroupLockBehavior.LockAllGroups"/>
        ///         - Command A
        ///     - ExecutionGroup B
        ///         - Command B
        ///         
        /// Command A will be executed.
        /// Command B is expected to return <c>false</c> when calling CanExecute while Command A is executing.
        /// </remarks>
        [Fact]
        public void BlockCommands3()
        {
            // Setup

            var tcs = new TaskCompletionSource<object>();

            var commandMockA = new Mock<ICommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockA
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(tcs.Task);

            var commandMockB = new Mock<ICommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockB
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            ICommand commandA = null;
            ICommand commandB = null;

            Manager
                .New()
                .AddGroup(eg => commandA = eg.Register(commandMockA.Object))
                .AddGroup(eg => commandB = eg.Register(commandMockB.Object));

            // Act

            var _ = commandA.ExecuteAsync(null);

            // Assert

            commandA.Should().NotBeNull();
            commandMockA.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockA.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);

            commandB.Should().NotBeNull();
            commandMockB.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockB.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandB.CanExecute(null).Should().BeFalse("Command A should block Command B.");

            tcs.SetResult(null);
        }

        /// <summary>
        /// Tests that the command manager will block commands as expected.
        /// </summary>
        /// <remarks>
        /// This test will create the following configuration:
        /// 
        ///     - ExecutionGroup A with <see cref="GroupLockBehavior.LockAllGroups"/>
        ///         - Command A
        ///     - ExecutionGroup B
        ///         - Command B
        ///     - ExecutionGroup C
        ///         - Command C
        ///         
        /// Command A will be executed.
        /// Command B and C are expected to return <c>false</c> when calling CanExecute while Command A is executing.
        /// </remarks>
        [Fact]
        public void BlockCommands4()
        {
            // Setup

            var tcs = new TaskCompletionSource<object>();

            var commandMockA = new Mock<ICommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockA
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(tcs.Task);

            var commandMockB = new Mock<ICommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockB
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            var commandMockC = new Mock<ICommand>();
            commandMockC
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockC
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            ICommand commandA = null;
            ICommand commandB = null;
            ICommand commandC = null;

            Manager
                .New()
                .AddGroup(eg => commandA = eg.Register(commandMockA.Object))
                .AddGroup(eg => commandB = eg.Register(commandMockB.Object))
                .AddGroup(eg => commandC = eg.Register(commandMockC.Object));

            // Act

            var _ = commandA.ExecuteAsync(null);

            // Assert

            commandA.Should().NotBeNull();
            commandMockA.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockA.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);

            commandB.Should().NotBeNull();
            commandMockB.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockB.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandB.CanExecute(null).Should().BeFalse("Command A should block Command B.");

            commandC.Should().NotBeNull();
            commandMockC.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockC.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandC.CanExecute(null).Should().BeFalse("Command A should block Command C.");

            tcs.SetResult(null);
        }

        /// <summary>
        /// Tests that the command manager will block commands as expected.
        /// </summary>
        /// <remarks>
        /// This test will create the following configuration:
        /// 
        ///     - ExecutionGroup A with <see cref="GroupLockBehavior.LockAllOtherGroups"/>
        ///         - Command A
        ///     - ExecutionGroup B
        ///         - Command B
        ///     - ExecutionGroup C
        ///         - Command C
        ///         
        /// Command A will be executed.
        /// Command A is expected to return <c>true</c> when calling CanExecute while Command A is executing.
        /// Command B and C are expected to return <c>false</c> when calling CanExecute while Command A is executing.
        /// </remarks>
        [Fact]
        public void BlockCommands5()
        {
            // Setup

            var tcs = new TaskCompletionSource<object>();

            var commandMockA = new Mock<ICommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockA
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(tcs.Task);

            var commandMockB = new Mock<ICommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockB
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            var commandMockC = new Mock<ICommand>();
            commandMockC
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockC
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            ICommand commandA = null;
            ICommand commandB = null;
            ICommand commandC = null;

            Manager
                .New()
                .AddGroup(
                    eg => commandA = eg.Register(commandMockA.Object),
                    GroupLockBehavior.LockAllOtherGroups)
                .AddGroup(eg => commandB = eg.Register(commandMockB.Object))
                .AddGroup(eg => commandC = eg.Register(commandMockC.Object));

            // Act

            var _ = commandA.ExecuteAsync(null);

            // Assert

            commandA.Should().NotBeNull();
            commandMockA.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockA.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);
            commandA.CanExecute(null).Should().BeTrue("Command A should NOT block itself.");

            commandB.Should().NotBeNull();
            commandMockB.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockB.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandB.CanExecute(null).Should().BeFalse("Command A should block Command B.");

            commandC.Should().NotBeNull();
            commandMockC.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockC.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandC.CanExecute(null).Should().BeFalse("Command A should block Command C.");

            tcs.SetResult(null);
        }

        /// <summary>
        /// Tests that the command manager will block commands as expected.
        /// </summary>
        /// <remarks>
        /// This test will create the following configuration:
        /// 
        ///     - ExecutionGroup A with <see cref="GroupLockBehavior.LockThisGroup"/>
        ///         - Command A
        ///     - ExecutionGroup B
        ///         - Command B
        ///     - ExecutionGroup C
        ///         - Command C
        ///         
        /// Command A will be executed.
        /// Command A is expected to return <c>false</c> when calling CanExecute while Command A is executing.
        /// Command B and C are expected to return <c>true</c> when calling CanExecute while Command A is executing.
        /// </remarks>
        [Fact]
        public void BlockCommands6()
        {
            // Setup

            var tcs = new TaskCompletionSource<object>();

            var commandMockA = new Mock<ICommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockA
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(tcs.Task);

            var commandMockB = new Mock<ICommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockB
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            var commandMockC = new Mock<ICommand>();
            commandMockC
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockC
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            ICommand commandA = null;
            ICommand commandB = null;
            ICommand commandC = null;

            Manager
                .New()
                .AddGroup(
                    eg => commandA = eg.Register(commandMockA.Object),
                    GroupLockBehavior.LockAllOtherGroups)
                .AddGroup(eg => commandB = eg.Register(commandMockB.Object))
                .AddGroup(eg => commandC = eg.Register(commandMockC.Object));

            // Act

            var _ = commandA.ExecuteAsync(null);

            // Assert

            commandA.Should().NotBeNull();
            commandMockA.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockA.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);
            commandA.CanExecute(null).Should().BeTrue("Command A should not block itself.");

            commandB.Should().NotBeNull();
            commandMockB.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockB.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandB.CanExecute(null).Should().BeFalse("Command A should block Command B.");

            commandC.Should().NotBeNull();
            commandMockC.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockC.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandC.CanExecute(null).Should().BeFalse("Command A should block Command C.");

            tcs.SetResult(null);
        }

        /// <summary>
        /// Tests that the command manager will block commands as expected.
        /// </summary>
        /// <remarks>
        /// This test will create the following configuration:
        /// 
        ///     - ExecutionGroup A with <see cref="GroupLockBehavior.LockNothing"/>
        ///         - Command A
        ///     - ExecutionGroup B
        ///         - Command B
        ///     - ExecutionGroup C
        ///         - Command C
        ///         
        /// Command A will be executed.
        /// Command A, B and C are expected to return <c>true</c> when calling CanExecute while Command A is executing.
        /// </remarks>
        [Fact]
        public void BlockCommands7()
        {
            // Setup

            var tcs = new TaskCompletionSource<object>();

            var commandMockA = new Mock<ICommand>();
            commandMockA
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockA
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(tcs.Task);

            var commandMockB = new Mock<ICommand>();
            commandMockB
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockB
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            var commandMockC = new Mock<ICommand>();
            commandMockC
                .Setup(m => m.CanExecute(It.Is<object>(v => v == null)))
                .Returns(true);
            commandMockC
                .Setup(m => m.ExecuteAsync(It.Is<object>(v => v == null)))
                .Returns(Task.Delay(-1));

            ICommand commandA = null;
            ICommand commandB = null;
            ICommand commandC = null;

            Manager
                .New()
                .AddGroup(
                    eg => commandA = eg.Register(commandMockA.Object),
                    GroupLockBehavior.LockNothing)
                .AddGroup(eg => commandB = eg.Register(commandMockB.Object))
                .AddGroup(eg => commandC = eg.Register(commandMockC.Object));

            // Act

            var _ = commandA.ExecuteAsync(null);

            // Assert

            commandA.Should().NotBeNull();
            commandMockA.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockA.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Once);
            commandA.CanExecute(null).Should().BeTrue("Command A should not block itself.");

            commandB.Should().NotBeNull();
            commandMockB.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockB.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandB.CanExecute(null).Should().BeTrue("Command A should not block Command B.");

            commandC.Should().NotBeNull();
            commandMockC.Verify(m => m.CanExecute(It.Is<object>(v => v == null)), Times.Never);
            commandMockC.Verify(m => m.ExecuteAsync(It.Is<object>(v => v == null)), Times.Never);
            commandC.CanExecute(null).Should().BeTrue("Command A should not block Command C.");

            tcs.SetResult(null);
        }
    }
}
