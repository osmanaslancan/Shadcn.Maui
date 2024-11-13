using System.ComponentModel;
using System.Diagnostics;

namespace Shadcn.Maui.Core.Selectors;

public abstract class Selector
{
    public abstract bool Matches(VisualElement styleable);
    public abstract void Bind(VisualElement styleable, Action action);
    public abstract void UnBind(VisualElement styleable);

    protected Dictionary<Guid, IList<Action<string?>>> _changeHandlers = new();

    private void PropertyChangedHandler(object? sender, PropertyChangedEventArgs args)
    {
        var ve = sender as VisualElement;
        Debug.Assert(ve != null);
        _changeHandlers.TryGetValue(ve.Id, out var handlers);
        Debug.Assert(handlers != null);
        foreach (var handler in handlers)
        {
            handler(args.PropertyName);
        }
    }

    protected void BindToProperty(VisualElement styleable, string propertyName, Action action)
    {
        var propertyAction = new Action<string?>(property =>
        {
            if (property == propertyName)
            {
                action();
            }
        });

        if (_changeHandlers.TryGetValue(styleable.Id, out var handlers))
        {
            handlers.Add(propertyAction);
        }
        else
        {
            _changeHandlers[styleable.Id] = [propertyAction];
            styleable.PropertyChanged += PropertyChangedHandler;
        }
    }

    protected void UnBindPropertyListener(VisualElement styleable)
    {
        if (_changeHandlers.TryGetValue(styleable.Id, out var handler))
        {
            styleable.PropertyChanged -= PropertyChangedHandler;
            _changeHandlers.Remove(styleable.Id);
        }
    }
}
