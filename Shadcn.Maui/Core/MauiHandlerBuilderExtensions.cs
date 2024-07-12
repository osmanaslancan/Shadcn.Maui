namespace Shadcn.Maui.Core;

public static class MauiHandlerBuilderExtensions
{
    public static IMauiHandlersCollection AddHandlerWithFallback<TElement, THandler, TFallback>(this IMauiHandlersCollection builder)
        where TElement : IElement
        where THandler : new()
        where TFallback : IElementHandler
    {
        if (typeof(IElementHandler).IsAssignableFrom(typeof(THandler)))
        {
            builder.AddHandler(typeof(TElement), typeof(THandler));
        }
        else
        {
            builder.AddHandler(typeof(TElement), typeof(TFallback));
        }

        return builder;
    }
}
