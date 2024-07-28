using Shadcn.Maui.Core;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Shadcn.Maui.Behaviors;

[ContentProperty(nameof(Style))]
public class SmartStyleBehavior : Behavior<VisualElement>
{
    
    private static readonly ConcurrentDictionary<VisualElement, List<SmartStyleBehavior>> ElementBehaviors = new ConcurrentDictionary<VisualElement, List<SmartStyleBehavior>>();

    public string Selector
    {
        set => _selector = Core.Selector.Parse(new StringReader(value));
    }

    public int Order { get; set; } = 0;

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

        ElementBehaviors.AddOrUpdate(ve, [this], (element, old) =>
        {
            old.Add(this);
            return old;
        });

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

    private (List<string> externClasses, Dictionary<SmartStyleBehavior, string> smartStyleClasses) SeparateClasses(VisualElement element)
    {
        var smartStyleClasses = new Dictionary<SmartStyleBehavior, string>();
        var externClasses = new List<string>(element.StyleClass);
        if (ElementBehaviors.TryGetValue(element, out var behaviors))
        {
            foreach (var behavior in behaviors)
            {
                if (behavior == this)
                    continue;

                if (smartStyleClasses.ContainsKey(behavior))
                    continue;

                if (element.StyleClass.Contains(behavior.Style!.Class))
                {
                    smartStyleClasses.Add(behavior, behavior.Style!.Class);
                    externClasses.Remove(behavior.Style!.Class);
                }
            }
        }

        

        return (externClasses, smartStyleClasses);
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
            var (externClasses, smartStyleClasses) = SeparateClasses(ve);

            smartStyleClasses.Add(this, Style!.Class);

            ve.StyleClass = [..smartStyleClasses.Where(x => x.Key.Order < 0).OrderBy(x => x.Key.Order).Select(x => x.Value), ..externClasses, ..smartStyleClasses.Where(x => x.Key.Order >= 0).OrderBy(x => x.Key.Order).Select(x => x.Value)];

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

        if (ElementBehaviors.TryGetValue(ve, out var list))
            list.Remove(this);


        ve.Loaded -= OnLoaded;
        if (state.AppliedClass)
            ve.StyleClass.Remove(_style?.Class);
        if (state.ListeningStyle)
            _selector!.UnBind(ve);

        stateBag.Remove(ve.Id);
    }
}
