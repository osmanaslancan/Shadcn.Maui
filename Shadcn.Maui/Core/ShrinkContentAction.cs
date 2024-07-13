using Microsoft.Maui.Layouts;
using System.ComponentModel;

namespace Shadcn.Maui.Core;

public class ShrinkContentAction : TriggerAction<View>
{
    public enum ContentState
    {
        Close,
        Open
    }

    public ContentState State { get; set; }

    protected override async void Invoke(View sender)
    {
        if (State == ContentState.Open)
        {
            PropertyChangedEventHandler? handler = null;
            TaskCompletionSource<bool> tcs = new();
            handler = (s, e) =>
            {
                if (e.PropertyName == "Height")
                {
                    sender.PropertyChanged -= handler;
                    tcs.SetResult(true);
                }
            };
            sender.PropertyChanged += handler;
            sender.HeightRequest = -1;
            await tcs.Task;
        }
        //var parent = (Layout)sender.Parent;
        //var size = sender.ComputeDesiredSize(parent.Height, parent.Width);
        var height = sender.Height;
        sender.Animate("ShrinkContentAction", new Animation((d) =>
        {
            if (State == ContentState.Close)
            {
                sender.HeightRequest = height * (1 - d);
            }
            else
            {
                sender.HeightRequest = height * d;
            }
        }), length: 100);
    }
}
