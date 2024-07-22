using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Core;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Shadcn.Maui.Controls;

[ContentProperty(nameof(Children))]
public class SCommandItem : TemplatedView
{
    public static readonly BindableProperty ChildrenProperty = BindableProperty.Create(
       nameof(Children),
       typeof(IList<View>),
       typeof(SCommand),
       defaultValueCreator: (BindableObject bindableObject) => new ObservableCollectionEx<View>());

    public static new readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(SBorder), Thickness.Zero);

    public new IList<View> Children
    {
        get { return (IList<View>)GetValue(ChildrenProperty); }
        set { SetValue(ChildrenProperty, value); }
    }

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public SCommandItem()
    {
        StyleClass = ["Shadcn-SCommandItem"];
        ControlTemplate = new ControlTemplate(() =>
        {
            return new SBorder()
            {
                StyleClass = ["Shadcn-SCommandItem-Border"],
                Content = new HorizontalStackLayout()
                {
                    Spacing = 10,
                }
                .Bind(BindableLayout.ItemsSourceProperty, nameof(Children), source: this)
                .SetBindableValue(BindableLayout.ItemTemplateProperty, new DataTemplate(() => new ContentPresenter().Bind(ContentPresenter.ContentProperty)))
            }.Bind(SBorder.PaddingProperty, nameof(Padding), source: this);
        });

        this.Bind(SCommandItem.IsVisibleProperty, binding1: new Binding(".", source: Children), binding2: new Binding(nameof(SCommand.SearchText), source: new RelativeBindingSource(RelativeBindingSourceMode.FindAncestor, typeof(SCommand))),
            convert: ((IList<View>? Children, string? SearchText) binds) =>
            {
                if (string.IsNullOrEmpty(binds.SearchText))
                {
                    return true;
                }

                return binds.Children!.OfType<Label>().Any(label => label.Text.Contains(binds.SearchText, StringComparison.CurrentCultureIgnoreCase));
            });
    }
}
