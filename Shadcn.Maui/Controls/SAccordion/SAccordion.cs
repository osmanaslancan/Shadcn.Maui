using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

[ContentProperty(nameof(Items))]
public class SAccordion : ContentView
{
    public enum Type
    {
        Single,
        Multiple
    }

    public static readonly BindableProperty ItemsProperty =
     BindableProperty.Create(nameof(Items), typeof(ObservableCollectionEx<SAccordionItem>), typeof(SAccordion));

    public static readonly BindableProperty AccordionTypeProperty =
     BindableProperty.Create(nameof(AccordionType), typeof(Type), typeof(SAccordion), defaultValue: Type.Single);

    public ObservableCollectionEx<SAccordionItem> Items
    {
        get { return (ObservableCollectionEx<SAccordionItem>)GetValue(ItemsProperty); }
        set { SetValue(ItemsProperty, value); }
    }

    public Type AccordionType
    {
        get { return (Type)GetValue(AccordionTypeProperty); }
        set { SetValue(AccordionTypeProperty, value); }
    }

    public SAccordion()
    {
        Items = [];
        Items.CollectionChanged += Items_CollectionChanged;
    }

    private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            foreach (SAccordionItem item in e.NewItems!)
            {
                item.PropertyChanged += OnChildPropertyChanged;
            }
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        {
            foreach (SAccordionItem item in e.OldItems!)
            {
                item.PropertyChanged -= OnChildPropertyChanged;
            }
        }

    }

    private void OnChildPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SAccordionItem.IsExpanded) && sender is SAccordionItem accordionItem)
        {
            if (AccordionType == Type.Multiple)
            {
                return;
            }

            if (accordionItem.IsExpanded == false)
                return;

            foreach (var item in Items)
            {
                if (item != sender)
                {
                    item.IsExpanded = false;
                }
            }
        }
    }
}
