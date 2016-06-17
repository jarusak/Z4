using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
//using Z3.Leger;
using Z3.Model;
using Z3.Workspace;
using System.Diagnostics;

namespace Z3.Workspace
{
    public class LevelManager : IDisposable
    {
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

        // caches table and/or target ZLevel object and returns it

        public ZLevel getType(string table, int level) {
            if (_disposed) throw new ObjectDisposedException("LevelManager");
            
            if (!_cache.ContainsKey(table))
                _cache[table] = new Dictionary<int, ZLevel>();
            if (!_cache[table].ContainsKey(level))
                _cache[table][level] = readFromDB(table, level);
            return _cache[table][level];
        }

        #region Helper Methods

        // searches for ZLevel with target ID in database 

        private ZLevel readFromDB(string table, int level) {
            ZLevel retval = null;
            using (SqlCeCommand cmd = _conn.CreateCommand())
            { 
                cmd.CommandText = "select * from Z3" + table + "Levels where internalid=" + level;
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    if (r.Read())
                        retval = fromReader(table, r);
                    else
                    {
                        throw new RowNotInTableException();
                    }     
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
            try
            {
                return _roots[table];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public void forceReload()
        {
            buildInitialTree("Countable");
            buildInitialTree("Container");
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
                        // Adds all the ZLevels and IDs to retval from a table
                        retval.Add(Convert.ToInt32(r["internalid"]), l = fromReader(table, r));
                        // grabs all the feilds and assignes them to the ZLevel
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

        // caches table and/or ZLevel and returns ZLevel object

        private ZLevel fromReader(string table, SqlCeDataReader r)
        {
            int i = Convert.ToInt32(r["internalid"]);

            if (!_cache.ContainsKey(table))
                _cache[table] = new Dictionary<int, ZLevel>();
            if (!_cache[table].ContainsKey(i))
                _cache[table][i] = ZLevel.fromReader(table, r, this);
            
            return _cache[table][i];
        }

        public void Save(ZLevel l, string table)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "update Z3" + table + "Levels set name=?, measurable=?, child=?, final=?, root=? where internalid=?";
                cmd.Parameters.AddWithValue("@name", l.Name);
                cmd.Parameters.AddWithValue("@meas", l.Measurable);
                cmd.Parameters.AddWithValue("@child", l.ChildID);
                cmd.Parameters.AddWithValue("@final", l.Final);
                cmd.Parameters.AddWithValue("@root", l.Root);
                cmd.Parameters.AddWithValue("@id", l.ID);

                cmd.ExecuteNonQuery();
            }
        }

        // gets all the feilds like name, mesh size, gear, lake etc

        private List<ZField> getFields(String table, ZLevel l) {
            List<ZField> retval = new List<ZField>();
            ZField f = new ZField();
            
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "select * from Z3" + table + "Fields where level=" + l.ID;
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                       f = ZField.fromReader(r);
                       if (f.Type.Equals("combobox"))
                        {
                            getComboBox(f.Name, f);
                        }
                       retval.Add(f);
                       f.Level = l;
                    }
                }

                return retval;
            }
        }

