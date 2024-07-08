using Shadcn.Maui.Core;

namespace Shadcn.Maui.Behaviors;

[ContentProperty(nameof(Style))]
public class SmartStyleBehavior : Behavior<VisualElement>
{
    public string Selector
    {
        set => _selector = Core.Selector.Parse(new StringReader(value));
    }

    private Selector? _selector;
    private Style? _style;
    private bool attached = false;

    public Style? Style
    {
        get => _style;
        set
        {
            if (value is not null)
            {
                value.Class = Guid.NewGuid().ToString();
            }

            _style = value;
        }
    }

    protected override void OnAttachedTo(VisualElement ve)
    {
        if (_selector is null || _style is null)
            return;

        ve.Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs args)
    {
        if (sender is not VisualElement ve || attached)
            return;

        if (_selector!.Matches(ve))
        {
            if (!Application.Current!.Resources.ContainsKey("Microsoft.Maui.Controls.StyleClass." + _style!.Class))
                Application.Current!.Resources.Add(_style);

            ve.StyleClass ??= [];
            ve.StyleClass = ve.StyleClass.Concat([_style!.Class]).ToList();
            attached = true;
        }
    }

    protected override void OnDetachingFrom(VisualElement ve)
    {
        ve.Loaded -= OnLoaded;
        if (attached)
            ve.StyleClass.Remove(_style?.Class);
    }
}
