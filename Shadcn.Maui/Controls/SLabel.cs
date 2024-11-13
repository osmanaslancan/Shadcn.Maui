using CommunityToolkit.Mvvm.Input;

namespace Shadcn.Maui.Controls;

public partial class SLabel : Label
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
        var self = (SLabel)bindable;
        self.GestureRecognizers.Clear();
    }

    private static void OnForChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SLabel)bindable;
        var target = (View)newValue;

        self.GestureRecognizers.Add(new TapGestureRecognizer()
        {
            Command = new RelayCommand(() =>
            {
                target.Focus();
            })
        });
    }
}
