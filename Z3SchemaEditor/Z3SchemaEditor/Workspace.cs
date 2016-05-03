using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

using Z3.Model;
using System.IO;
using System.Text;
using Z3.Util;

namespace Z3.Workspace
{
    /// <summary>
    /// Provides full access to the database functionality, for the Model
    /// and Workspace classes only.
    /// </summary>
    public class WorkspaceInternals
    {
        private string _fn;
        private SqlCeConnection _connection;
        private LevelManager _levels;
        private List<ZMeasurement> _mtypes;
        private ReportTypeManager _rtypes;
        private bool _disposed = false;
        private Dictionary<string, string> _properties;

        public WorkspaceInternals(string filename, SqlCeConnection conn)
        {
            _connection = conn;
            _fn = filename;
            _properties = getProperties();

            _levels = new LevelManager(this);
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
        public int getIdentity()
        {
            if (_disposed) throw new ObjectDisposedException("workspace");
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "select @@IDENTITY";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public static int getIdentity(SqlCeConnection _connection)
        {
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "select @@IDENTITY";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        #endregion

        #region IWorkspace Members
        public void refreshMeasurementTypes()
        {
            _mtypes = new List<ZMeasurement>();

            using (SqlCeCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "select * from z3measurementtypes";
                using (SqlCeDataReader r = cmd.ExecuteReader())
                    while (r.Read())
                        _mtypes.Add(ZMeasurement.fromReader(r, this));
            }
        }
        
        public List<ZMeasurement> MeasurementTypes
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("workspace");
                if (Connection == null) return null;

                if (_mtypes == null) refreshMeasurementTypes();
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

        
        public Dictionary<string, string> getProperties()
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

        public void addProperty(string name, string value)
        {
            if (name.ToUpper().StartsWith("SYS_")) return;
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "insert into Z3DataSource (name, value) values(?, ?)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
            }
        }

        public void setProperty(string name, string newname, string value)
        {
            if (name.ToUpper().StartsWith("SYS_")) return;
            if (newname.ToUpper().StartsWith("SYS_")) return;
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "update Z3DataSource set name=?,value=? where name=?";
                cmd.Parameters.AddWithValue("@nname", newname);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("workspace");

            _disposed = true;
            _levels.Dispose();
            foreach (ZMeasurement m in _mtypes)
            {
                m.Dispose();
            }
            _rtypes.Dispose();
            _connection.Close();
            Disposed(this, new EventArgs());
        }
        public event EventHandler Modified;
        public event EventHandler Disposed;

        public void addMeasurementType(string p)
        {
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "insert into Z3MeasurementTypes (name) values(?)";
                cmd.Parameters.AddWithValue("@name", p);
                cmd.ExecuteNonQuery();
            }
        }

        public void deleteProperty(string p)
        {
            if (p.ToUpper().StartsWith("SYS_")) return;
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "delete from Z3DataSource where name=?";
                cmd.Parameters.AddWithValue("@name", p);
                cmd.ExecuteNonQuery();
            }
        }

        public void deleteMeasurementType(ZMeasurement zMeasurement)
        {
            using (SqlCeCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "delete from Z3MeasurementTypes where internalid=?";
                cmd.Parameters.AddWithValue("@name", zMeasurement.ID);
                cmd.ExecuteNonQuery();
            }
        }
    }

    #region Hierarchy Stores

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

    //internal class CountableFactory : HierarchicalEntityFactory<ZCountable>
    //{

    //}
    #endregion

    public static class Factory
    {
        private const string CREATE_INDIVIDUAL_TABLE = "CREATE TABLE Z3Individuals (internalid int identity, Container int not null, Countable int not null, ContainerLevel int not null, CountableLevel int not null, Comments ntext not null default(''));";
        private const string CREATE_MEASUREMENT_TABLE = "CREATE TABLE Z3Measurements (internalid int identity, Individual int not null, Measurement int not null, Value float not null)";

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

