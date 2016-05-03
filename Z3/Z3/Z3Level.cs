using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using Z3.Leger;

namespace Z3.Model {
    public class Z3Level {
        private static Dictionary<string, Dictionary<int, string>> _names = new Dictionary<string,Dictionary<int,string>>();
        private static Dictionary<string, Dictionary<int, Z3Level>> _cache = new Dictionary<string, Dictionary<int, Z3Level>>();

        public static ICollection<Z3Level> Types(string table) {
            table = table.ToLowerInvariant();

            if (!_cache.ContainsKey(table))
                return new List<Z3Level>();
            return _cache[table].Values;
        }

        public static Z3Level getType(string table, int level) {
            table = table.ToLowerInvariant();
            
            if (!_cache.ContainsKey(table))
                _cache[table] = new Dictionary<int, Z3Level>();
            if (!_cache[table].ContainsKey(level))
                _cache[table][level] = readFromDB(table, level);
            return _cache[table][level];
        }

        private static Z3Level readFromDB(string table, int level) {
            Z3Level retval = null;
            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "select * from z3" + table + "levels where internalid=" + level;
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    if (r.Read())
                        retval = fromReader(table, r);
                    else
                        throw new RowNotInTableException();
                }
            }
            return retval;
        }

        public static string getName(string table, int level) {
            if (!_names.ContainsKey(table))
                _names[table] = new Dictionary<int, string>();
            if (!_names[table].ContainsKey(level))
                _names[table][level] = getType(table, level).Name;
            return _names[table][level];
        }

        public static void buildInitialTree(String table) {
            Dictionary<int, Z3Level> retval = new Dictionary<int, Z3Level>();
            Z3Level l;

            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "select * from z3" + table + "levels";
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                        retval.Add(Convert.ToInt32(r["internalid"]), l = Z3Level.fromReader(table, r));
                        l.Fields = getFields(table + "fields", l);
                    }
                }
                flatten(retval);
                _cache[table] = retval;
            }
        }

        private static List<Z3Field> getFields(String table, Z3Level l) {
            List<Z3Field> retval = new List<Z3Field>();
            Z3Field f;

            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "select * from z3" + table + " where level=" + l.ID;
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                        retval.Add(f = Z3Field.fromReader(r));
                        f.Level = l;
                    }
                }
                return retval;
            }
        }

        private int _id;
        private int _childid;
        private int _parentid;
        private string _name;
        private bool _root;
        private bool _final;
        private bool _measurable;

        private List<Z3Field> _fields;

        public bool Measurable {
            get { return _measurable; }
            set { _measurable = value; }
        }
        
        public List<Z3Field> Fields {
            get { return _fields; }
            set { _fields = value; }
        }

        public int ID {
            get { return _id; }
        }
        
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public bool Root {
            get { return _root; }
            set { _root = value; }
        }

        public bool Final {
            get { return _final; }
            set { _final = value; }
        }

        public Z3Level Child {
            get { return Z3Level.getType(_table, ChildID); }
        }

        public int ChildID {
            get { return _childid; }
            set { _childid = value; }
        }

        public int ParentID {
            get { return _parentid; }
            set { _parentid = value; }
        }
        
        public Z3Level Parent {
            get { return getType(_table, ParentID); }
        }

        private string _table;

        private Z3Level(string table) {
            _table = table;
        }

        public static Z3Level fromReader(string table, SqlCeDataReader r) {
            table = table.ToLowerInvariant();

            int i = Convert.ToInt32(r["internalid"]);
            if (!_cache.ContainsKey(table))
                _cache[table] = new Dictionary<int,Z3Level>();
            if (!_cache[table].ContainsKey(i))
                _cache[table][i] = new Z3Level(table);
            Z3Level l = _cache[table][i];

            l._id = i;
            l._name = r["name"].ToString();
            l._root = Convert.ToBoolean(r["root"]);
            l._childid = Convert.ToInt32(r["child"]);
            l._final = Convert.ToBoolean(r["final"]);
            l._measurable = Convert.ToBoolean(r["measurable"]);
            return l;
        }

        private static void flatten(Dictionary<int, Z3Level> d) {
            List<Z3Level> retval = new List<Z3Level>();
            Z3Level z = null;
            foreach (Z3Level zz in d.Values) {
                if (zz.Root) {
                    z = zz;
                    retval.Add(zz);
                    break;
                }
            }

            while (z != null && !z.Final) {
                retval.Add(z.Child);
                z.Child.ParentID = z.ID;
                z = z.Child;
            }
        }

        public override String ToString() {
            return _name;
        }
    }
}
