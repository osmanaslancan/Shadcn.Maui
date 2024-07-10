using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinRT;

namespace Shadcn.Maui.Behaviors;

public partial class EntryExtensionsBehavior : PlatformBehavior<Entry, TextBox>
{
    private Dictionary<TextBox, Action<TextBox>> _restores = new();


    
    protected override void OnAttachedTo(Entry bindable, TextBox platformView)
    {
        var oldBorderThickness = platformView.BorderThickness;
        platformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);

        FrameworkElement childRoot = (FrameworkElement)VisualTreeHelper.GetChild(platformView, 0);

        var commonStatesGroup = Microsoft.UI.Xaml.VisualStateManager.GetVisualStateGroups(childRoot).First(x => x.Name == "CommonStates");
        var focusedState = commonStatesGroup.States.First(x => x.Name == "Focused");
        commonStatesGroup.States.Remove(focusedState);

        _restores[platformView] = (restoringItem) =>
        {
            commonStatesGroup.States.Add(focusedState);
            restoringItem.BorderThickness = oldBorderThickness;
        };

    }

    protected override void OnDetachedFrom(Entry bindable, TextBox platformView)
    {
        if (_restores.TryGetValue(platformView, out var restore))
        {
            restore(platformView);
            _restores.Remove(platformView);
        }
    }
}