using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

[ContentProperty(nameof(Children))]
public class SCommandGroup : TemplatedView
{
    public static readonly BindableProperty HeadingProperty = BindableProperty.Create(nameof(Heading), typeof(string), typeof(SCommandGroup), default(string));
    public static readonly BindableProperty ChildrenProperty = BindableProperty.Create(
        nameof(Children), 
        typeof(IList<View>), 
        typeof(SCommandGroup), 
        defaultValueCreator: (BindableObject bindableObject) => new ObservableCollectionEx<View>());

    public string Heading
    {
        get { return (string)GetValue(HeadingProperty); }
        set { SetValue(HeadingProperty, value); }
    }

    public new IList<View> Children
    {
        get { return (IList<View>)GetValue(ChildrenProperty); }
        set { SetValue(ChildrenProperty, value); }
    }

    public SCommandGroup()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            return new VerticalStackLayout()
            {
                Padding = 2,
                Children =
                {
                    new SLabel
                    {
                        StyleClass = ["Shadcn-SCommandGroup-Heading"],
                    }.Bind(Label.TextProperty, nameof(Heading), source: this),
                    new VerticalStackLayout()
                    {
                    }
                    .Bind(BindableLayout.ItemsSourceProperty, nameof(Children), source: this)
                    .SetBindableValue(BindableLayout.ItemTemplateProperty, new DataTemplate(() => new ContentPresenter().Bind(ContentPresenter.ContentProperty)))
                }
            };
        });
    }
}