        private const string SCHEMA_CREATE_RTYPES = UPG_V2_CREATE_REPORT_TABLE;
        private const string SCHEMA_CREATE_CONTFIELDS = "create table Z3ContainerFields (internalid int identity primary key, name nvarchar(100) not null default 'something', label nvarchar(100) not null default 'something', \"default\" nvarchar(100) not null default 'something', type nvarchar(100) not null default 'nvarchar(100)', level int not null default 1);";
        private const string SCHEMA_CREATE_CONTLEVELS = "create table Z3ContainerLevels (internalid int identity primary key, name nvarchar(30) not null default 'something', child int not null default 1, final bit not null default 1, root bit not null default 1, measurable bit not null default 1);";
        private const string SCHEMA_CREATE_COUNTFIELDS = "create table Z3CountableFields ( internalid int identity primary key, name nvarchar(100) not null default 'something', label nvarchar(100) not null default 'something', \"default\" nvarchar(100) not null default 'something', type nvarchar(100) not null default 'nvarchar(100)', level int not null default 1);";
        private const string SCHEMA_CREATE_COUNTLEVELS = "create table Z3CountableLevels (internalid int identity primary key, name nvarchar(30) not null default 'something', child int not null default 1, final bit not null default 1, root bit not null default 1, measurable bit not null default 1);";
        private const string SCHEMA_CREATE_DATASOURCE = "create table Z3DataSource (name nvarchar(100) not null default 'something', value nvarchar(100) not null default 'something');";
        private const string SCHEMA_CREATE_MTYPES = "create table Z3MeasurementTypes (internalid int identity primary key, measured bit not null default 1, autocount bit not null default 1, name nvarchar(100) not null default 'something', required bit not null default 0, increment bit not null default 1, displayed bit not null default 1, counted bit not null default 1, counttype int not null default 0, autotype int not null default 0, Hotkey int not null default 0, defaultmode bit not null default 0);";

        public static void CreateSchema(string filename)
        {
            using (SqlCeEngine eng = new SqlCeEngine("data source='" + filename + "';"))
            {
                if (File.Exists(filename)) File.Delete(filename);
                eng.CreateDatabase();
            }
            using (SqlCeConnection _conn = new SqlCeConnection("data source='" + filename + "';"))
            {
                _conn.Open();
                executeDDL(_conn, SCHEMA_CREATE_CONTFIELDS);
                executeDDL(_conn, SCHEMA_CREATE_CONTLEVELS);
                executeDDL(_conn, SCHEMA_CREATE_COUNTFIELDS);
                executeDDL(_conn, SCHEMA_CREATE_COUNTLEVELS);
                executeDDL(_conn, SCHEMA_CREATE_DATASOURCE);
                executeDDL(_conn, SCHEMA_CREATE_MTYPES);
                executeDDL(_conn, SCHEMA_CREATE_RTYPES);
                executeDDL(_conn, "insert into Z3DataSource (name, value) values('SYS_MetaSchemaVersion',4)");
            }
        }

        public static WorkspaceInternals Load(String filename)
        {
            SqlCeConnection _conn = openConnection(filename);

            //Construct a new Workspace pointing to it
            return new WorkspaceInternals(filename, _conn);
        }

        public static WorkspaceInternals Create(string schema, string filename, bool copyTaxonomy, bool copyReports)
        {
            if (File.Exists(filename)) File.Delete(filename);
            File.Copy(schema, filename);

            using (SqlCeConnection conn = openConnection(filename))
            {
                deleteTable(conn, "Z3Individuals");
                executeDDL(conn, CREATE_INDIVIDUAL_TABLE);
                deleteTable(conn, "Z3Measurements");
                executeDDL(conn, CREATE_MEASUREMENT_TABLE);
                populate(conn, copyTaxonomy);
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
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
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
                foreach (ZLevel l in mgr.Types("Container"))
                {
                    deleteTable(_conn, l.Table);
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
                sb.Append(f.Type);
                sb.Append(" not null default ");
                sb.Append(f.Default);
            }
            sb.Append(");");
            executeDDL(_conn, sb.ToString());
        }
        #endregion

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