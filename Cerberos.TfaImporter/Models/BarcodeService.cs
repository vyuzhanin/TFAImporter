using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;

namespace Cerberos.TfaImporter.Models;

public class BarcodeService
{
    private readonly ZXing.ImageSharp.BarcodeReader<Rgba32> _barcodeReader = new();
    
    public BarcodeService()
    {
        _barcodeReader.AutoRotate = true;
        _barcodeReader.Options.TryHarder = true;
        _barcodeReader.Options.PureBarcode = false;
        _barcodeReader.Options.PossibleFormats = new List<BarcodeFormat>();
        _barcodeReader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
    }

    public async Task<string?> DecodeBarcodeAsync(Stream barcodeStaream)
    {
        using var barcodeImage = await Image.LoadAsync<Rgba32>(barcodeStaream);
        var result = _barcodeReader.Decode(barcodeImage);

        return result?.Text;
    }
}