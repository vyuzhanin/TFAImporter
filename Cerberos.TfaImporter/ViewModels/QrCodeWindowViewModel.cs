using System.IO;
using Avalonia.Media.Imaging;
using Cerberos.TfaImporter.DTO;
using Cerberos.TfaImporter.Models;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;

namespace Cerberos.TfaImporter.ViewModels;

public class QrCodeWindowViewModel : ViewModelBase
{
    public Bitmap GeneratedQrCode { get; private set; }
    private readonly ProtobufService _protobufService = new ();

    private readonly ZXing.ImageSharp.BarcodeWriter<Rgba32> _barcodeWriter = new();

    public QrCodeWindowViewModel(DecodedTokenDto decodedToken)
    {
        _barcodeWriter.Options.Width = 590;
        _barcodeWriter.Options.Height = 590;
        
        _barcodeWriter.Format = BarcodeFormat.QR_CODE;
        
        var data = _protobufService.GenerateOauthPathUrl(decodedToken);

        var result = _barcodeWriter.Write(data);

        using MemoryStream ms = new MemoryStream();
        result.Save(ms, new GifEncoder());
        ms.Position = 0;
        GeneratedQrCode = new Bitmap(ms);
    }


}