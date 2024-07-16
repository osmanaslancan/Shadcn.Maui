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

    public static Style<T> SetPointerOverVisualState<T>(this Style<T> style, params (BindableProperty property, object value)[] setters)
        where T : BindableObject
    {
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
                    }.Add(setters),
                }
            }
        });

        return style;
    }
}
