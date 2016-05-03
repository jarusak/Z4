using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;

namespace Z3.Model {
    public class Z3Field {
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
        private Z3Level _level;

        public Z3Level Level {
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
                if (Convert.ToChar(v) == 0)
                    return "";
                else
                    return new String(Convert.ToChar(v), 1);
            else
                return v;
        }
        
        public object DatabaseFormat(object v) {
            if (_name.ToLower() == "hotkey")
                if (v == null)
                    return 0;
                else if (v.ToString().Length == 0)
                    return 0;
                else 
                    return Convert.ToInt32(v.ToString()[0]);
            else
                return v;
        }

        public static Z3Field fromReader(SqlCeDataReader r) {
            Z3Field retval = new Z3Field();
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
    }
}
