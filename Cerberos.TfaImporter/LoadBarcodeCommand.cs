using System;
using System.Reactive;
using System.Reactive.Concurrency;
using ReactiveUI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Cerberos.TfaImporter;

public class LoadBarcodeCommand : ReactiveCommand<Image<Rgba32>, Unit>
{
    protected internal LoadBarcodeCommand(Func<Image<Rgba32>, IObservable<Unit>> execute, IObservable<bool>? canExecute, IScheduler? outputScheduler) : base(execute, canExecute, outputScheduler)
    {
    }
}