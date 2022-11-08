using System.Threading.Tasks;

namespace Cerberos.TfaImporter.Commands.Base;

public interface ICommandAsyncHandler
{
    Task InvokeAsync();
}