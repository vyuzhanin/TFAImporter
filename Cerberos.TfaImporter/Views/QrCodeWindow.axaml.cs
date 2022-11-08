using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Cerberos.TfaImporter.Views;

public partial class QrCodeWindow : Window
{
    public QrCodeWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}