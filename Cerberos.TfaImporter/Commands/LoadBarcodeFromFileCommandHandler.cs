using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Cerberos.TfaImporter.Commands.Base;
using Cerberos.TfaImporter.ViewModels;
using ReactiveUI;

namespace Cerberos.TfaImporter.Commands;

public class LoadBarcodeFromFileCommandHandler : ICommandAsyncHandler
{
    private readonly ViewModelBase _viewModel;
    private readonly Action<Stream> _fillBarcodeStreamCallback;
    private readonly Func<string> _receiveBarcodePath;

    public LoadBarcodeFromFileCommandHandler(ViewModelBase viewModel, Action<Stream> fillBarcodeStreamCallback, Func<string> receiveBarcodePath)
    {
        _viewModel = viewModel;
        _fillBarcodeStreamCallback = fillBarcodeStreamCallback;
        _receiveBarcodePath = receiveBarcodePath;
    }

    public Task InvokeAsync()
    {
        var barcodePath = _receiveBarcodePath();

        if (barcodePath?.Length > 0)
        {
            _fillBarcodeStreamCallback(File.OpenRead(barcodePath));
                
            ((IReactiveObject)_viewModel).RaisePropertyChanged(new PropertyChangedEventArgs("LoadButtonEnabled"));
            
            return Task.CompletedTask;
        }

        throw new ArgumentNullException(nameof(barcodePath));
    }
}