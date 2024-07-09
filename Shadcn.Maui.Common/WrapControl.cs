namespace Shadcn.Maui.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class WrapsControlAttribute : Attribute
{
    public WrapsControlAttribute(Type controlType)
    {
        ControlType = controlType;
    }

    public Type ControlType { get; }
}
