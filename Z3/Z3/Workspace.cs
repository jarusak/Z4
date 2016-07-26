using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

using Z3.Model;
using System.IO;
using System.Text;
using Z3.Util;
using System.Diagnostics;

namespace Z3.Workspace
{
    /// <summary>
    /// Provides an interface to access the Workspace data.
    /// </summary>
    public interface IWorkspace : Z3.State.Watchable
    {
        /// <summary>
        /// The repository of all Data Sets.
        /// </summary>
        Store<ZDataSet> DataSetStore { get; }

        /// <summary>
        /// The repository of all Countables.
        /// </summary>
        Store<ZCountable> CountableStore { get; }

        /// <summary>
        /// The repository of all Report Types.
        /// </summary>
        ReportTypeManager ReportTypes { get; }

        /// <summary>
        /// Both Hierarchical stores.
        /// </summary>
        /// <param name="Hierarchy">Can be Container or Countable</param>
        /// <returns>The appropriate store from the Workspace.</returns>
        Store<HierarchicalEntity> Store(string Hierarchy);
        
        /// <summary>
        /// The name of the current Schema.
        /// </summary>
        string Schema { get; }

        /// <summary>
        /// The filename of the current workspace.
        /// </summary>
        string Filename { get; }
        
        /// <summary>
        /// The short filename (without path) of the current workspace.
        /// </summary>
        string ShortFileName { get; }

        /// <summary>
        /// Gets a list of all measurement types.
        /// </summary>
        /// <returns></returns>
        List<ZMeasurement> MeasurementTypes { get; }

        /// <summary>
        /// Gets a certain measurement type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ZMeasurement getMeasurementType(int id);

        /// <summary>
        /// The repository for all Levels.
        /// </summary>
        LevelManager Levels { get; }

        /// <summary>
        /// The repository for all Data Points.
        /// </summary>
        DataPointManager DataPoints { get; }

        new event EventHandler Modified;
        new event EventHandler Disposed;

    }

    /// <summary>
    /// Provides full access to the database functionality, for the Model
    /// and Workspace classes only.
    /// </summary>
    public class WorkspaceInternals : IWorkspace
    {
        private string _fn;
        private SqlCeConnection _connection;
        private IndividualManager _indivs;
        private LevelManager _levels;
        private DataPointManager _datapts;
        private List<ZMeasurement> _mtypes;
        private Store<ZDataSet> _zstore_ds;
        private Store<ZCountable> _zstore_zc;
        private ReportTypeManager _rtypes;
        private bool _disposed = false;
        private Dictionary<string, string> _properties;

        public WorkspaceInternals(string filename, SqlCeConnection conn)
        {
            _connection = conn;
            _fn = filename;
            _properties = getProperties();

            _indivs = new IndividualManager(this);
            _levels = new LevelManager(this);
            _datapts = new DataPointManager(this);
            _zstore_ds = new StoreImpl<ZDataSet>(new ContainerFactory(this), this);
            _zstore_zc = new StoreImpl<ZCountable>(new CountableFactory(this), this);
            _rtypes = new ReportTypeManager(this);
        }

