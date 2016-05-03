using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using System.Data;
using Z3.Workspace;

namespace Z3.Model {

    public interface HierarchicalEntity : IDisposable, State.Watchable {
        Dictionary<ZField, ZFieldValue> GetValues();
        void SetValues(Dictionary<ZField, ZFieldValue> values);
        void Delete();

        string Name { get; }
        int ID { get; }
        int ParentID { get; }
        int TypeID { get; set; }

        ZLevel Type { get; }

        Store<HierarchicalEntity> Store { get; }

        List<HierarchicalEntity> Children { get; }
        HierarchicalEntity Parent { get; }
    }

    public abstract class BasicHierarchicalEntity : HierarchicalEntity {
        public event EventHandler Modified;
        public event EventHandler Disposed;

        #region Fields
        private bool _disposed = false;
        private int _id = -1;
        private string _name;
        private int _parentid = -1;
        private Store<HierarchicalEntity> _store;
        private string _table;
        private int _type = -1;
        private WorkspaceInternals _workspace;
        private HierarchicalEntityFactory _factory;
        #endregion
        #region Constructor
        /// <summary>
        /// Constructs the core Entity
        /// </summary>
        /// <param name="suffix">The specific table suffix</param>
        internal BasicHierarchicalEntity(string suffix, WorkspaceInternals workspace, HierarchicalEntityFactory factory) {
            _table = suffix;
            _workspace = workspace;
            _workspace.Disposed += new EventHandler(_workspace_Disposed);
            _store = workspace.Store(suffix);
            _factory = factory;
        }
        #endregion
        #region Protected Members
        protected WorkspaceInternals Workspace
        {
            get
            {
                return _workspace;
            }
        }
        #endregion
        #region Public Members
        public List<HierarchicalEntity> Children
        {
            get
            {
                return _store.ChildrenOf(this);
            }
        }

        public HierarchicalEntity Parent
        {
            get
            {
                if (Type.Root) throw new InvalidOperationException("Root child has no parent");
                try
                {
                    return _store.ByID(Type.ParentID, ParentID);
                }
                catch (RowNotInTableException ex)
                {
                    throw new InconsistentWorkspaceException("Unable to find the parent of " + Name + GetDatabaseLocation() + ".  Error was " + ex.Message, ex);
                }
            }
        }

        public string GetDatabaseLocation()
        {
            return "[class=" + _table + ",level=" + _type + ",id=" + _id + "]";
        }

        public Store<HierarchicalEntity> Store
        {
            get
            {
                return _store;
            }
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("basicHierarchicalEntity");
            _disposed = true;

            if (Disposed != null)
                Disposed(this, new EventArgs());
        }

        /// <summary>
        /// The primary key
        /// </summary>
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == -1)
                {
                    _id = value;
                }
                else
                {
                    throw new InvalidOperationException("ID has already been set");
                }
            }
        }

        /// <summary>
        /// The primary key of the parent
        /// </summary>
        public int ParentID
        {
            get
            {
                return _parentid;
            }
            set
            {
                if (_parentid == -1)
                {
                    _parentid = value;
                }
                else
                {
                    throw new InvalidOperationException("Parent ID Has already been set");
                }
            }
        }

        /// <summary>
        /// The entity's type id
        /// </summary>
        public int TypeID
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type == -1)
                {
                    _type = value;
                }
                else
                {
                    throw new InvalidOperationException("Type id has already been set");
                }
            }
        }

        /// <summary>
        /// The Type of entity
        /// </summary>
        public ZLevel Type
        {
            get { return _workspace.Levels.getType(_table, _type); }
        }

        /// <summary>
        /// Name of the entity
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Saves the field values for this entity.
        /// </summary>
        /// <param name="values"></param>
        public void SetValues(Dictionary<ZField, ZFieldValue> fields)
        {
            foreach (ZFieldValue zf in fields.Values)
            {
                if (zf.Field.Name.Equals("Name"))
                {
                    _name = zf.ReadableValue.ToString();
                }
            }
            
            if (ID == -1)
            {
                _factory.SaveAsNew(this, fields);
            }
            else
            {
                _factory.Save(this, fields);
            }
            if (Modified != null)
                Modified(this, new EventArgs());
        }

        /// <summary>
        /// Deletes this entity.
        /// </summary>
        public void Delete()
        {
            _factory.Delete(this);
            Dispose();
        }

        public void loadFromReader(SqlCeDataReader r)
        {
            _id = Convert.ToInt32(r["internalid"]);
            _name = r["name"].ToString();
            _parentid = Convert.ToInt32(r["parent"]);
            readDelegate(r);
        }

        protected abstract void readDelegate(SqlCeDataReader r);
        

        /// <summary>
        /// Gets this entity's fields and values.
        /// </summary>
        /// <returns>Entity's value map</returns>
        public Dictionary<ZField, ZFieldValue> GetValues()
        {
            Dictionary<ZField, ZFieldValue> retval = new Dictionary<ZField, ZFieldValue>();
            using (SqlCeCommand cmd = _workspace.Connection.CreateCommand())
            {
                if (ID > -1)
                {
                    cmd.CommandText = "select * from " + Type.Name + _table + " where internalid=" + ID;
                }
                else
                {
                    bool first = true;
                    cmd.CommandText = "select ";
                    foreach (ZField f in Type.Fields)
                    {
                        if (!first) cmd.CommandText += ",";
                        first = false;
                        cmd.CommandText += f.Default + " as " + f.Name;
                    }
                }

                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        foreach (ZField f in Type.Fields)
                        {
                            retval[f] = new ZFieldValue(f, r[f.Name]);
                        }
                    }
                    else
                    {
                        throw new RowNotInTableException();
                    }
                }
            }
            valuesHook(retval);
            return retval;

        }
        protected virtual void valuesHook(Dictionary<ZField, ZFieldValue> hook) {

        }
        protected void wasModified()
        {
            if (Modified != null)
            {
                Modified(this, new EventArgs());
            }
        }
        #endregion
        #region Event Handlers
        void _workspace_Disposed(object sender, EventArgs e)
        {
            Dispose();
        }
        #endregion

        //protected abstract void SaveFieldsHook(Dictionary<ZField, ZFieldValue> fields);

        //protected abstract ZEntity fromFactoryReader(int type, SqlCeDataReader r);

        //protected ZEntity factorize(ZLevel level, ZEntity entity) {
        //    return factorize(level.ID, (entity!=null?entity.ID:0));
        //}

        //protected abstract ZEntity factorize(int level, int entity);
        
        //internal static ZEntity createNew(string _category, ZLevel z3Level, ZEntity z3Entity) {
        //    return _factories[_category.ToLowerInvariant()].factorize(z3Level, z3Entity);
        //}

        //internal static ZEntity createNew(string _category, int z3Level, int z3Entity) {
        //    return _factories[_category.ToLowerInvariant()].factorize(z3Level, z3Entity);
        //}

        //protected static Dictionary<string, ZEntity> _factories = new Dictionary<string,ZEntity>();

        public override bool Equals(object obj)
        {
            if (!(obj is BasicHierarchicalEntity)) return false;

            BasicHierarchicalEntity e = (BasicHierarchicalEntity)obj;
            return (e._type == _type && e._id == _id && e._table.Equals(_table));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
