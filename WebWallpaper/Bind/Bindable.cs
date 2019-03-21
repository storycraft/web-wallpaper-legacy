using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebWallpaper.Bind
{
    public class Bindable<T>
    {

        public delegate void ValueUpdateEventArgs(T oldValue, T newValue);

        public event ValueUpdateEventArgs OnChange;

        private T val;
        public T Value
        {
            get => val;
            set
            {
                T oldVal = val;

                val = value;

                if (val != null && !val.Equals(oldVal))
                {
                    OnChange?.Invoke(oldVal, val);
                }
            }
        }

        public Bindable(T defaultValue)
        {
            val = defaultValue;
        }

        public override string ToString()
        {
            return "Bindable: " + val.ToString();
        }

    }
}
