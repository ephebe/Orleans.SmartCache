using System;
using System.Collections.Generic;
using System.Text;

namespace Grains.StateHolder
{
    public class StateHolderStatus<T>
    {
        public StateHolderStatus() : this(default(T))
        {
        }

        public StateHolderStatus(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
