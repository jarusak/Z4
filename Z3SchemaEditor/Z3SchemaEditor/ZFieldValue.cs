using System;
using System.Collections.Generic;
using System.Text;

namespace Z3.Model {
    public class ZFieldValue {
        private ZField _field;
        
        public ZField Field {
            get { return _field; }
            set { _field = value; }
        }
        
        private object _value;

        public object ReadableValue {
            get { return _field.ReadableFormat(_value); }
        }

        public object Value {
            set { _value = value; }
        }

        public object DatabaseValue {
            get { return _field.DatabaseFormat(_value); }
        }

        public ZFieldValue(ZField f, object v) {
            Field = f;
            Value = v;
        }
    }
}
