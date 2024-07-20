using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Handlers;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SLabel : Label
{
    public static readonly BindableProperty ForProperty = BindableProperty.Create(nameof(For), typeof(View), typeof(SLabel), propertyChanging: OnForChanging, propertyChanged: OnForChanged);

    public View For
    {
        get { return (View)GetValue(ForProperty); }
        set { SetValue(ForProperty, value); }
    }

    public SLabel()
    {
    }

    private static void OnForChanging(BindableObject bindable, object oldValue, object newValue)
    {

    }

    private static void OnForChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SLabel)bindable;
        var target = (View)newValue;

        self.GestureRecognizers.Add(new TapGestureRecognizer()
        {
            Command = new RelayCommand(() =>
            {
                if (target.Handler is ViewHandler vh)
                {
                    vh.ProgrammaticClick();
                }
            })
        });
    }
}
