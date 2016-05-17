using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using Z3.Util;
using System.Windows.Forms;

namespace Z3.Model {
    public class ZField {
        private int _id;

        public int Id {
            get { return _id; }
            set { _id = value; }
        }
        private int _levelid;

        public int Levelid {
            get { return _levelid; }
            set { _levelid = value; }
        }
        private ZLevel _level;

        public ZLevel Level {
            get { return _level; }
            set { _level = value; }
        }
        private String _name;

        public String Name {
            get { return _name; }
            set { _name = value; }
        }
        private String _label;

        public String Label {
            get { return _label; }
            set { _label = value; }
        }
        private String _default;

        public String Default {
            get { return _default; }
            set { _default = value; }
        }
        private String _type;

        public String Type {
            get { return _type; }
            set { _type = value; }
        }

        public object ReadableFormat(object v) {
            if (_name.ToLower() == "hotkey")
            {
                if (Keyboard.IsMeaningful((Keys)v))
                {
                    return Keyboard.GetReadableKey((Keys)v);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return v;
            }
        }
        
        public object DatabaseFormat(object v) {
            return v;
        }

        public static ZField fromReader(SqlCeDataReader r) {
            ZField retval = new ZField();
            retval._id = Convert.ToInt32(r["internalid"]);
            retval._name = r["name"].ToString();
            retval._label = r["label"].ToString();
            retval._default = r["default"].ToString();
            retval._type = r["type"].ToString();
            retval._levelid = Convert.ToInt32(r["level"]);
            return retval;
        }

        public override String ToString() {
            return _name;
        }

        public static ZField getDummyField(string name, string type, string defaultv)
        {
            ZField f = new ZField();
            f._id = -1;
            f._name = name;
            f._type = type;
            f._default = defaultv;
            return f;
        }
    }
}
