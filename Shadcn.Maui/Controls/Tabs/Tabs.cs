using Shadcn.Maui.Core;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace Shadcn.Maui.Controls;

[ContentProperty(nameof(TabContents))]
public partial class Tabs : ContentView
{
    public static readonly BindableProperty TabTriggersProperty = BindableProperty.Create(nameof(TabTriggers), typeof(TabTriggerList), typeof(Tabs), new TabTriggerList());

    public TabTriggerList TabTriggers
    {
        get { return (TabTriggerList)GetValue(TabTriggersProperty); }
        set { SetValue(TabTriggersProperty, value); }
    }

    public static readonly BindableProperty TabTriggerControlTemplateProperty = BindableProperty.Create(nameof(TabTriggerControlTemplate), typeof(ControlTemplate), typeof(Tabs), null);

    public ControlTemplate TabTriggerControlTemplate
    {
        get { return (ControlTemplate)GetValue(TabTriggerControlTemplateProperty); }
        set { SetValue(TabTriggerControlTemplateProperty, value); }
    }

    public static readonly BindableProperty TabContentsProperty = BindableProperty.Create(nameof(TabContents), typeof(ObservableCollectionEx<TabContent>), typeof(Tabs), new ObservableCollectionEx<TabContent>());

    public ObservableCollectionEx<TabContent> TabContents
    {
        get { return (ObservableCollectionEx<TabContent>)GetValue(TabContentsProperty); }
        set { SetValue(TabContentsProperty, value); }
    }

    public static readonly BindableProperty TabContentControlTemplateProperty = BindableProperty.Create(nameof(TabContentControlTemplate), typeof(ControlTemplate), typeof(Tabs), null);

    public ControlTemplate TabContentControlTemplate
    {
        get { return (ControlTemplate)GetValue(TabContentControlTemplateProperty); }
        set { SetValue(TabContentControlTemplateProperty, value); }
    }

    public static readonly BindableProperty ActiveTabProperty = BindableProperty.Create(nameof(ActiveTab), typeof(string), typeof(Tabs), null, propertyChanged: OnActiveTabChanged);

    public string? ActiveTab
    {
        get { return (string?)GetValue(ActiveTabProperty); }
        set { SetValue(ActiveTabProperty, value); }
    }

    public TabContent? ActiveContent => TabContents.FirstOrDefault(x => x.TabName == ActiveTab);

    private int? _internalActiveTabIndex = null;

    private void UpdateInternalActiveTabIndex(int newValue)
    {
        _internalActiveTabIndex = newValue;
        if (_internalActiveTabIndex.HasValue && _internalActiveTabIndex.Value < TabTriggers.Count)
        {
            ActiveTab = TabTriggers[_internalActiveTabIndex.Value].Value;
        }
        foreach (var (index, trigger) in TabTriggers.Index())
        {
            trigger.IsActive = index == _internalActiveTabIndex;
        }

        OnPropertyChanged(nameof(ActiveContent));
    }

    private static void OnActiveTabChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        var self = (Tabs)bindableObject;
        var index = self.TabTriggers.ToList().FindIndex(x => x.Value == (string)newValue);
        if (index >= 0)
        {
            self.UpdateInternalActiveTabIndex(index);
        }
        else
        {
            throw new InvalidOperationException($"Invalid ActiveTab value. Tab named {newValue} not found.");
        }
    }

    private ICommand _tabTriggerClickCommand;

    public Tabs()
    {
        TabTriggers.CollectionChanged += TabTriggers_CollectionChanged;
        TabContents.CollectionChanged += TabContents_CollectionChanged;
        _tabTriggerClickCommand = new Command<string>(OnTabTriggerClick);
    }

    private void OnTabTriggerClick(string tabName)
    {
        ActiveTab = tabName;
    }

    private void TabContents_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_internalActiveTabIndex is null && TabContents.Count > 0)
        {
            UpdateInternalActiveTabIndex(0);
        }
        if (_internalActiveTabIndex is not null && _internalActiveTabIndex.Value >= TabContents.Count)
        {
            UpdateInternalActiveTabIndex(TabContents.Count - 1);
        }
    }

    private void TabTriggers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in TabTriggers)
        {
            item.ClickCommand = _tabTriggerClickCommand;
        }
    }
}
