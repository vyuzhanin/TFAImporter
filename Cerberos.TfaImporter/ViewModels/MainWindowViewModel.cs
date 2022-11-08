using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Cerberos.TfaImporter.Commands;
using Cerberos.TfaImporter.DTO;
using Cerberos.TfaImporter.Views;
using ReactiveUI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Cerberos.TfaImporter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Stream? _barcodeStream;
        
        public string LoadBarcodeCaption => "Load Barcode";
        public string SelectImageCaption => "Select Image";
        public bool LoadButtonEnabled => !string.IsNullOrEmpty(BarcodeImagePath);

        public string BarcodeImagePath { get; set; }

        public IEnumerable<DecodedTokenDto> ExtractedTokens { get; set; }

        public MainWindowViewModel()
        {

            OnSelectImageClickCommand = CommandFactory.CreateAsyncCommand(new LoadBarcodeFromFileCommandHandler(this, (r) => _barcodeStream = r, () => BarcodeImagePath));
            
            OnLoadBarcodeClickCommand = CommandFactory.CreateAsyncCommand(new ExtractOtpKeysCommandHandler(()=> _barcodeStream, 
                this, (r) => DecodedResult = r, (et) => ExtractedTokens = et));
            

        }
        
        private void DragOrOpenFileControl_OnFileLoaded(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
        
        public string DecodedResult { get; set; } = "";

        public ReactiveCommand<Unit, Unit> OnSelectImageClickCommand { get; }

        public ReactiveCommand<Unit, Unit> OnLoadBarcodeClickCommand { get; }
        
        public async Task Button_OnClick(DecodedTokenDto decodedToken)
        {

            var dialog = new QrCodeWindow
            {
                DataContext = new QrCodeWindowViewModel(decodedToken)
            };
            
            await dialog.ShowDialog<QrCodeWindowViewModel?>(this.FindWindowByViewModel());
        }
    }
}
