using System.Threading.Tasks;
using Xunit;

namespace AlinSpace.Commands.Tests
{
    public class PruteForceTests
    {
        public class Counter
        {
            public int Number { get; set; }

            public bool Failed { get; set; }
        }

        [Fact]
        public void Test()
        {
            Counter counter = new Counter();

            int numberOfCommands = 10;

            ICommand[] commands = new ICommand[numberOfCommands];

            for(int i = 0; i < numberOfCommands; i++)
            {
                commands[i] = Command.New().OnExecuteAsync(async _ =>
                {
                    counter.Number += 1;

                    await Task.Delay(1);

                    if (counter.Number != 1)
                    {
                        counter.Failed = true;
                    }

                    counter.Number -= 1;
                });
            }

            ICommand[] registeredCommands = new ICommand[numberOfCommands];

            var manager = Manager.New();

            for (int i = 0; i < numberOfCommands; i++)
            {
                manager.LockAll(x =>
                {
                    registeredCommands[i] = x.Register(commands[i]);
                });
            }

            long numberOfIncrements = 10000;

            Task[] tasks = new Task[numberOfIncrements];

            Parallel.For(0, 5000, async value =>
            {
                var indexOfCommand = value % numberOfCommands;
                await registeredCommands[indexOfCommand].ExecuteAsync();
            });

            Assert.False(counter.Failed);
        }
    }
}
