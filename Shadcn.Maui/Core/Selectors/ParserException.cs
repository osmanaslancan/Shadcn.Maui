using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadcn.Maui.Core.Selectors;


[Serializable]
public class ParserException : Exception
{
    public ParserException() { }
    public ParserException(string message) : base(message) { }
    public ParserException(string message, Exception inner) : base(message, inner) { }
    public ParserException(int index, string message)
        : base($"Error at index {index}: {message}")
    {
    }
}
