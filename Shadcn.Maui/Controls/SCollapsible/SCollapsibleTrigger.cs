using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Mvvm.Input;
using Shadcn.Maui.Core;
using System.Runtime.CompilerServices;

namespace Shadcn.Maui.Controls;

public class SCollapsibleTrigger : ContentView
{
    private readonly TapGestureRecognizer gestureRecognizer;
    public SCollapsibleTrigger()
    {
        gestureRecognizer = new TapGestureRecognizer()
        {
            Command = new RelayCommand(ToggleParent)
        };
    }

    private void ToggleParent()
    {
        var parent = this.FindParentOfType<SCollapsible>();
        if (parent is not null)
            parent.IsCollapsed = !parent.IsCollapsed;
    }

    private void OnButtonClicked(object? sender, EventArgs e)
    {
        ToggleParent();
    }

    private void OnContentChanging()
    {
        if (Content is Button button)
        {
            button.Clicked -= OnButtonClicked;
        }
        else
        {
            Content?.GestureRecognizers.Remove(gestureRecognizer);
        }
    }

    private void OnContentChanged()
    {
        if (Content is Button button)
        {
            button.Clicked += OnButtonClicked;
        }
        else
        {
            Content?.GestureRecognizers.Add(gestureRecognizer); 
        }
    }

    protected override void OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanging(propertyName);
        if (propertyName == nameof(Content))
        {
            OnContentChanging();
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(Content))
        {
            OnContentChanged();
        }
    }
}
