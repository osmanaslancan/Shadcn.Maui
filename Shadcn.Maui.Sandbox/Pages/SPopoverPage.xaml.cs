using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using Shadcn.Maui.Controls;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Sandbox.Pages;

public partial class SPopoverPage : ContentPage
{
    public SPopoverPage()
    {
        InitializeComponent();
        border.TapGesture(ShowPopupAsync);
    }


    private async void ShowPopupAsync()
    {
        var (x, y) = border.PositionRelativeToPage();
        
        Popup? popup = null;

        popup = new Popup()
        {
            Size = new Size(Width, Height),
            Color = Colors.Transparent,
            Content = new Grid
            {
                new AbsoluteLayout()
                {
                    BackgroundColor = Colors.Transparent,
                    Children =
                    {
                        new SCommand()
                        {
                            MinimumWidthRequest = border.Width,
                            Children =
                            {
                                new SCommandInput()
                                {
                                    Placeholder = "Search Framework..."
                                },
                                new SCommandGroup()
                                {
                                    Children =
                                    {
                                        new SCommandItem()
                                        {
                                            Children =
                                            {
                                                new SLabel() { Text = "Next.js" }
                                            }
                                        },
                                        new SCommandItem()
                                        {
                                            Children =
                                            {
                                                new SLabel() { Text = "SvelteKit" }
                                            }
                                        },
                                        new SCommandItem()
                                        {
                                            Children =
                                            {
                                                new SLabel() { Text = "Nuxt.js" }
                                            }
                                        },
                                        new SCommandItem()
                                        {
                                            Children =
                                            {
                                                new SLabel() { Text = "Remix" }
                                            }
                                        },
                                        new SCommandItem()
                                        {
                                            Children =
                                            {
                                                new SLabel() { Text = "Astro" }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        .Assign(out SCommand self)
                        .Bind(AbsoluteLayout.LayoutBoundsProperty, nameof(Height), source: self, convert: (double height) => new Rect(x, y + 90, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)),
                    }
                }.TapGesture(() => popup!.Close())
            }
        };

        popup.Opened += (s, e) =>
        {
            popup.Window.RemoveOverlay(popup.Window.Overlays.First());
        };

        await this.ShowPopupAsync(popup);
    }
}