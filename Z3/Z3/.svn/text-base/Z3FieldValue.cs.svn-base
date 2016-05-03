using System;
using System.Collections.Generic;
using System.Text;

namespace Z3.Model {
    public class Z3FieldValue {
        private Z3Field _field;
        
        public Z3Field Field {
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

        public Z3FieldValue(Z3Field f, object v) {
            Field = f;
            Value = v;
        }
    }
}
