using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Shadcn.Maui.Controls;

public class SAccordionItem : ContentView
{
    public static readonly BindableProperty TriggerTemplateProperty =
      BindableProperty.Create(nameof(TriggerTemplate), typeof(DataTemplate), typeof(SAccordionItem), propertyChanged: OnTriggerTemplateChanged);

    public DataTemplate TriggerTemplate
    {
        get { return (DataTemplate)GetValue(TriggerTemplateProperty); }
        set { SetValue(TriggerTemplateProperty, value); }
    }

    public static readonly BindableProperty ContentTemplateProperty =
        BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(SAccordionItem), propertyChanged: OnContentTemplateChanged);

    public DataTemplate ContentTemplate
    {
        get { return (DataTemplate)GetValue(ContentTemplateProperty); }
        set { SetValue(ContentTemplateProperty, value); }
    }

    public static readonly BindableProperty IsExpandedProperty =
        BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(SAccordionItem), defaultValue: false);

    public bool IsExpanded
    {
        get { return (bool)GetValue(IsExpandedProperty); }
        set { SetValue(IsExpandedProperty, value); }
    }

    private static void OnContentTemplateChanged(BindableObject bindableObject, object? oldValue, object? newValue)
    {
        var self = (SAccordionItem)bindableObject;

        var content = (View)self.ContentTemplate.CreateContent();
        content.BindingContext = self.BindingContext;
        self._contentTemplateView = content;

        self.OnPropertyChanged(nameof(ContentTemplateView));
    }

    private View? _contentTemplateView;

    public View? ContentTemplateView => _contentTemplateView;

    private static void OnTriggerTemplateChanged(BindableObject bindableObject, object? oldValue, object? newValue)
    {
        var self = (SAccordionItem)bindableObject;

        var trigger = (View)self.TriggerTemplate.CreateContent();
        trigger.BindingContext = self.BindingContext;
        self._triggerTemplateView = trigger;

        self.OnPropertyChanged(nameof(TriggerTemplateView));
    }

    private View? _triggerTemplateView;
  

    public View? TriggerTemplateView => _triggerTemplateView;

    public static readonly BindableProperty ToggleCommandProperty =
      BindableProperty.Create(nameof(ToggleCommand), typeof(ICommand), typeof(SAccordionItem));

    public ICommand ToggleCommand
    {
        get { return (ICommand)GetValue(ToggleCommandProperty); }
        set { SetValue(ToggleCommandProperty, value); }
    }

    public SAccordionItem()
    {
        ToggleCommand = new RelayCommand(() =>
        {
            IsExpanded = !IsExpanded;
        });
    }
}
