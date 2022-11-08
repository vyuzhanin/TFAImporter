using System.Reactive;
using System.Threading.Tasks;
using Cerberos.TfaImporter.Commands.Base;
using ReactiveUI;

namespace Cerberos.TfaImporter.Commands;

public static class CommandFactory
{
    public static ReactiveCommand<Unit, Unit> CreateCommand(ICommandHandler handler)
    {
        return ReactiveCommand.Create(handler.Invoke);
    }
    
    public static ReactiveCommand<Unit, Unit> CreateAsyncCommand(ICommandAsyncHandler handler)
    {
        return ReactiveCommand.CreateFromTask(handler.InvokeAsync);
    }
}