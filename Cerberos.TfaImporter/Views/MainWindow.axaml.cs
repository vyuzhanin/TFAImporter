using Avalonia.Controls;
using Avalonia.Interactivity;
using Cerberos.TfaImporter.ViewModels;

namespace Cerberos.TfaImporter.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DragOrOpenFileControl_OnFileLoaded(object? sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                viewModel.OnSelectImageClickCommand.Execute();

                viewModel.OnLoadBarcodeClickCommand.Execute();
            }
        }
    }
}