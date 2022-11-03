using NiftyFramework.Core.Commands;

namespace Commands
{
    public delegate void Completed(IAsyncCommand command, bool success = true);
    public interface IAsyncCommand : ICommand
    {
        public void Execute(Completed OnDone);
    }
}