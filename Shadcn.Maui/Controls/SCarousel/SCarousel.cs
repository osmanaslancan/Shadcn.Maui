using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Mvvm.Input;
using Shadcn.Maui.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;
namespace Shadcn.Maui.Controls;

public class SCarousel : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty = CarouselView.ItemsSourceProperty;
    public static readonly BindableProperty ItemTemplateProperty = CarouselView.ItemTemplateProperty;
    public static readonly BindableProperty PositionProperty = BindableProperty.Create(
        propertyName: nameof(Position),
        returnType: typeof(int),
        declaringType: typeof(SCarousel),
        defaultValue: 0);

    public int Position
    {
        get { return (int)GetValue(PositionProperty); }
        set { SetValue(PositionProperty, value); }
    }

    public IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    private void BindToCarousel(CarouselView view)
    {
        view
            .Bind(CarouselView.ItemTemplateProperty, nameof(ItemTemplate), source: this)
            .Bind(CarouselView.PositionProperty, nameof(Position), source: this)
            .Bind(CarouselView.ItemsSourceProperty, nameof(ItemsSource), source: this);
    }

    private CarouselView _innerCarouselView;

    private ICommand goToNextCommand;

    public SCarousel()
    {
        _innerCarouselView = new CarouselView
        {
            BackgroundColor = Colors.Blue,
            PeekAreaInsets = 0,
        };

        goToNextCommand = new RelayCommand(() =>
        {
            if (ItemsSource is IList list)
            {
                var currentIndex = list.IndexOf(_innerCarouselView.CurrentItem);

                //_innerCarouselView.CurrentItem = list[(currentIndex + 1) % list.Count];
                _innerCarouselView.ScrollTo((currentIndex + 1) % list.Count);
            }
            else
            {
                _innerCarouselView.ScrollTo(Position + 1);
            }
        });

        BindToCarousel(_innerCarouselView);
        ControlTemplate = new ControlTemplate(() =>
        {
            return new Grid
            {
                ColumnDefinitions = Columns.Define(35, Star, 35),
                Children = 
                {
                    new SBorder()
                    {
                        StyleClass = ["Shadcn-SCarousel-Button"],
                        Content = new SIcon()
                        {
                            StyleClass = ["Shadcn-SCarousel-ButtonIcon"],
                            Icon = Icons.ArrowLeft,
                        }
                    },
                    new Border()
                    {
                        Content = _innerCarouselView,
                    }.Column(1),
                    new SBorder()
                    {
                        StyleClass = ["Shadcn-SCarousel-Button"],
                        Content = new SIcon()
                        {
                            StyleClass = ["Shadcn-SCarousel-ButtonIcon"],
                            Icon = Icons.ArrowRight,
                            GestureRecognizers =
                            {
                                new TapGestureRecognizer()
                                {
                                    Command = goToNextCommand,
                                }
                            }
                        }
                    }.Column(2)
                }
            };
        });
    }
}