        private void getComboBox (String table, ZField f)
        {
            List<String> comboBoxVals = new List<String>();

            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "select * from " + table;
                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        comboBoxVals.Add(r["name"].ToString());
                        Debug.WriteLine("RIGHT HERE: " + r["name"].ToString());
                    }
                }
            }

            f.AllComboVals = comboBoxVals;
        }

        // returns a List containing ZLevels that can have children

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
                z.Child.ParentID = z.ID; // why?
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

        public void Create(string table)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "insert into Z3" + table + "Levels (name, root, child, final, measurable) values('NewItem', ?, 0, 1, 1)";
                ZLevel l = getFinalLevel(table);
                cmd.Parameters.AddWithValue("@root", (l == null) ? 1 : 0);
                cmd.ExecuteNonQuery();

                int level = WorkspaceInternals.getIdentity(_conn);

                if (l != null)
                {
                    l.ChildID = level;
                    l.Final = false;
                    l.Save();
                }

                cmd.CommandText = "insert into Z3" + table + "Fields (name, label, \"default\", type, level) values('Name', 'Name', '''New Item''', 'nvarchar(100)', " + level + ")";
                cmd.ExecuteNonQuery();
            }

            forceReload();
        }

        private ZLevel getFinalLevel(string table)
        {
            ZLevel i = getRootType(table);
            if (i == null) return null;
            while (!i.Final)
            {
                i = i.Child;
            }
            return i;
        }

        public void SaveField(ZLevel zLevel, string _table, ZField zf)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "update Z3" + _table + "Fields set name=?, label=?, \"default\"=?, type=? where internalid=?";
                cmd.Parameters.AddWithValue("@name", zf.Name);
                cmd.Parameters.AddWithValue("@label", zf.Label);
                cmd.Parameters.AddWithValue("@def", zf.Default);
                cmd.Parameters.AddWithValue("@type", zf.Type);
                cmd.Parameters.AddWithValue("@iid", zf.Id);
                cmd.ExecuteNonQuery();

                if (zf.Type.Equals("combobox"))
                {
                    if (!Factory.tableExists(_conn, zf.Name))
                    {
                        cmd.CommandText = "create table " + zf.Name + " (name nvarchar(100) not null default 'something')";
                        cmd.ExecuteNonQuery();
                    }
                    if (zf.AddedComboVals != null)
                    {
                        cmd.CommandText = "insert into " + zf.Name + " (name) values(@param)";
                        foreach (String param in zf.AddedComboVals)
                        {
                            cmd.Parameters.AddWithValue("@param", param);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        
                        zf.AddedComboVals.Clear();
                    }                  
                }
            }
        }

        public void Delete(ZLevel zLevel, string table)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {   
                cmd.CommandText = "delete from Z3" + table + "Levels where internalid=?";
                cmd.Parameters.AddWithValue("@id", zLevel.ID);
                cmd.ExecuteNonQuery();

                foreach (ZField f in zLevel.Fields)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "delete from Z3" + table + "Fields where level=?";
                    cmd.Parameters.AddWithValue("@id", zLevel.ID);
                    cmd.ExecuteNonQuery();    
                }
            }

            forceReload();
        }

        public void DeleteField(string _table, ZField field)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "delete from Z3" + _table + "Fields where internalid=" + field.Id;
                cmd.ExecuteNonQuery();

                if (field.Type.Equals("combobox"))
                {
                    cmd.CommandText = "drop table " + field.Name;
                    cmd.ExecuteNonQuery();
                }
            }

            forceReload();
        }

        public void DeleteComboBoxValue(String table, String name)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "delete from " + table + " where name=@name";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            }
        }
        public void AddField(ZLevel zLevel, string _table, string p)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "insert into Z3" + _table + "Fields (name, level) values(?, ?)";
                cmd.Parameters.AddWithValue("@name", p);
                cmd.Parameters.AddWithValue("@lvl", zLevel.ID);
                cmd.ExecuteNonQuery();
            }
            forceReload();
        }
    }
}

namespace Z3.Model {

    // ZLevel are the objects under the tabs Countable and Containers
    // ie. Samples, Subsample, Genus, Species etc

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

        // Objects like Sample or Genus
        // No dependencies

        public bool Root {
            get { return _root; }
            set { _root = value; }
        }

        // objects that can have no children
        // ie subsamples

        public bool Final {
            get { return _final; }
            set { _final = value; }
        }

        // returns the Child

        public ZLevel Child {
            get
            {
                try
                {
                    return _mgr.getType(_table, ChildID);
                }
                catch (RowNotInTableException)
                {
                    return null;
                }
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

        // intializes a ZLevel

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

        public void Save()
        {
            _mgr.Save(this, _table);
        }

        public void SaveField(ZField zf)
        {
            _mgr.SaveField(this, _table, zf);
        }

        public void Delete()
        {
            if (this.Root)
            {
                if (!this.Final)
                {
                    this.Child.Root = true;
                    this.Child.ParentID = 0;
                    this.Child.Save();
                }
            }
            else if (this.Final)
            {
                this.Parent.Final = true;
                this.Parent.ChildID = 0;
                this.Parent.Save();
            }
            else
            {
                this.Parent.ChildID = this.ChildID;
                this.Child.ParentID = this.ParentID;
                this.Parent.Save();
                this.Child.Save();
            }

            _mgr.Delete(this, _table);
        }

        public void AddField(string p)
        {
            _mgr.AddField(this, _table, p);
        }

        public void DeleteField(ZField f)
        {
            _mgr.DeleteField(_table, f);
        }

        public void DeleteComboBoxValue(String table, String name)
        {
            _mgr.DeleteComboBoxValue(table, name);
        }
    }
}
