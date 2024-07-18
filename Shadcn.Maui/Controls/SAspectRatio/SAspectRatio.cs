using Microsoft.Maui.Layouts;
using Shadcn.Maui.Core;
using System.ComponentModel;

namespace Shadcn.Maui.Controls;

public class SAspectRatio : ContentView
{
    public static readonly BindableProperty AspectRatioProperty =
        BindableProperty.Create(nameof(AspectRatio), typeof(double), typeof(SAspectRatio), 1.0);

    [TypeConverter(typeof(StringDivisionToDoubleTypeConverter))]
    public double AspectRatio
    {
        get { return (double)GetValue(AspectRatioProperty); }
        set { SetValue(AspectRatioProperty, value); }
    }
   
    protected override Size ArrangeOverride(Rect bounds)
    {
        var computedFrame = this.ComputeFrame(bounds);
        if (AspectRatio > 0)
        {
            double width = computedFrame.Width;
            double height = computedFrame.Height;
            double calculatedHeight = width / AspectRatio;
            double calculatedWidth = height * AspectRatio;
            if (calculatedHeight <= height)
            {
                computedFrame.Height = calculatedHeight;
            }
            else
            {
                computedFrame.Width = calculatedWidth;
            }
        }

        Frame = computedFrame;
        Handler?.PlatformArrange(Frame);
        return Frame.Size;
    }
}
