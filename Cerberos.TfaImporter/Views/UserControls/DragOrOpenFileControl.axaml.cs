using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace Cerberos.TfaImporter.Views.UserControls;

public partial class DragOrOpenFileControl : UserControl
{
    private TextBlock _dropState;
    private int _dragCount = 0;
    private string? _filePath = default;

    private readonly string[] _allowdExtensions = new[] { "png", "jpg", "jpeg" };

    public static DirectProperty<DragOrOpenFileControl, string?> FilePathProperty =
        AvaloniaProperty.RegisterDirect<DragOrOpenFileControl, string?>(nameof(FilePath), obj => obj.FilePath, (obj, val) => obj.FilePath = val, unsetValue: default, BindingMode.TwoWay);

    public string? FilePath
    {
        get => _filePath;
        set => SetAndRaise(FilePathProperty, ref _filePath, value);
    }
    
    
    public static readonly RoutedEvent<RoutedEventArgs> FileLoadedEvent =
        RoutedEvent.Register<DragOrOpenFileControl, RoutedEventArgs>(nameof(FileLoaded), RoutingStrategies.Bubble);

    public event EventHandler<RoutedEventArgs> FileLoaded
    { 
        add => AddHandler(FileLoadedEvent, value);
        remove => RemoveHandler(FileLoadedEvent, value);
    }

    public DragOrOpenFileControl()
    {
        Debug.WriteLine("DragOrOpenFileControl");
        InitializeComponent();

        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
        this.AddHandler(PointerReleasedEvent, MouseUpHandler, handledEventsToo: true);
    }

    private void DragOver(object sender, DragEventArgs e)
    {
        Debug.WriteLine("DragOver");
        // Only allow Copy or Link as Drop Operations.!
        e.DragEffects &= (DragDropEffects.Copy | DragDropEffects.Link);

        // Only allow if the dragged data contains text or filenames.
        if (!e.Data.Contains(DataFormats.FileNames) || !e.Data.GetFileNames().Any(obj => _allowdExtensions.Any(ext => obj.EndsWith(ext))))
            e.DragEffects = DragDropEffects.None;
    }
    
    private async void MouseUpHandler(object sender, PointerReleasedEventArgs e)
    {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.Filters.Add(new FileDialogFilter() {Name = "Supported image formats", Extensions = _allowdExtensions.ToList()});
        
        var result = await dlg.ShowAsync((Window)VisualRoot);
        if (result?.Length > 0)
        {
            _dropState.Text = string.Join(Environment.NewLine, result);
            FilePath = _dropState.Text;

            RaiseEvent(new RoutedEventArgs(FileLoadedEvent));
        }
    }

    private void Drop(object sender, DragEventArgs e)
    {
        Debug.WriteLine("Drop");
        if (e.Data.Contains(DataFormats.Text))
            _dropState.Text = e.Data.GetText();
        else if (e.Data.Contains(DataFormats.FileNames))
        {
            _dropState.Text = string.Join(Environment.NewLine, e.Data.GetFileNames());
            FilePath = _dropState.Text;
            RaiseEvent(new RoutedEventArgs(FileLoadedEvent));
        }
    }

    private void InitializeComponent()
    {
        Debug.WriteLine("InitializeComponent");
        AvaloniaXamlLoader.Load(this);

        _dropState = this.Find<TextBlock>("DropState");
    }
}