using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using System.ComponentModel;

namespace Shadcn.Maui.Behaviors;

public partial class LabelExtensionsBehavior : PlatformBehavior<Label, TextBlock>
{
    protected override void OnAttachedTo(Label bindable, TextBlock platformView)
    {
        platformView.IsTextSelectionEnabled = true;
        bindable.PropertyChanged += LabelPropertyChanged;
    }

    private void LabelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Label label && (e.PropertyName == "SelectStart" || e.PropertyName == "SelectEnd"))
        {
            UpdateSelection(label);
        }
    }

    private void UpdateSelection(Label label)
    {
        var start = GetSelectStart(label);
        var end = GetSelectEnd(label);

        var platformView = (TextBlock)label.Handler!.PlatformView!;

        if (start >= 0 && end >= 0)
        {
            var startPointer = platformView.ContentStart.GetPositionAtOffset(start, LogicalDirection.Forward);
            var endPointer = platformView.ContentStart.GetPositionAtOffset(end, LogicalDirection.Forward);
            platformView.Focus(FocusState.Programmatic);
            platformView.Select(startPointer, endPointer);
            
        }
    }

    protected override void OnDetachedFrom(Label bindable, TextBlock platformView)
    {
        platformView.IsTextSelectionEnabled = false;
        bindable.PropertyChanged -= LabelPropertyChanged;
    }   
}
