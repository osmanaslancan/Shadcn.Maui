using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Core;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

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

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(SCommandItem));

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(SCommandItem));

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

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
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
                .Bind(BindableLayout.ItemsSourceProperty, nameof(Children), source: RelativeBindingSource.TemplatedParent)
                .SetBindableValue(BindableLayout.ItemTemplateProperty,
                    new DataTemplate(() => new ContentView()
                    {
                        Content = new ContentView()
                        {
                        }
                        .Bind(ContentView.BindingContextProperty, "BindingContext", source: RelativeBindingSource.TemplatedParent)
                        .Bind(ContentView.ContentProperty, "BindingContext", source: new RelativeBindingSource(RelativeBindingSourceMode.FindAncestor, typeof(ContentView)))
                    }))
            }
            .BindTapGesture(nameof(Command), this, nameof(CommandParameter), this)
            .Bind(SBorder.PaddingProperty, nameof(Padding), source: this);
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
