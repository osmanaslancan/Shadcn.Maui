using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadcn.Maui.Core.Selectors;

internal class ElementSelector(string name) : Selector
{
    private List<string> GetNames(VisualElement styleable)
    {
        var list = new List<string>();
        var t = styleable.GetType();
        while (t != typeof(BindableObject))
        {
            list.Add(t!.Name);
            t = t.BaseType;
        }
        return list;
    }

    public override bool Matches(VisualElement styleable)
    {
        return GetNames(styleable).Contains(name);
    }
    public override void Bind(VisualElement styleable, Action action)
    {
    }
    public override void UnBind(VisualElement styleable)
    {
    }
}
