﻿using Xunit;

namespace AlinSpace.Commands.Tests
{
    /// <summary>
    /// Tests for <see cref="AsyncCommand"/> and <see cref="Command{TParameter}"/>.
    /// </summary>
    public class CommandTests
    {
        /// <summary>
        /// Tests no execution.
        /// </summary>
        [Fact]
        public void NoExecution()
        {
            //// Setup
            //bool executeFlag = false;
            //var sut = FluentCommand
            //    .New()
            //    .OnExecuteAsync(async _ => executeFlag = true);

            //// Assert
            //sut.CanExecute(null).Should().BeTrue();
            //executeFlag.Should().BeFalse();
        }

        /// <summary>
        /// Tests execution.
        /// </summary>
        [Fact]
        public void Execution()
        {
            //// Setup
            //bool executeFlag = false;
            //var sut = FluentCommand
            //    .New()
            //    .OnExecuteAsync(async _ => executeFlag = true);

            //// Act
            //sut.ExecuteAsync(null).Wait();

            //// Assert
            //sut.CanExecute(null).Should().BeTrue();
            //executeFlag.Should().BeTrue();
        }

        /// <summary>
        /// Tests execution with VerifyCanExecuteBeforeExecution=true.
        /// </summary>
        [Fact]
        public void ExecutionWithVerifyBeforeExecution()
        {
            //// Setup
            //bool canExecuteFlag = false;
            //bool executeFlag = false;

            //var sut = FluentCommand
            //    .New(verifyCanExecuteBeforeExecution: true)
            //    .OnCanExecute(_ => { canExecuteFlag = true; return true; })
            //    .OnExecuteAsync(async _ => executeFlag = true);

            //// Act
            //sut.ExecuteAsync(null).Wait();

            //// Assert
            //sut.CanExecute(null).Should().BeTrue();
            //canExecuteFlag.Should().BeTrue();
            //executeFlag.Should().BeTrue();
        }

        /// <summary>
        /// Tests can not execute.
        /// </summary>
        [Fact]
        public void CanNotExecute()
        {
            //// Setup
            //bool executeFlag = false;
            //var sut = FluentCommand
            //    .New()
            //    .OnCanExecute(_ => false)
            //    .OnExecuteAsync(async _ => executeFlag = true);

            //// Assert
            //sut.CanExecute(null).Should().BeFalse();
            //executeFlag.Should().BeFalse();
        }
    }
}
