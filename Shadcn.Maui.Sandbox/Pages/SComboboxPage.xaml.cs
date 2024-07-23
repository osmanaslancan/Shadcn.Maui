using System.Diagnostics;
using System.Windows.Input;

namespace Shadcn.Maui.Sandbox.Pages;

public partial class SComboboxPage : ContentPage
{
    private string? _selectedItem = null;
    private bool _isOpen = false;

    public string? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem == value)
                return;

            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (_isOpen == value)
                return;

            _isOpen = value;
            OnPropertyChanged();
        }
    }

    public ICommand SelectCommand { get; set; }

    public SComboboxPage()
    {
        SelectCommand = new Command<string>(item =>
        {
            SelectedItem = item;
            IsOpen = false;
        });
        InitializeComponent();
    }
}
