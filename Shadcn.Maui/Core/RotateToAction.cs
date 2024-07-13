namespace Shadcn.Maui.Core;

public class RotateToAction : TriggerAction<View>
{
    public double ToDegree { get; set; }

    protected override void Invoke(View sender)
    {
        var start = sender.Rotation;
        sender.Animate("RotateToAction", new Animation((d) =>
        {
            var current = start + (ToDegree - start) * d;
            sender.Rotation = current;
        }), length:100);
    }
}
