using System.Collections.Generic;
using NiftyFramework.Core.Commands;

namespace Commands
{
    public class CommandRunner
    {
        public struct ProcessResponse
        {
            public int Processed;
            public int Skipped;
            public int Total;

            public int Remaining => Total - Processed;
        }
        
        private List<ICommand> _commands = new List<ICommand>();
        public IReadOnlyList<ICommand> Commands => _commands;

        public bool IsEmpty()
        {
            return _commands == null || _commands.Count == 0;
        }

        public void Add(ICommand command)
        {
            if (!_commands.Contains(command))
            {
                _commands.Add(command);
            }
            
        }

        public void Process()
        {
            for (int i = 0; i < _commands.Count; i++)
            {
                ICommand command = _commands[i];
                if (command is IAsyncCommand asyncCommand)
                {
                    _commands.Remove(command);
                    asyncCommand.Execute(HandleAsyncCommandComplete);
                    return;
                }
                command.Execute();
            }
        }

        private void HandleAsyncCommandComplete(IAsyncCommand command, bool success)
        {
            if (_commands.Count > 0)
            {
                Process();
            }
        }
    }
}