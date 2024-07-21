namespace Shadcn.Maui.Controls;

using CommunityToolkit.Maui.Markup;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

[ContentProperty(nameof(Children))]
public partial class SCard : ContentView
{
    public static readonly BindableProperty ChildrenProperty =
        BindableProperty.Create(nameof(Children), typeof(ObservableCollection<IView>), typeof(SCard), defaultValueCreator: (obj) => new ObservableCollection<IView>());

    new public ObservableCollection<IView> Children
    {
        get => (ObservableCollection<IView>)GetValue(ChildrenProperty);
        set => SetValue(ChildrenProperty, value);
    }

    public static new readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(SCard), default(Thickness));

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public SCard()
    {
        StyleClass = ["Shadcn-SCard"];
    }
}
