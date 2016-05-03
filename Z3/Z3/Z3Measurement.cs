using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;

namespace Z3.Model {
    public class Z3Measurement {
        private Z3Measurement() { }

        private int _id;

        public int ID {
            get { return _id; }
            set { _id = value; }
        }
        private bool _measured;

        public bool Measured {
            get { return _measured; }
            set { _measured = value; }
        }
        private bool _autocount;

        public bool AutoCount {
            get { return _autocount; }
            set { _autocount = value; }
        }
        private bool _required;

        public bool Required {
            get { return _required; }
            set { _required = value; }
        }
        private String _name;

        public String Name {
            get { return _name; }
            set { _name = value; }
        }

        public override String ToString() {
            return _name;
        }

        public static Z3Measurement fromReader(SqlCeDataReader r) {
            Z3Measurement m = new Z3Measurement();
            m._id = Convert.ToInt32(r["internalid"]);
            m._name = r["name"].ToString();
            m._autocount = Convert.ToBoolean(r["autocount"]);
            m._measured = Convert.ToBoolean(r["measured"]);
            m._required = Convert.ToBoolean(r["required"]);
            return m;
        }
    }
}
