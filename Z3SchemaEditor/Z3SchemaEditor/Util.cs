using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Z3.Util
{
    /// <summary>
    /// A Generic EventArgs class
    /// </summary>
    /// <typeparam name="T">Value Type to include</typeparam>
    public class ObjectEventArgs<T> : EventArgs
    {
        public T Value;
        public object Tag;

        public ObjectEventArgs(T value)
            : this(value, null)
        {
        }

        public ObjectEventArgs(T value, object tag)
        {
            this.Value = value;
            this.Tag = tag;
        }
    }

    public interface Hotkeyable
    {
        string Name { get; }
        Keys Hotkey { get; set; }
    }

    public class Keyboard
    {
        public static Keys GetBaseKey(Keys _keychar)
        {
            Keys k = (Keys)((_keychar | Keys.Shift | Keys.Control | Keys.Alt) - (Keys.Shift | Keys.Control | Keys.Alt));
            switch (k)
            {
                case Keys.ControlKey:
                case Keys.Menu:
                case Keys.ShiftKey:
                    return Keys.None;
                default:
                    return k;
            }
        }

        public static string GetReadableKey(Keys value)
        {
            string k = "";
            if ((value & Keys.Control) > 0) k += "Ctrl-";
            if ((value & Keys.Alt) > 0) k += "Alt-";
            if ((value & Keys.Shift) > 0) k += "Shift-";
            return k + GetBaseKey(value).ToString();
        }

        public static bool IsMeaningful(Keys value)
        {
            return (GetBaseKey(value) != Keys.None);
        }
    }
}