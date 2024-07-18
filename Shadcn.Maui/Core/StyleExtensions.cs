using CommunityToolkit.Maui.Markup;


namespace Shadcn.Maui.Core;

public static class StyleExtensions
{
    public static VisualState Add(this VisualState visualState, params (BindableProperty property, object value)[] setters)
    {
        foreach (var (property, value) in setters)
        {
            visualState.Add(property, value);
        }
        return visualState;
    }

    public static VisualState Add(this VisualState visualState, BindableProperty property, object value)
    {
        visualState.Setters.Add(property, value);
        return visualState;
    }

    private static VisualState Add<T>(this VisualState visualState, Style<T> style)
        where T : BindableObject
    {
        foreach (var setter in style.MauiStyle.Setters)
        {
            visualState.Setters.Add(setter);
        }

        return visualState;
    }
    public static Trigger Add(this Trigger trigger, BindableProperty property, object value)
    {
        trigger.Setters.Add(property, value);
        return trigger;
    }

    public static Trigger Add(this Trigger trigger, params (BindableProperty property, object value)[] setters)
    {
        foreach (var (property, value) in setters)
        {
            trigger.Add(property, value);
        }

        return trigger;
    }

    public static Style<T> SetPointerOverVisualState<T>(this Style<T> style, Action<Style<T>> configure)
        where T : BindableObject
    {
        var proxyStyle = new Style<T>();
        configure(proxyStyle);

        style.Add(VisualStateManager.VisualStateGroupsProperty, new VisualStateGroupList
        {
            new VisualStateGroup
            {
                Name = "CommonStates",
                States = 
                {
                    new VisualState
                    {
                        Name = "Normal"
                    },
                    new VisualState
                    {
                        Name = "Disabled"
                    },
                    new VisualState
                    {
                        Name = "Focused"
                    },
                    new VisualState
                    {
                        Name = "Selected"
                    },
                    new VisualState
                    {
                        Name = "PointerOver",
                    }.Add(proxyStyle)
                }
            }
        });

        return style;
    }
}
