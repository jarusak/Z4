using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using Z3.Leger;
using System.Data;

namespace Z3.Model {
    public abstract class Z3Entity {
        protected abstract void listenerAdd();
        protected abstract void listenerEdit();
        protected abstract void listenerDelete();

        protected void getNew(int level, int parent, string fields, object[] obs) {
            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "insert into " + Z3Level.getName(_table, level) + _table + " (Name, Parent" + fields + ") values('New Item',?";
                cmd.Parameters.AddWithValue("@parent", parent);
                int i=0;
                foreach (object ob in obs) {
                    cmd.CommandText += ",?";
                    cmd.Parameters.AddWithValue("@something" + (i++), ob);
                }
                cmd.CommandText += ")";
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Could not create a new record.");
            }
            _type = level;
            _parentid = parent;
            _id = Z3Schema.getInstance().getIdentity();
            listenerAdd();
        }

        protected abstract void readDelegate(SqlCeDataReader r);

        protected void getExisting(int level, int id) {
            _type = level;
            _id = id;
            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "select * from " + Z3Level.getName(_table, level) + " where internalid=?";
                cmd.Parameters.AddWithValue("@iid", _id);
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    if (r.Read()) {
                        ID = Convert.ToInt32(r["internalid"]);
                        Name = r["name"].ToString();
                        ParentID = Convert.ToInt32(r["parent"]);
            
                        readDelegate(r);
                    } else
                        throw new InvalidOperationException("Could not create a new record.");
                }
            }
            listenerAdd();
        }

        protected abstract void deleteDelegate();

        protected void doDelete() {
            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "delete from " + Z3Level.getName(_table, TypeID) + _table + " where internalid=" + ID;
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Failed to delete the record.");
            }
            deleteDelegate();
            listenerDelete();
        }

        
        
        
        protected Z3Entity(string suffix) {
            _table = suffix;
        }

        private int _id;
        private string _table;

        public int ID {
            get { return _id; }
            set { _id = value; }
        }
        private int _parentid;

        public int ParentID {
            get { return _parentid; }
            set { _parentid = value; }
        }

        private int _type;

        public int TypeID {
            get { return _type; }
            set { _type = value; }
        }

        public Z3Level Type {
            get { return Z3Level.getType(_table, _type); }
        }

        private string _name;

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public void SetValues(Dictionary<Z3Field, Z3FieldValue> values) {
            saveEntityFields(values);
        }

        public void Delete() {
            doDelete();
        }

        public Dictionary<Z3Field, Z3FieldValue> GetValues() {
            Dictionary<Z3Field, Z3FieldValue> retval = new Dictionary<Z3Field, Z3FieldValue>();
            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "select * from " + Type.Name + _table + " where internalid=" + ID;
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    if (r.Read()) {
                        foreach (Z3Field f in Type.Fields) {
                            retval[f] = new Z3FieldValue(f, r[f.Name]);
                        }
                    } else {
                        throw new RowNotInTableException();
                    }
                }
                return retval;
            }
        }

        public void saveEntityFields(Dictionary<Z3Field, Z3FieldValue> fields) {
            bool first = true;
            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "update " + Type.Name + _table + " set ";
                foreach (Z3FieldValue v in fields.Values) {
                    if (!first)
                        cmd.CommandText += ", ";
                    else first = false;
                    cmd.CommandText += v.Field.Name + "=?";
                    cmd.Parameters.AddWithValue(v.Field.Name, v.DatabaseValue);
                }
                cmd.CommandText += " where internalid=" + ID;
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Failed to update the record.");
            }
            listenerEdit();
        }

        public abstract Z3Entity fromFactoryReader(int type, SqlCeDataReader r);

        protected Z3Entity factorize(Z3Level level, Z3Entity entity) {
            return factorize(level.ID, (entity!=null?entity.ID:0));
        }

        public static Dictionary<string, Z3Entity> Factories {
            get { return _factories; }
        }
        
        public static Z3Entity Factory(string type) {
            return _factories[type.ToLowerInvariant()];
        }

        protected abstract Z3Entity factorize(int level, int entity);
        public abstract void Subscribe(EntityWatcher e);
        public abstract void Unsubscribe(EntityWatcher e);

        internal static Z3Entity createNew(string _category, Z3Level z3Level, Z3Entity z3Entity) {
            return _factories[_category.ToLowerInvariant()].factorize(z3Level, z3Entity);
        }

        internal static Z3Entity createNew(string _category, int z3Level, int z3Entity) {
            return _factories[_category.ToLowerInvariant()].factorize(z3Level, z3Entity);
        }

        protected static Dictionary<string, Z3Entity> _factories = new Dictionary<string,Z3Entity>();
    }
}
