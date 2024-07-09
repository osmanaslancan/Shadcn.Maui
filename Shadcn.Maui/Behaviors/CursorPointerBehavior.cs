namespace Shadcn.Maui.Behaviors;

public partial class CursorPointerBehavior : Behavior<View>
{
    public static readonly BindableProperty CursorPointerProperty =
        BindableProperty.CreateAttached("CursorPointer", typeof(bool), typeof(CursorPointerBehavior), false, propertyChanged: OnAttachBehaviorChanged);

    public static bool GetCursorPointer(BindableObject view)
    {
        return (bool)view.GetValue(CursorPointerProperty);
    }

    public static void SetCursorPointer(BindableObject view, bool value)
    {
        view.SetValue(CursorPointerProperty, value);
    }

    static void OnAttachBehaviorChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        if (bindableObject is not View view)
        {
            return;
        }

        bool attachBehavior = (bool)newValue;
        if (attachBehavior)
        {
            view.Behaviors.Add(new CursorPointerBehavior());
        }
        else
        {
            Behavior? toRemove = view.Behaviors.FirstOrDefault(b => b is CursorPointerBehavior);
            if (toRemove != null)
            {
                view.Behaviors.Remove(toRemove);
            }
        }
    }

    private void OnHandlerChanged(object? sender, EventArgs e)
    {
        if (sender is not View visualElement)
            return;

        if (visualElement.Handler is not null)
            CursorPointer(visualElement);
    }

    protected override void OnAttachedTo(View view)
    {
        if (view.Handler != null)
            CursorPointer(view);

        view.HandlerChanged += OnHandlerChanged;
    }

    protected override void OnDetachingFrom(View view)
    {
        CursorArrow(view);
    }

    partial void CursorPointer(View view);
    partial void CursorArrow(View view);
}