        #region Properties
        public SqlCeConnection Connection
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _connection;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                _connection = value;
            }
        }
        public SqlCeCommand CreateCommand()
        {
            if (_disposed) throw new ObjectDisposedException("workspace");
            return _connection.CreateCommand();
        }
        public IndividualManager Individuals
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _indivs;
            }
        }
        public int getIdentity()
        {
            if (_disposed) throw new ObjectDisposedException("workspace");
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "select @@IDENTITY";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        #endregion

        #region IWorkspace Members
        public DataPointManager DataPoints
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _datapts;
            }
        }
        public List<ZMeasurement> MeasurementTypes
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                if (_mtypes == null)
                {
                    _mtypes = new List<ZMeasurement>();
                    if (Connection == null) return null;

                    using (SqlCeCommand cmd = Connection.CreateCommand())
                    {
                        cmd.CommandText = "select * from z3measurementtypes";
                        using (SqlCeDataReader r = cmd.ExecuteReader())
                            while (r.Read())
                                _mtypes.Add(ZMeasurement.fromReader(r, this));
                    }
                }

                return _mtypes;
            }
        }
        public ZMeasurement getMeasurementType(int id)
        {
            if (_disposed) throw new ObjectDisposedException("workspace");
            foreach (ZMeasurement z in MeasurementTypes)
            {
                if (z.ID == id) return z;
            }

            throw new RowNotInTableException("Measurement type " + id + " not found.");
        }
        public Store<ZDataSet> DataSetStore
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _zstore_ds;
            }
        }
        public Store<ZCountable> CountableStore
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _zstore_zc;
            }
        }
        public Store<HierarchicalEntity> Store(string Hierarchy)
        {
            if (_disposed) throw new ObjectDisposedException("workspace");
            if (Hierarchy[2] == 'u') return CountableStore.Ungenericized;
            else return DataSetStore.Ungenericized;
        }
        public LevelManager Levels
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _levels;
            }
        }
        public ZLevel GetRootLevel(string table)
        {
            return _levels.getRootType(table);
        }
        public string Schema
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _properties["Name"];
            }
        }
        public string Filename
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _fn;
            }
        }
        public String ShortFileName
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                if (Filename == null) return "<No Workspace>";
                if (!Filename.Contains("\\"))
                    return Filename;
                return Filename.Substring(Filename.LastIndexOf("\\") + 1);
            }
        }
        public ReportTypeManager ReportTypes
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                return _rtypes;
            }
        }
        #endregion

        #region Privates
        private Dictionary<string, string> getProperties()
        {
            Dictionary<string, string> retval = new Dictionary<string, string>();

            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "select * from z3datasource";
                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        retval.Add(r["name"].ToString(), r["value"].ToString());
                    }
                }
                return retval;
            }
        }
        #endregion
        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("workspace");
            
            _disposed = true;
            _indivs.Dispose();
            _levels.Dispose();
            _datapts.Dispose();
            foreach (ZMeasurement m in _mtypes)
            {
                m.Dispose();
            }
            _zstore_ds.Dispose();
            _zstore_zc.Dispose();
            _rtypes.Dispose();
            _connection.Close();
            Disposed(this, new EventArgs());
        }
        event EventHandler IWorkspace.Modified { add { } remove { } }
        event EventHandler Z3.State.Watchable.Modified { add { } remove { } }
        public event EventHandler Disposed;
    }

    #region Queries
    public interface LookupQuery<T> : IDisposable where T : HierarchicalEntity
    {
        SqlCeDataReader Execute();
        int Level { get; }
    }

    internal abstract class BasicLookupQuery<T> : LookupQuery<T> where T : HierarchicalEntity
    {
        protected SqlCeCommand cmd;
        protected int level;

        protected BasicLookupQuery(WorkspaceInternals ws, int level)
        {
            cmd = ws.Connection.CreateCommand();
            this.level = level;
        }

        public int Level
        {
            get
            {
                return level;
            }
        }

        public SqlCeDataReader Execute()
        {
            return cmd.ExecuteReader();
        }

        public void Dispose()
        {
            cmd.Dispose();
        }
    }

    internal class LookupChildrenQuery<T> : BasicLookupQuery<T> where T : HierarchicalEntity
    {
        public LookupChildrenQuery(WorkspaceInternals ws, T parent)
            : base(ws, parent.Type.ChildID)
        {
            if (parent.Type.Final) throw new NotSupportedException("No children on final type");
            cmd.CommandText = "select * from " + parent.Type.Child.Table + " where parent=?";
            cmd.Parameters.AddWithValue("@parent", parent.ID);
        }
    }

    internal class ByLevelQuery<T> : BasicLookupQuery<T> where T : HierarchicalEntity
    {
        public ByLevelQuery(WorkspaceInternals ws, ZLevel level)
            : base(ws, level.ID)
        {
            cmd.CommandText = "select * from " + level.Table;
        }
    }
    #endregion

    #region Hierarchy Stores
    public interface Store<T> where T : HierarchicalEntity
    {
        //List<T> ByQuery(LookupQuery<T> q);
        //List<T> ByQuery(SqlCeDataReader r);
        List<T> All { get; }
        List<T> ChildrenOf(T t);
        List<T> AllFromLevel(ZLevel l);

        T FromReader(int level, SqlCeDataReader r);
        T Create(T parent);
        T ByID(int level, int id);

        Store<HierarchicalEntity> Ungenericized { get; }

        event EventHandler<ObjectEventArgs<T>> ItemAdded;
        event EventHandler<ObjectEventArgs<T>> ItemEdited;
        event EventHandler<ObjectEventArgs<T>> ItemDeleted;

        void Dispose();
    }

    

    public class StoreWrapper<T2> : Store<HierarchicalEntity> where T2 : HierarchicalEntity
    {
        public Store<T2> BaseStore;
        private bool _disposed = false;

        public StoreWrapper(Store<T2> wrapped)
        {
            BaseStore = wrapped;
            BaseStore.ItemAdded += new EventHandler<ObjectEventArgs<T2>>(BaseStore_ItemAdded);
            BaseStore.ItemEdited += new EventHandler<ObjectEventArgs<T2>>(BaseStore_ItemEdited);
            BaseStore.ItemDeleted += new EventHandler<ObjectEventArgs<T2>>(BaseStore_ItemDeleted);
        }

        void BaseStore_ItemDeleted(object sender, ObjectEventArgs<T2> e)
        {
            if (ItemDeleted != null)
                ItemDeleted(sender, new ObjectEventArgs<HierarchicalEntity>(e.Value));
        }

        void BaseStore_ItemEdited(object sender, ObjectEventArgs<T2> e)
        {
            if (ItemEdited != null)
                ItemEdited(sender, new ObjectEventArgs<HierarchicalEntity>(e.Value));
        }

        void BaseStore_ItemAdded(object sender, ObjectEventArgs<T2> e)
        {
            if (ItemAdded != null)
                ItemAdded(sender, new ObjectEventArgs<HierarchicalEntity>(e.Value));
        }

        public List<HierarchicalEntity> All
        {
            get
            {
                List<HierarchicalEntity> retval = new List<HierarchicalEntity>();
                foreach (T2 t in BaseStore.All)
                {
                    retval.Add(t);
                }
                return retval;
            }
        }

        public Store<HierarchicalEntity> Ungenericized
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("StoreWrapper");

                return this;
            }
        }

        private List<HierarchicalEntity> wraplist(List<T2> oldlist)
        {
            List<HierarchicalEntity> retval = new List<HierarchicalEntity>();
            foreach (T2 h in oldlist)
            {
                retval.Add(h);
            }
            return retval;
        }

        #region Store<HierarchicalEntity> Members

        public List<HierarchicalEntity> ChildrenOf(HierarchicalEntity t)
        {
            if (_disposed) throw new ObjectDisposedException("StoreWrapper");

            return wraplist(BaseStore.ChildrenOf((T2)t));
        }

        public List<HierarchicalEntity> AllFromLevel(ZLevel l)
        {
            if (_disposed) throw new ObjectDisposedException("StoreWrapper");

            return wraplist(BaseStore.AllFromLevel(l));
        }

        public HierarchicalEntity FromReader(int l, SqlCeDataReader r)
        {
            if (_disposed) throw new ObjectDisposedException("StoreWrapper");

            return BaseStore.FromReader(l,r);
        }

        public HierarchicalEntity Create(HierarchicalEntity parent)
        {
            if (_disposed) throw new ObjectDisposedException("StoreWrapper");

            return BaseStore.Create((T2)parent);
        }

        public HierarchicalEntity ByID(int level, int id)
        {
            if (_disposed) throw new ObjectDisposedException("StoreWrapper");

            return BaseStore.ByID(level, id);
        }

        public event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemAdded;
        public event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemEdited;
        public event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemDeleted;

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("StoreWrapper");
            _disposed = true;

            BaseStore = null;
        }

        #endregion
    }

    public class StoreImpl<T> : Store<T> where T : HierarchicalEntity
    {
        public event EventHandler<ObjectEventArgs<T>> ItemAdded;
        public event EventHandler<ObjectEventArgs<T>> ItemEdited;
        public event EventHandler<ObjectEventArgs<T>> ItemDeleted;

        private StoreWrapper<T> _ungen;
        private List<T> _cachelist = new List<T>();
        private Dictionary<int, Dictionary<int, T>> _dualmap = new Dictionary<int, Dictionary<int, T>>();
        private HierarchicalEntityFactory _factory;
        private WorkspaceInternals _ws;
        private bool _disposed = false;

        internal StoreImpl(HierarchicalEntityFactory factory, WorkspaceInternals ws)
        {
            _factory = factory;
            _factory.ItemAdded += new EventHandler<ObjectEventArgs<HierarchicalEntity>>(_factory_ItemAdded);
            _factory.ItemDeleted += new EventHandler<ObjectEventArgs<HierarchicalEntity>>(_factory_ItemDeleted);
            _factory.ItemEdited += new EventHandler<ObjectEventArgs<HierarchicalEntity>>(_factory_ItemEdited);
            _ws = ws;
            _ws.Disposed += new EventHandler(_ws_Disposed);
            _ungen = new StoreWrapper<T>(this);
        }

        void _ws_Disposed(object sender, EventArgs e)
        {
            Dispose();
        }

        void _factory_ItemEdited(object sender, ObjectEventArgs<HierarchicalEntity> e)
        {
            ItemEdited(sender, new ObjectEventArgs<T>((T)e.Value));
        }

        void _factory_ItemDeleted(object sender, ObjectEventArgs<HierarchicalEntity> e)
        {
            ItemDeleted(sender, new ObjectEventArgs<T>((T)e.Value));
        }

        void _factory_ItemAdded(object sender, ObjectEventArgs<HierarchicalEntity> e)
        {
            ItemAdded(sender, new ObjectEventArgs<T>((T)e.Value));
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            _disposed = true;

            _ungen.Dispose();
            _ungen = null;
            _cachelist.Clear();
            _cachelist = null;
            _dualmap.Clear();
            _dualmap = null;
            _factory.Dispose();
            _factory = null;
            _ws = null;
        }

        public bool Disposed
        {
            get { return _disposed; }
        }

        public Store<HierarchicalEntity> Ungenericized
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("StoreImpl");

                return _ungen;
            }
        }

        public List<T> All
        {
            get
            {
                List<T> retval = new List<T>();
                ZLevel l = _factory.RootLevel;
                while (l != null)
                {
                    retval.AddRange(this.AllFromLevel(l));
                    
                    if (l.Final) l = null;
                    else l = l.Child;
                } 
                return retval;
            }
        }

        public List<T> ChildrenOf(T t)
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            return ByQuery(new LookupChildrenQuery<T>(_ws, t));
        }

        public List<T> AllFromLevel(ZLevel l)
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            return ByQuery(new ByLevelQuery<T>(_ws, l));
        }

        List<T> ByQuery(LookupQuery<T> q)
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            return (ByQuery(q.Level, q.Execute()));
        }

        List<T> ByQuery(int level, SqlCeDataReader r)
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            List<T> retval = new List<T>();
            while (r.Read())
            {
                retval.Add(FromReader(level, r));
            }
            return retval;
        }

        public T FromReader(int level, SqlCeDataReader r)
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            int id = Convert.ToInt32(r["internalid"]);

            return (cachelookup(level, id, r));
        }

        /// <summary>
        /// Find item in cache; if not present, create a new one.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected T cachelookup(int level, int id, SqlCeDataReader r)
        {
            Dictionary<int, T> levelmap = getCache(level);
            if (levelmap.ContainsKey(id))
            {
                return levelmap[id];
            }
            else
            {
                return (T)_factory.Read(r, _factory.Construct(), level);
            }
        }

        /// <summary>
        /// Find or create the first level of cache.
        /// </summary>
        /// <param name="level">The int level id</param>
        /// <returns>The object cache for that level</returns>
        protected Dictionary<int, T> getCache(int level)
        {
            if (_dualmap.ContainsKey(level))
            {
                return _dualmap[level];
            }
            else
            {
                Dictionary<int, T> newdict = new Dictionary<int, T>();
                _dualmap[level] = newdict;
                return newdict;
            }
        }

        public T Create(T parent)
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            return (T)_factory.New(parent);
        }

        public T ByID(int level, int id)
        {
            if (_disposed) throw new ObjectDisposedException("StoreImpl");

            Dictionary<int, T> lvl = getCache(level);
            if (lvl.ContainsKey(id))
            {
                return lvl[id];
            }
            else
            {
                return (T)_factory.ByID(level, id);
            }
        }
    }

    public class InconsistentWorkspaceException : System.Exception
    {
        public InconsistentWorkspaceException()
            : base()
        {
        }

        public InconsistentWorkspaceException(string message)
            : base(message)
        {
        }

        public InconsistentWorkspaceException(string message, Exception other)
            : base(message, other)
        {
        }
    }

    //internal interface HierarchicalEntityFactory : IDisposable
    //{
    //    
    //    event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemAdded;
    //    event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemEdited;
    //    event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemDeleted;
    //
    //    ZLevel RootLevel { get; }
    //    HierarchicalEntity Construct();
    //    HierarchicalEntity Read(SqlCeDataReader r, HierarchicalEntity value, int level);
    //    HierarchicalEntity New(HierarchicalEntity parent);
    //    HierarchicalEntity New(int level, int parentid);
    //    HierarchicalEntity ByID(int level, int id);
    //    void Delete(HierarchicalEntity item);
    //    void SaveAsNew(HierarchicalEntity item, Dictionary<ZField, ZFieldValue> values);
    //    void Save(HierarchicalEntity item, Dictionary<ZField, ZFieldValue> values);
    //}

    internal abstract class HierarchicalEntityFactory
    {

        #region Hooks
        
        /// <summary>
        /// This should construct an editable instance of the object.
        /// </summary>
        /// <returns></returns>
        public abstract BasicHierarchicalEntity Construct();

        #endregion

        #region Factory Interface
        public HierarchicalEntity New(int level, int parentid)
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            BasicHierarchicalEntity retval = Construct();
            retval.ParentID = parentid;
            retval.TypeID = level;
            return retval;
        }

        public HierarchicalEntity New(HierarchicalEntity parent)
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            if (parent == null)
            {
                return New(_ws.GetRootLevel(_table).ID, 0);
            }

            if (parent.Type.Final)
            {
                throw new InvalidOperationException("Cannot create a child of a leaf object");
            }
            
            return New(parent.Type.Child.ID, parent.ID);
        }

        /// <summary>
        /// Saves an object to the database for the first time (using an INSERT).
        /// </summary>
        /// <param name="value"></param>
        public void SaveAsNew(BasicHierarchicalEntity value, Dictionary<ZField, ZFieldValue> values)
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            //protected void getNew(int level, int parent, string fields, object[] obs) {
            using (SqlCeCommand cmd = Workspace.Connection.CreateCommand())
            {
                string fields = "", sqlvalues = "";
                cmd.Parameters.AddWithValue("@parent", value.ParentID);
                foreach (ZFieldValue v in values.Values)
                {
                    fields += "," + v.Field.Name;
                    sqlvalues += ",?";
                    cmd.Parameters.AddWithValue(v.Field.Name, v.DatabaseValue);
                }
                cmd.CommandText = "insert into " + value.Type.Table + " (parent" + fields + ") values(?" + sqlvalues + ")";

                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException("Could not create a new record.");
                }
            }

            value.ID = _ws.getIdentity();
            ItemAdded(this, new ObjectEventArgs<HierarchicalEntity>(value));
        }

        public event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemAdded;
        public event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemEdited;
        public event EventHandler<ObjectEventArgs<HierarchicalEntity>> ItemDeleted;

        public void Save(HierarchicalEntity value, Dictionary<ZField, ZFieldValue> values)
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            bool first = true;
            using (SqlCeCommand cmd = Workspace.Connection.CreateCommand())
            {
                cmd.CommandText = "update " + value.Type.Name + Hierarchy + " set ";
                foreach (ZFieldValue v in values.Values)
                {
                    if (!first)
                        cmd.CommandText += ", ";
                    else first = false;
                    cmd.CommandText += v.Field.Name + "=?";
                    cmd.Parameters.AddWithValue(v.Field.Name, v.DatabaseValue);
                }
                cmd.CommandText += " where internalid=" + value.ID;
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Failed to update the record.");
            }
            ItemEdited(this, new ObjectEventArgs<HierarchicalEntity>(value));
        }

        /// <summary>
        /// Reads an object from an SqlCeDataReader
        /// </summary>
        /// <param name="r"></param>
        /// <param name="retval"></param>
        /// <returns></returns>
        public HierarchicalEntity Read(SqlCeDataReader r, BasicHierarchicalEntity retval, int level)
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            retval.TypeID = level;
            retval.loadFromReader(r);
            
            return retval;
        }

        /// <summary>
        /// Loads an existing object from the db.
        /// </summary>
        /// <param name="level">Level #</param>
        /// <param name="id">ID #</param>
        public HierarchicalEntity ByID(int level, int id)
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            BasicHierarchicalEntity retval = Construct();

            using (SqlCeCommand cmd = Workspace.Connection.CreateCommand())
            {

                cmd.CommandText = "select * from " + tablename(level) + " where internalid=?";
                cmd.Parameters.AddWithValue("@iid", id);
                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return Read(r, retval, level);
                    }
                    else
                    {
                        throw new RowNotInTableException("Entity [class=" + _table + ",level=" + level + ",id=" + id + "] not found in table " + tablename(level) + ". ");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the object and its children.
        /// </summary>
        /// <param name="item"></param>
        public void Delete(HierarchicalEntity item)
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            if (item.ID == -1) return;

            if (findReferences(item))
            {
                throw new OperationCanceledException("This item has dependencies elsewhere in the workspace.");
            }

            doDeleteRecursive(item);
            ItemDeleted(this, new ObjectEventArgs<HierarchicalEntity>(item));
        }

        public ZLevel RootLevel
        {
            get
            {
                return _ws.Levels.getRootType(_table);
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// The current Workspace.
        /// </summary>
        protected WorkspaceInternals Workspace
        {
            get
            {
                return _ws;
            }
        }

        /// <summary>
        /// Which object hierarchy we are talking about: Countable or Container.
        /// </summary>
        protected string Hierarchy
        {
            get
            {
                return _table;
            }
        }


        #endregion

        #region Private Helpers
        private string tablename(int level)
        {
            return (Workspace.Levels.getType(Hierarchy, level).Name + Hierarchy);
        }

        private bool findReferences(HierarchicalEntity h)
        {
            if (individualReferences(h)) return true;
            if (!h.Type.Final)
            foreach (HierarchicalEntity ch in h.Children)
            {
                if (findReferences(ch)) return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if any Z3Individuals reference this entity.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private bool individualReferences(HierarchicalEntity h)
        {
            int count;

            // Find any data points that reference this child
            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "select count(*) from Z3Individuals where " + Hierarchy + "level=? and " + Hierarchy + "=?";
                cmd.Parameters.AddWithValue("@level", h.TypeID);
                cmd.Parameters.AddWithValue("@id", h.ID);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return (count > 0);
        }

        private void doDeleteRecursive(HierarchicalEntity h)
        {
            if (!h.Type.Final)
            foreach (HierarchicalEntity ch in h.Children)
            {
                doDeleteRecursive(ch);
            }
            doDeleteSingle(h);
        }

        private void doDeleteSingle(HierarchicalEntity h)
        {
            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "delete from " + h.Type.Table + " where internalid=?";
                cmd.Parameters.AddWithValue("@id", h.ID);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Fields
        private WorkspaceInternals _ws;
        private string _table;
        private bool _disposed = false;

        /// <summary>
        /// Instantiates a basic entity factory.  All you need is a workspace link.
        /// </summary>
        /// <param name="ws"></param>
        protected HierarchicalEntityFactory(WorkspaceInternals ws, string hierarchy)
        {
            _ws = ws;
            _table = hierarchy;
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("BasicHEFactory");

            _ws = null;
        }

        public bool Disposed
        {
            get
            {
                return _disposed;
            }
        }
        #endregion
    }

    internal class CountableFactory : HierarchicalEntityFactory
    {
        public CountableFactory(WorkspaceInternals ws)
            : base(ws, "Countable")
        {
        }

        public override BasicHierarchicalEntity Construct()
        {
            return new ZCountable(this.Workspace, this);
        }
    }

    internal class ContainerFactory : HierarchicalEntityFactory
    {
        public ContainerFactory(WorkspaceInternals ws)
            : base(ws, "Container")
        {

        }

        public override BasicHierarchicalEntity Construct()
        {
            return new ZDataSet(this.Workspace, this);
        }
    }

    //internal class CountableFactory : HierarchicalEntityFactory<ZCountable>
    //{

    //}
    #endregion

    public static class Factory
    {
        private const string CREATE_INDIVIDUAL_TABLE = "CREATE TABLE Z3Individuals (internalid int identity, Container int not null, Countable int not null, ContainerLevel int not null, CountableLevel int not null, Comments ntext not null default(''));";
        private const string CREATE_MEASUREMENT_TABLE = "CREATE TABLE Z3Measurements (internalid int identity, Individual int not null, Measurement int not null, Value float not null, Weight float not null default 0)";

        private const string UPG_V2_CREATE_REPORT_TABLE = "CREATE TABLE Z3Reports(internalid int IDENTITY PRIMARY KEY, name nvarchar(100) NOT NULL DEFAULT 'New Query', query ntext NOT NULL DEFAULT 'select 1');";

        private const string UPG_V3_MODIFY_MTYPES_TABLE1 = "ALTER TABLE Z3MeasurementTypes ADD increment bit not null default 0, displayed bit not null default 1, counted bit not null default 0, counttype int not null default 1, autotype int not null default 1;";
        private const string UPG_V3_MODIFY_MTYPES_TABLE2 = "UPDATE Z3MeasurementTypes SET autocount = 1, increment = 1, displayed = 1, counted = 1, counttype = 3, autotype = 1 WHERE (name = 'Egg Count')";
        private const string UPG_V3_MODIFY_MTYPES_TABLE3 = "UPDATE Z3MeasurementTypes SET autocount = 1, increment = 0, displayed = 1, counted = 0, counttype = 1, autotype = 1 WHERE (name = 'Length')";
        private const string UPG_V3_MODIFY_MTYPES_TABLE4 = "UPDATE Z3MeasurementTypes SET autocount = 0, increment = 0, displayed = 0, counted = 1, counttype = 0, autotype = 1 WHERE (name = 'Count')";
        
        private const string UPG_V4_MODIFY_MTYPES_TABLE1 = "ALTER TABLE Z3MeasurementTypes ADD hotkey int not null default 0, defaultmode bit not null default 0;";
        private const string UPG_V4_DELETE_HOTKEY_DEFS = "DELETE FROM Z3CountableFields where name='Hotkey';";
        private const string UPG_V4_MODIFY_EGG_COUNT = "UPDATE Z3MeasurementTypes SET hotkey=192 where name='Egg Count'";
        private const string UPG_V4_MODIFY_LENGTH = "UPDATE Z3MeasurementTypes SET defaultmode=1 where name='Length'";


        private const string TABLE_EXISTENCE_QUERY = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=?";

        private const string DEFAULT_COUNTABLE_FIELDS = ", hotkey int not null default 0";
        private const string DEFAULT_CONTAINER_FIELDS = "";

        public static IWorkspace Load(String filename)
        {
            SqlCeConnection _conn = openConnection(filename);
            Options.LastWorkspace = filename;

            //Construct a new Workspace pointing to it
            return new WorkspaceInternals(filename, _conn);
        }

        public static IWorkspace Create(string schema, string filename, bool copyTaxonomy, bool copyReports)
        {
            if (File.Exists(filename)) File.Delete(filename);
            File.Copy(schema, filename);

            using (SqlCeConnection conn = openConnection(filename))
            {

               deleteTable(conn, "Z3Individuals"); // one exception
                //Debug.WriteLine("Here");
                executeDDL(conn, CREATE_INDIVIDUAL_TABLE);
               // Debug.WriteLine("Here");
                deleteTable(conn, "Z3Measurements"); // one exception
               // Debug.WriteLine("Here");
                executeDDL(conn, CREATE_MEASUREMENT_TABLE);
               // Debug.WriteLine("Here");
                populate(conn, copyTaxonomy); // two exceptions
               // Debug.WriteLine("Here");
                if (!copyReports || !tableExists(conn, "Z3Reports"))
                {
                    deleteTable(conn, "Z3Reports");
                    executeDDL(conn, UPG_V2_CREATE_REPORT_TABLE);
                }
                conn.Close();
            }
            return Load(filename);
        }

        #region Helper Methods
        private static bool tableExists(SqlCeConnection conn, string tablename)
        {
            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = TABLE_EXISTENCE_QUERY;
                cmd.Parameters.AddWithValue("@table", tablename);
                return (Convert.ToInt32(cmd.ExecuteScalar()) > 0);
            }
        }
        private static void deleteTable(SqlCeConnection conn, string tablename)
        {
            using (SqlCeCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE " + tablename;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlCeException)
                {
                    
                }
            }
        }
        private static SqlCeConnection openConnection(String filename)
        {
            SqlCeConnection _conn = new SqlCeConnection("data source='" + filename + "';");

            // If version of SqlCe is upgraded, upgrade database (handles change to 3.5)
            try
            {
                _conn.Open();
            }
            catch (SqlCeInvalidDatabaseFormatException)
            {
                using (SqlCeEngine g = new SqlCeEngine("data source='" + filename + "'; mode=Exclusive;"))
                {
                    g.Upgrade();
                }
                _conn.Open();
            }

            // Handle metaschema updates
            handleMetaSchemaUpdates(_conn);

            return _conn;
        }
        private static void handleMetaSchemaUpdates(SqlCeConnection _conn)
        {
            int version = getMetaSchemaVersion(_conn);
            if (version < 2)
            {
                executeDDL(_conn, UPG_V2_CREATE_REPORT_TABLE);
            }

            if (version < 3)
            {
                executeDDL(_conn, UPG_V3_MODIFY_MTYPES_TABLE1);
                executeDDL(_conn, UPG_V3_MODIFY_MTYPES_TABLE2);
                executeDDL(_conn, UPG_V3_MODIFY_MTYPES_TABLE3);
                executeDDL(_conn, UPG_V3_MODIFY_MTYPES_TABLE4);
            }

            if (version < 4)
            {
                executeDDL(_conn, UPG_V4_DELETE_HOTKEY_DEFS);
                executeDDL(_conn, UPG_V4_MODIFY_MTYPES_TABLE1);
                executeDDL(_conn, UPG_V4_MODIFY_EGG_COUNT);
                executeDDL(_conn, UPG_V4_MODIFY_LENGTH);
            }

            executeDDL(_conn, "delete from Z3DataSource where name='SYS_MetaSchemaVersion'");
            executeDDL(_conn, "insert into Z3DataSource (name, value) values('SYS_MetaSchemaVersion',4)");
        }
        private static void executeDDL(SqlCeConnection _conn, String query)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                catch(SqlCeException e)
                {
                    Debug.WriteLine(e.NativeError);
                }
                
            }
        }
        private static int getMetaSchemaVersion(SqlCeConnection _conn)
        {
            using (SqlCeCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "select value from Z3DataSource where name='SYS_MetaSchemaVersion'";
                using (SqlCeDataReader r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        return 0;
                    else
                        return Convert.ToInt32(r["value"]);
                }
            }
        }
        private static void populate(SqlCeConnection _conn, bool copyTaxonomy)
        {
            using (LevelManager mgr = new LevelManager(_conn))
            {
                // returns a collection of values from table Container
                foreach (ZLevel l in mgr.Types("Container")) // returns _cache[table] not sure how it gets cached
                {
                    Debug.WriteLine("This is the table: " + l.Table);
                    deleteTable(_conn, l.Table); // two exceptions
                    createTable(_conn, l.Table, l.Fields, DEFAULT_CONTAINER_FIELDS);
                }

                if (copyTaxonomy) // Only create the tables if they do not exist
                {
                    foreach (ZLevel l in mgr.Types("Countable"))
                    {
                        try
                        {
                            createTable(_conn, l.Table, l.Fields, DEFAULT_COUNTABLE_FIELDS);
                        }
                        catch (SqlCeException) { }
                    }
                }
                else
                {
                    foreach (ZLevel l in mgr.Types("Countable"))
                    {
                        deleteTable(_conn, l.Table);
                        createTable(_conn, l.Table, l.Fields, DEFAULT_COUNTABLE_FIELDS);
                    }
                }
            }
        }
        private static void createTable(SqlCeConnection _conn, string name, List<ZField> fields, string additionals)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("create table ");
            sb.Append(name);
            sb.Append(" (internalid int identity primary key, parent int not null default -1");
            sb.Append(additionals);
            foreach (ZField f in fields)
            {
                sb.Append(", ");
                sb.Append(f.Name);
                sb.Append(' ');
                if (f.Type.Equals("combobox"))
                {
                    sb.Append("nvarchar(100)");
                } else
                {
                    sb.Append(f.Type);
                }
                sb.Append(" not null default ");
                sb.Append(f.Default);
            }
            sb.Append(");");
            Debug.WriteLine(sb.ToString());
            executeDDL(_conn, sb.ToString());
        }
        #endregion

    }

    public class IndividualManager : IDisposable
    {
        private WorkspaceInternals _workspace;
        private Dictionary<int, ZIndividual> _indivs;
        private bool _disposed;

        public IndividualManager(WorkspaceInternals ws)
        {
            _workspace = ws;
            _indivs = new Dictionary<int, ZIndividual>();
        }

        public void Dispose() {
            if (_disposed) throw new ObjectDisposedException("IndividualManager");
            _disposed = true;
            _indivs.Clear();
            _workspace = null;
        }

        public ZIndividual byID(int id)
        {
            if (_disposed) throw new ObjectDisposedException("IndividualManager");

            if (!_indivs.ContainsKey(id))
            {
                using (SqlCeCommand cmd = _workspace.CreateCommand())
                {
                    cmd.CommandText = "select * from Z3Individuals where internalid=?";
                    cmd.Parameters.AddWithValue("@iid", id);
                    using (SqlCeDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            _indivs[id] = fromReader(r);
                        }
                        else
                            throw new InvalidOperationException("Record was not found.");
                    }
                }
            }

            return _indivs[id];
        }

        public ZIndividual fromReader(SqlCeDataReader r)
        {
            if (_disposed) throw new ObjectDisposedException("IndividualManager");

            int id = Convert.ToInt32(r["internalid"]);
            if (_indivs.ContainsKey(id)) return _indivs[id];

            ZIndividual z = new ZIndividual(
                Convert.ToInt32(r["internalid"]),
                Convert.ToInt32(r["Containerlevel"]),
                Convert.ToInt32(r["Container"]),
                Convert.ToInt32(r["Countablelevel"]),
                Convert.ToInt32(r["Countable"]),
                r["comments"].ToString(),
                _workspace
            );
            _indivs[id] = z;
            return z;
        }

        /// <summary>
        /// Creates a new ZIndividual in the database and returns its object.
        /// </summary>
        /// <param name="c">Data set</param>
        /// <param name="s">Countable</param>
        /// <param name="comments">Comments</param>
        /// <returns></returns>
        public ZIndividual insert(ZDataSet c, ZCountable s, string comments)
        {
            if (_disposed) throw new ObjectDisposedException("IndividualManager");
            Debug.WriteLine("Refering one: " + c.Name);
            int id = createNew(c.TypeID, c.ID, s.TypeID, s.ID, comments);
            c.hasBeenModified();
            return byID(id);
        }

        /// <summary>
        /// Creates a new Individual record and returns its id.
        /// </summary>
        /// <param name="dslvl">Container Type ID</param>
        /// <param name="dsid">Container ID</param>
        /// <param name="splvl">Countable Type ID</param>
        /// <param name="spid">Countable ID</param>
        /// <param name="cmts">Comments</param>
        /// <returns></returns>
        internal int createNew(int dslvl, int dsid, int splvl, int spid, string cmts)
        {
            if (_disposed) throw new ObjectDisposedException("IndividualManager");

            using (SqlCeCommand cmd = _workspace.CreateCommand())
            {
                cmd.CommandText =
                    "insert into Z3Individuals (ContainerLevel, Container, " +
                    "CountableLevel, Countable, Comments) values(" +
                    dslvl + ", " +
                    dsid + ", " +
                    splvl + ", " +
                    spid + ", ?)";
                cmd.Parameters.AddWithValue("@comments", cmts);
                Debug.WriteLine(cmd.CommandText);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Could not insert record");
            }

            return _workspace.getIdentity();
        }
    }

    public class IndividualDataPointsManager
    {
        private List<ZDataPoint> _points;
        private ZIndividual _indiv;
        private WorkspaceInternals _ws;

        public IndividualDataPointsManager(ZIndividual i, WorkspaceInternals ws)
        {
            _indiv = i;
            _ws = ws;
        }

        public ZDataPoint Get(ZMeasurement m, bool create)
        {
            if (_points == null) loadDataPoints();
            foreach (ZDataPoint d in _points)
            {
                if (d.MeasurementTypeID == m.ID) return d;
            }

            if (create)
            {
                return Add(m, 0, 0);
            }
            else
            {
                return null;
            }
        }

        public ZDataPoint Add(ZMeasurement m, double value, double weight)
        {
            if (_points == null) loadDataPoints();
            ZDataPoint retval = _ws.DataPoints.insert(_indiv, m, value, weight);

            _points.Add(retval);
            retval.Modified += new EventHandler(p_Modified);
            retval.Disposed += new EventHandler(p_Disposed);

            if (PointAdded != null)
                PointAdded(this, new ObjectEventArgs<ZDataPoint>(retval));
            this._indiv.DataSet.hasBeenModified();
            return retval;
        }

        public event EventHandler<ObjectEventArgs<ZDataPoint>> PointAdded;
        public event EventHandler<ObjectEventArgs<ZDataPoint>> PointModified;
        public event EventHandler<ObjectEventArgs<ZDataPoint>> PointDeleted;

        public List<ZDataPoint> GetAll()
        {
            if (_points == null) loadDataPoints();
            return _points;
        }

        public void Delete(ZDataPoint p)
        {
            _points.Remove(p);
            _ws.DataPoints.Delete(p);
        }

        private void loadDataPoints()
        {
            _points = new List<ZDataPoint>();
            if (_indiv.ID > -1)
            {
                using (SqlCeCommand cmd = _ws.CreateCommand())
                {
                    cmd.CommandText = "select * from Z3Measurements where individual=" + _indiv.ID;
                    using (SqlCeDataReader r = cmd.ExecuteReader()) {
                        while (r.Read()) {
                            ZDataPoint p = _ws.DataPoints.fromReader(r);
                            _points.Add(p);
                            p.Modified += new EventHandler(p_Modified);
                            p.Disposed += new EventHandler(p_Disposed);
                        }
                    }
                }
            }
        }

        void p_Disposed(object sender, EventArgs e)
        {
            if (PointDeleted != null)
            {
                PointDeleted(this, new ObjectEventArgs<ZDataPoint>((ZDataPoint)sender));
            }
            this._indiv.DataSet.hasBeenModified();
        }

        void p_Modified(object sender, EventArgs e)
        {
            if (PointModified != null)
            {
                PointModified(this, new ObjectEventArgs<ZDataPoint>((ZDataPoint)sender));
            }
            this._indiv.DataSet.hasBeenModified();
        } 
    }

    public class DataPointManager : IDisposable
    {
        public DataPointManager(WorkspaceInternals ws)
        {
            _ws = ws;
        }

        private WorkspaceInternals _ws;
        private bool _disposed = false;

        public void set(int id, string field, object value)
        {
            if (_disposed) throw new ObjectDisposedException("DataPointManager");

            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "update Z3Measurements set " + field + "=? where internalid=" + id;
                cmd.Parameters.AddWithValue("@field", value);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Could not insert measurement record");
            }
        }

        public ZDataPoint insert(ZIndividual i, ZMeasurement m, double value, double weight)
        {
            if (_disposed) throw new ObjectDisposedException("DataPointManager");

            int id = createInDB(i.ID, m.ID, value, weight);
            ZDataPoint d = new ZDataPoint(id, i.ID, m.ID, value, weight, _ws);
            return d;
        }

        public void Delete(ZDataPoint dp)
        {
            if (_disposed) throw new ObjectDisposedException("DataPointManager");

            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "delete from Z3Measurements where internalid=" + dp.ID;
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Could not delete measurement record");
            }

            dp.Dispose();
        }

        internal int createInDB(int individual, int mtype, double value, double weight)
        {
            if (_disposed) throw new ObjectDisposedException("DataPointManager");

            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "insert into Z3Measurements (Individual, Measurement, Value, Weight) values(" +
                    individual + ", " +
                    mtype + ", " +
                    value + ", " +
                    weight + ")";
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Could not insert measurement record");
            }

            return _ws.getIdentity();
        }

        public ZDataPoint fromReader(SqlCeDataReader r)
        {
            if (_disposed) throw new ObjectDisposedException("DataPointManager");

            ZDataPoint m = new ZDataPoint(
                Convert.ToInt32(r["internalid"]),
                Convert.ToInt32(r["individual"]),
                Convert.ToInt32(r["measurement"]),
                Convert.ToDouble(r["value"]),
                Convert.ToDouble(r["weight"]),
                _ws);
            return m;
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("DataPointManager");

            _ws = null;
            _disposed = true;
        }
    }
}

namespace Z3.State
{

    /// <summary>
    /// Allows you to listen for changes in the value of an object.
    /// </summary>
    public interface Watchable
    {
        event EventHandler Modified;
        event EventHandler Disposed;
    }
}