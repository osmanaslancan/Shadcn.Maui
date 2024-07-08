namespace Shadcn.Maui.Controls;

using CommunityToolkit.Maui.Markup;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

[ContentProperty(nameof(Children))]
public class SCard : ContentView
{
    public static readonly BindableProperty ChildrenProperty =
        BindableProperty.Create(nameof(Children), typeof(ObservableCollection<IView>), typeof(SCard), new ObservableCollection<IView>());

    new public ObservableCollection<IView> Children
    {
        get => (ObservableCollection<IView>)GetValue(ChildrenProperty);
    }

    private VerticalStackLayout _layout;

    public SCard()
    {
        Children.CollectionChanged += Children_CollectionChanged;

        Content = new Border
        {
            StyleClass = ["SCard.Border"],
            Content = new VerticalStackLayout
            {
            }.Assign(out _layout)
        };
    }

    private void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (IView view in e.NewItems!)
            {
                if (view is View v)
                {
                    _layout.Children.Add(v);
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (IView view in e.OldItems!)
            {
                if (view is View v)
                {
                    _layout.Children.Remove(v);
                }
            }
        }
    }

    private static void HeaderChanging(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SCard)bindable;
        if (oldValue is not null)
        {
            self._layout.Children.Remove((View)oldValue);
        }
        if (newValue is not null)
        {
            var newView = (View)newValue;
            newView.Row(0);
            self._layout.Children.Add(newView);
        }
    }

    private static void CardContentChanging(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SCard)bindable;
        if (oldValue is not null)
        {
            self._layout.Children.Remove((View)oldValue);
        }
        if (newValue is not null)
        {
            var newView = (View)newValue;
            newView.Row(1);
            self._layout.Children.Add(newView);
        }
    }

    private static void FooterChanging(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SCard)bindable;
        if (oldValue is not null)
        {
            self._layout.Children.Remove((View)oldValue);
        }
        if (newValue is not null)
        {
            var newView = (View)newValue;
            newView.Row(2);
            self._layout.Children.Add(newView);
        }
    }
}
