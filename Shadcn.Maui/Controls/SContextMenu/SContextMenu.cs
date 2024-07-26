using CommunityToolkit.Maui.Markup;

namespace Shadcn.Maui.Controls;

public class SContextMenu : SPopover
{
    private Point? _tapOffset;

    protected override GestureRecognizer GetTriggerGestureRecognizer()
    {
        var gesture = new TapGestureRecognizer()
        {
            Buttons = ButtonsMask.Secondary
        };

        gesture.Tapped += (s, e) =>
        {
            _tapOffset = e.GetPosition(TriggerView);
            IsOpen = true;
        };

        return gesture;
    }

    protected override Rect GetTriggerPosition()
    {
        var trigger = base.GetTriggerPosition();

        if (_tapOffset.HasValue)
        {
            var result = new Rect(trigger.X + _tapOffset.Value.X, trigger.Y + _tapOffset.Value.Y, 0, 0);
            return result;
        }

        return new Rect(trigger.X, trigger.Y, 0, 0);
    }

    protected override string TriggerStyleClass => "Shadcn-SContextMenuTriggerView";

    protected override double Spacing => 0;

    protected override void ConfigureAnchor(View view, SPopoverSide side)
    {
        switch (side)
        {
            case SPopoverSide.Bottom:
                view.Anchor(0, 0);
                break;
            case SPopoverSide.Top:
                view.Anchor(0, 1);
                break;
            default:
                break;
        }
    }
}
