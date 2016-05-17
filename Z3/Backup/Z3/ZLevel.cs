using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using Z3.Model;
using Z3.Workspace;

namespace Z3.Workspace
{
    public class LevelManager : IDisposable
    {
        //private WorkspaceInternals _ws;
        private SqlCeConnection _conn;
        private bool _disposed = false;
        
        private Dictionary<string, Dictionary<int, ZLevel>> _cache = new Dictionary<string, Dictionary<int, ZLevel>>();

        public LevelManager(SqlCeConnection conn)
        {
            _conn = conn;
            buildInitialTree("Countable");
            buildInitialTree("Container");
        }

        public LevelManager(WorkspaceInternals ws) : this(ws.Connection)
        {
        }

        public ICollection<ZLevel> Types(string table)
        {
            if (_disposed) throw new ObjectDisposedException("LevelManager");
            
            if (!_cache.ContainsKey(table))
                return new List<ZLevel>();
            return _cache[table].Values;
        }

        public ZLevel getType(string table, int level) {
            if (_disposed) throw new ObjectDisposedException("LevelManager");
            
            if (!_cache.ContainsKey(table))
                _cache[table] = new Dictionary<int, ZLevel>();
            if (!_cache[table].ContainsKey(level))
                _cache[table][level] = readFromDB(table, level);
            return _cache[table][level];
        }

        #region Helper Methods
        private ZLevel readFromDB(string table, int level) {
            ZLevel retval = null;
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
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

        //public string getName(string table, int level) {
        //    if (!_names.ContainsKey(table))
        //        _names[table] = new Dictionary<int, string>();
        //    if (!_names[table].ContainsKey(level))
        //        _names[table][level] = getType(table, level).Name;
        //    return _names[table][level];
        //}

        private Dictionary<string, ZLevel> _roots = new Dictionary<string, ZLevel>();
        public ZLevel getRootType(string table)
        {
            return _roots[table];
        }
        private void buildInitialTree(String table) {
            Dictionary<int, ZLevel> retval = new Dictionary<int, ZLevel>();
            ZLevel l;

            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "select * from Z3" + table + "Levels";
                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        retval.Add(Convert.ToInt32(r["internalid"]), l = fromReader(table, r));
                        l.Fields = getFields(table, l);
                    }
                }
                List<ZLevel> vals = flatten(retval);
                if (vals.Count > 0)
                {
                    _roots[table] = vals[0];
                }
                else
                {
                    _roots[table] = null;
                }
                _cache[table] = retval;
            }
        }

        private ZLevel fromReader(string table, SqlCeDataReader r)
        {
            int i = Convert.ToInt32(r["internalid"]);
            if (!_cache.ContainsKey(table))
                _cache[table] = new Dictionary<int, ZLevel>();
            if (!_cache[table].ContainsKey(i))
                _cache[table][i] = ZLevel.fromReader(table, r, this);
            
            return _cache[table][i];
        }

        private List<ZField> getFields(String table, ZLevel l) {
            List<ZField> retval = new List<ZField>();
            ZField f;

            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "select * from Z3" + table + "Fields where level=" + l.ID;
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                        retval.Add(f = ZField.fromReader(r));
                        f.Level = l;
                    }
                }
                return retval;
            }
        }

        private List<ZLevel> flatten(Dictionary<int, ZLevel> d)
        {
            List<ZLevel> retval = new List<ZLevel>();
            ZLevel z = null;
            foreach (ZLevel zz in d.Values)
            {
                if (zz.Root)
                {
                    z = zz;
                    retval.Add(zz);
                    break;
                }
            }

            while (z != null && !z.Final)
            {
                retval.Add(z.Child);
                z.Child.ParentID = z.ID;
                z = z.Child;
            }

            return retval;
        }
        #endregion

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("LevelManager");

            _disposed = true;
            _conn = null;
            _cache.Clear();
        }
    }
}

namespace Z3.Model {
    public class ZLevel {
        
        public string Table
        {
            get
            {
                return _name + _table;
            }
        }

        private int _id;
        private int _childid;
        private int _parentid;
        private string _name;
        private bool _root;
        private bool _final;
        private bool _measurable;
        private LevelManager _mgr;

        private List<ZField> _fields;

        public bool Measurable {
            get { return _measurable; }
            set { _measurable = value; }
        }
        
        public List<ZField> Fields {
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

        public ZLevel Child {
            get {
                if (_final) return null;
                return _mgr.getType(_table, ChildID);
            }
        }

        public int ChildID {
            get { return _childid; }
            set { _childid = value; }
        }

        public int ParentID {
            get { return _parentid; }
            set { _parentid = value; }
        }
        
        public ZLevel Parent {
            get { return _mgr.getType(_table, ParentID); }
        }

        private string _table;

        private ZLevel(string table, LevelManager mgr) {
            _table = table;
            _mgr = mgr;
        }
        
        public override String ToString() {
            return _name;
        }

        public static ZLevel fromReader(string table, SqlCeDataReader r, LevelManager mgr)
        {
            ZLevel l = new ZLevel(table, mgr);

            l._id = Convert.ToInt32(r["internalid"]);
            l._name = r["name"].ToString();
            l._root = Convert.ToBoolean(r["root"]);
            l._childid = Convert.ToInt32(r["child"]);
            l._final = Convert.ToBoolean(r["final"]);
            l._measurable = Convert.ToBoolean(r["measurable"]);

            return l;
        }
    }
}
