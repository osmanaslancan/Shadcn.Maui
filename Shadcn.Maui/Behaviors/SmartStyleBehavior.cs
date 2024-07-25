using Shadcn.Maui.Core;
using System.Diagnostics;

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
    private Dictionary<Guid, State> stateBag = new();

    private record State()
    {
        public bool AppliedClass { get; set; }
        public bool ListeningStyle { get; set; }
    }

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

        stateBag[ve.Id] = new State
        {
            AppliedClass = false,
            ListeningStyle = false
        };

        ve.Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs args)
    {
        if (sender is not VisualElement ve || !stateBag.TryGetValue(ve.Id, out var state))
            return;

        CheckStyle(ve);


        if (state.ListeningStyle)
            return;

        _selector!.Bind(ve, () => CheckStyle(ve));
        state.ListeningStyle = true;

    }

    private void CheckStyle(VisualElement ve)
    {
        if (!stateBag.TryGetValue(ve.Id, out var state))
            Debug.Assert(false);


        if (_selector!.Matches(ve) && !state.AppliedClass)
        {
            if (!Application.Current!.Resources.ContainsKey("Microsoft.Maui.Controls.StyleClass." + _style!.Class))
                Application.Current!.Resources.Add(_style);

            ve.StyleClass ??= [];
            ve.StyleClass = ve.StyleClass.Concat([_style!.Class]).ToList();
            state.AppliedClass = true;
        }
        else if (!_selector!.Matches(ve) && state.AppliedClass)
        {
            if (ve.StyleClass != null)
            {
                ve.StyleClass.Remove(_style!.Class);
                ve.StyleClass = [..ve.StyleClass];
            }
            state.AppliedClass = false;
        }
    }

    protected override void OnDetachingFrom(VisualElement ve)
    {
        if (!stateBag.TryGetValue(ve.Id, out var state))
            Debug.Assert(false);


        ve.Loaded -= OnLoaded;
        if (state.AppliedClass)
            ve.StyleClass.Remove(_style?.Class);
        if (state.ListeningStyle)
            _selector!.UnBind(ve);

        stateBag.Remove(ve.Id);
    }
}
