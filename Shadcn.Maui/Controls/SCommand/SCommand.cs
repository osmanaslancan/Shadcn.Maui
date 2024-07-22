using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

[ContentProperty(nameof(Children))]
public class SCommand : TemplatedView
{
    public static readonly BindableProperty ChildrenProperty = BindableProperty.Create(
        nameof(Children),
        typeof(IList<View>),
        typeof(SCommand),
        defaultValueCreator: (BindableObject bindableObject) => new ObservableCollectionEx<View>());

    public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(
        nameof(SearchText),
        typeof(string),
        typeof(SCommand));

    public string SearchText
    {
        get { return (string)GetValue(SearchTextProperty); }
        set { SetValue(SearchTextProperty, value); }
    }

    public new IList<View> Children
    {
        get { return (IList<View>)GetValue(ChildrenProperty); }
        set { SetValue(ChildrenProperty, value); }
    }

    public SCommand()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            return new SBorder()
            {
                Content = new Grid
                {
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
