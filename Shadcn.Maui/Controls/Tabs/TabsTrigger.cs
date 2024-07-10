using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Shadcn.Maui.Controls;

public partial class TabsTrigger : ObservableObject
{
    private bool isActive;
    private ICommand? clickCommand;

    public required string Name { get; init; }
    public required string Value { get; init; }

    public bool IsActive
    {
        get => isActive;
        internal set
        {
            if (isActive == value)
            {
                return;
            }
            OnPropertyChanging();
            isActive = value;
            OnPropertyChanged();
        }
    }

    public ICommand? ClickCommand
    {
        get => clickCommand;
        internal set
        {
            if (clickCommand == value)
            {
                return;
            }
            OnPropertyChanging();
            clickCommand = value;
            OnPropertyChanged();
        }
    }
}
