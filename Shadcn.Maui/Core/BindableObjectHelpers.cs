namespace Shadcn.Maui.Core;

internal static class BindableObjectHelpers
{
    public static void ToggleBehavior<T, TTargetType>(BindableObject bindableObject, bool attach)
        where TTargetType : VisualElement 
        where T : new()
    {
        if (!typeof(T).IsSubclassOf(typeof(Behavior)))
        {
            return;
            //throw new InvalidOperationException($"This behavior is not supported in this platform.");
        }

        if (bindableObject is not TTargetType target)
        {
            throw new InvalidOperationException($"This behavior cannot be applied to {nameof(TTargetType)}");
        }

        if (attach)
        {
            if (!target.Behaviors.Any(x => x is T))
            {
                target.Behaviors.Add(new T() as Behavior);
            }
        }
        else
        {
            Behavior? toRemove = target.Behaviors.FirstOrDefault(b => b is T);
            if (toRemove != null)
            {
                target.Behaviors.Remove(toRemove);
            }
        }
    }
}
