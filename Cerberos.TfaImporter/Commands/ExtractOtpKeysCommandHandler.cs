using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Cerberos.TfaImporter.Commands.Base;
using Cerberos.TfaImporter.DTO;
using Cerberos.TfaImporter.Models;
using Cerberos.TfaImporter.ViewModels;
using Google.Protobuf;
using Cerberos.TfaImporter.DTO.Proto;
using ReactiveUI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;

namespace Cerberos.TfaImporter.Commands;

public class ExtractOtpKeysCommandHandler: ICommandAsyncHandler
{
    private readonly Func<Stream> _receiveBarcodeStreamCallback;
    private readonly ZXing.ImageSharp.BarcodeReader<Rgba32> _barcodeReader = new();
    private readonly ViewModelBase _viewModel;
    private readonly Action<string> _fillEncodedUrlCallback;
    private readonly Action<IEnumerable<DecodedTokenDto>> _fillExtractedTokensCallback;
    private readonly ProtobufService _protobufService = new ();

    public ExtractOtpKeysCommandHandler(Func<Stream> receiveBarcodeStreamCallback, ViewModelBase viewModel, 
        Action<string> fillEncodedUrlCallback, Action<IEnumerable<DecodedTokenDto>> fillExtractedTokensCallback)
    {
        _receiveBarcodeStreamCallback = receiveBarcodeStreamCallback;
        _viewModel = viewModel;
        _fillEncodedUrlCallback = fillEncodedUrlCallback;
        _fillExtractedTokensCallback = fillExtractedTokensCallback;
        
        _barcodeReader.AutoRotate = true;
        _barcodeReader.Options.TryHarder = true;
        _barcodeReader.Options.PureBarcode = false;
        _barcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
        _barcodeReader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
    }

    public async Task InvokeAsync()
    {
        var barcodeStream = _receiveBarcodeStreamCallback();
        var result = await _protobufService.ReceiveTokensAsync(barcodeStream);
        
        _fillEncodedUrlCallback(result.Item1);
        _fillExtractedTokensCallback(result.Item2);


        ((IReactiveObject)_viewModel).RaisePropertyChanged(new PropertyChangedEventArgs("DecodedResult"));
        ((IReactiveObject)_viewModel).RaisePropertyChanged(new PropertyChangedEventArgs("ExtractedTokens"));
        
    }


}