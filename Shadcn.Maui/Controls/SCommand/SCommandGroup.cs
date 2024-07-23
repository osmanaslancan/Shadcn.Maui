using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Core;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Shadcn.Maui.Controls;

[ContentProperty(nameof(Children))]
public class SCommandGroup : TemplatedView, IBindableLayout
{
    public static readonly BindableProperty HeadingProperty = BindableProperty.Create(nameof(Heading), typeof(string), typeof(SCommandGroup), default(string));
    public static readonly BindablePropertyKey ChildrenPropertyKey = BindableProperty.CreateReadOnly(
        nameof(Children), 
        typeof(IList<View>), 
        typeof(SCommandGroup), 
        null,
        defaultValueCreator: (BindableObject bindableObject) => new ObservableCollectionEx<View>());

    public string Heading
    {
        get { return (string)GetValue(HeadingProperty); }
        set { SetValue(HeadingProperty, value); }
    }

    public new IList<View> Children
    {
        get { return (IList<View>)GetValue(ChildrenPropertyKey.BindableProperty); }
    }

    IList IBindableLayout.Children => (ObservableCollectionEx<View>)Children;

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
                    }
                    .Bind(Label.TextProperty, nameof(Heading), source: this)
                    .Bind(Label.IsVisibleProperty, nameof(Heading), source: this, convert: static (string? heading) => heading is not null),
                    new VerticalStackLayout()
                    {
                    }
                    .Bind(BindableLayout.ItemsSourceProperty, nameof(Children), source: this)
                    .SetBindableValue(BindableLayout.ItemTemplateProperty, new DataTemplate(() => new ContentPresenter().Bind(ContentPresenter.ContentProperty)))
                }
            };
        });
        ((ObservableCollectionEx<View>)Children).CollectionChanged += OnChildrenCollectionChanged;
    }

    private void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SCommandItem.IsVisible))
        {
            IsVisible = Children.Any(c => c.IsVisible);
        }
    }

    private void OnChildrenCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems is not null)
        {
            foreach (var item in e.NewItems)
            {
                ((View)item).PropertyChanged += ChildPropertyChanged;
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems is not null)
        {
            foreach (var item in e.OldItems)
            {
                ((View)item).PropertyChanged -= ChildPropertyChanged;
            }
        }
    }
}
