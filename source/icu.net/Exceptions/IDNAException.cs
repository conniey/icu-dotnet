using System;

namespace Icu
{
    public class IDNAException : Exception
    {
        public IDNAException(string message) : base(message)
        { }
    }
}
