using FluentAssertions;
using Xunit;

namespace FluentCommands.Tests
{
    /// <summary>
    /// Tests for <see cref="FluentCommand"/> and <see cref="FluentCommand{TParameter}"/>.
    /// </summary>
    public class FluentCommandTests
    {
        [Fact]
        public void TestExecute()
        {
            // Setup

            bool executeFlag = false;

            var command = FluentCommand
                .New()
                .OnExecute(p => executeFlag = true);

            // Act

            command.Execute(null);

            // Assert

            command.CanExecute(null).Should().BeTrue();
            executeFlag.Should().BeTrue();
        }

        [Fact]
        public void TestCanExecuteFalse()
        {
            // Setup

            bool executeFlag = false;

            var command = FluentCommand
                .New()
                .OnCanExecute(_ => false)
                .OnExecute(_ => executeFlag = true);

            // Act

            // Assert

            command.CanExecute(null).Should().BeFalse();
            executeFlag.Should().BeFalse();
        }
    }
}
