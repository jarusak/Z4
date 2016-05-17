using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Zoopomatic2.Controls;

namespace Z3
{
    class Options
    {
        public const string REGISTRY_KEY = @"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3";

        private static bool _invert_loaded = false;
        private static bool _invert_value;

        private static bool _dotsize_loaded = false;
        private static int _dotsize_value;

        private static bool _hcolor_loaded = false;
        private static Color _hcolor_value;

        private static bool _acolor_loaded = false;
        private static Color _acolor_value;

        private static bool _places_loaded = false;
        private static int _places_value;

        private static Measurer measurer;

        public static Measurer Measurer
        {
            get
            {
                return measurer;
            }

            set
            {
                measurer = value;
            }
        }
        
        public static bool InvertMouseButtons
        {
            get {
                if (!_invert_loaded)
                {
                    _invert_value = (GetNumeric("AlternateMouseMode") > 0);
                    _invert_loaded = true;
                }
                return _invert_value;
            }
            set
            {
                _invert_value = value;
                _invert_loaded = true;
                SetNumeric("AlternateMouseMode", value? 1:0);
            }
        }

        public static int DotSize
        {
            get {
                if (!_dotsize_loaded)
                {
                    _dotsize_value = GetNumeric("DotSize");
                    _dotsize_loaded = true;
                }
                return _dotsize_value;
            }
            set
            {
                _dotsize_value = value;
                _dotsize_loaded = true;
                SetNumeric("DotSize", value);
            }
        }

        public static int GetNumeric(string name)
        {
            object retval = Registry.GetValue(REGISTRY_KEY, name, null);
            if (retval == null) return GetDefaultNumeric(name);
            return Convert.ToInt32(retval);
        }

        public static void SetNumeric(string name, int value)
        {
            Registry.SetValue(REGISTRY_KEY, name, value);
        }

        public static Color HistoryColor
        {
            get {
                if (!_hcolor_loaded)
                {
                    object valueR = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "HistoryColorR", null);
                    object valueG = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "HistoryColorG", null);
                    object valueB = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "HistoryColorB", null);
                    if (valueR == null) _hcolor_value = Color.Orange;
                    else {
                        int r = (Convert.ToInt32(valueR));
                        int g = (Convert.ToInt32(valueG));
                        int b = (Convert.ToInt32(valueB));
                        _hcolor_value = Color.FromArgb(r,g,b);
                    }
                    _hcolor_loaded = true;
                }
                return _hcolor_value;
            }
            set
            {
                _hcolor_value = value;
                _hcolor_loaded = true;
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "HistoryColorR", value.R);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "HistoryColorG", value.G);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "HistoryColorB", value.B);
                if (Measurer != null) Measurer.notifyColorChange();
            }
        }
        
        public static Color ActiveColor
        {
            get {
                if (!_acolor_loaded)
                {
                    object valueR = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "ActiveColorR", null);
                    object valueG = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "ActiveColorG", null);
                    object valueB = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "ActiveColorB", null);
                    if (valueR == null) _acolor_value = Color.Red;
                    else {
                        int r = (Convert.ToInt32(valueR));
                        int g = (Convert.ToInt32(valueG));
                        int b = (Convert.ToInt32(valueB));
                        _acolor_value = Color.FromArgb(r,g,b);
                    }
                    _acolor_loaded = true;
                }
                return _acolor_value;
            }
            set
            {
                _acolor_value = value;
                _acolor_loaded = true;
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "ActiveColorR", value.R);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "ActiveColorG", value.G);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "ActiveColorB", value.B);
                if (Measurer != null) Measurer.notifyColorChange();
            }
        }

        public static int RoundingPlaces
        {
            get {
                if (!_places_loaded)
                {
                    object value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "DecimalPlaces", 5);
                    _places_value = (Convert.ToInt32(value));
                    _places_loaded = true;
                }
                return _places_value;
            }
            set
            {
                _places_value = value;
                _places_loaded = true;
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "DecimalPlaces", value);
            }
        }

        public static string LastWorkspace
        {
            get
            {
                object retval = Registry.GetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "RecentFile", null);
                if (retval != null && !retval.Equals(""))
                {
                    return retval.ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "RecentFile", "");
                }
                else
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\UW Center for Limnology\CountingFramework\3", "RecentFile", value);
                }
            }
        }

        public static int GetDefaultNumeric(string name)
        {
            if (name.Equals("Z3FloatingControlsFormX")) return 6;
            if (name.Equals("Z3FloatingControlsFormY")) return 72;
            if (name.Equals("Z3FloatingControlsFormW")) return 226;
            if (name.Equals("Z3FloatingControlsFormH")) return 328;
            if (name.Equals("Z3FloatingMeasurementFormX")) return 6;
            if (name.Equals("Z3FloatingMeasurementFormY")) return 409;
            if (name.Equals("Z3FloatingMeasurementFormW")) return 542;
            if (name.Equals("Z3FloatingMeasurementFormH")) return 151;
            if (name.Equals("Z3FloatingMenuFormX")) return 6;
            if (name.Equals("Z3FloatingMenuFormY")) return 6;
            if (name.Equals("Z3FloatingVideoFormX")) return 240;
            if (name.Equals("Z3FloatingVideoFormY")) return 72;
            if (name.Equals("Z3FloatingVideoFormW")) return 308;
            if (name.Equals("Z3FloatingVideoFormH")) return 328;
            return -1;
        }
    }
}
