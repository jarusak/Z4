using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Text;
using Z3.Model;

namespace Z3.Leger
{
    public class Z3Schema
    {
        private static Z3Schema _instance = new Z3Schema();

        public static Z3Schema getInstance() { return _instance; }

        /// <summary>
        /// SQL to create the Z3Individuals table.
        /// </summary>
        private static string CREATE_INDIVIDUAL_TABLE = "create table Z3Individuals (internalid int identity, Container int not null, Countable int not null, Comments ntext not null default(''));";
        
        /// <summary>
        /// SQL to create the Z3Measurements table.
        /// </summary>
        private static string CREATE_MEASUREMENT_TABLE = "create table Z3Measurements (internalid int identity, Individual int not null, Measurement int not null, Value float not null)";
        
        /// <summary>
        /// Z3Workspace or Schema filename
        /// </summary>
        private String _fn;

        public String FileName
        {
          get { return _fn; }
          set { _fn = value; }
        }
        
        private SqlCeConnection _conn;
        private Dictionary<string, string> _properties;

        public static SqlCeCommand CreateCommand() { 
            return getInstance()._conn.CreateCommand();
        }

        private Z3Schema(String filename, SqlCeConnection conn) {
            _fn = filename;
            _conn = conn;
            init();
        }

        public static void open(string filename) {
            _instance = new Z3Schema(filename);
        }

        private Z3Schema() {
            
        }

        private Z3Schema(string filename)
        {
            _fn = filename;
            _conn = new SqlCeConnection("data source='" + _fn + "'; mode=Exclusive;");
            try {
                _conn.Open();
            } catch (SqlCeInvalidDatabaseFormatException) {
                using (SqlCeEngine g = new SqlCeEngine("data source='" + _fn + "'; mode=Exclusive;")) {
                    g.Upgrade();
                }
                _conn.Open();
            }

            _instance = this;
            init();
        }

        public void Close() {
            if (_conn != null)
            {
                try { _conn.Close(); }
                catch (Exception) { }
                //We don't care about any errors... we're just trying to
                //do a little proactive garbage collection.
                //  Yes, I realize this is considered bad practice. But we are
                //  just throwing this object away.
                _conn.Dispose();
            }
        }

        private void init() {
            Z3Level.buildInitialTree("container");
            Z3Level.buildInitialTree("countable");
            getBaseEntities("container");
            getBaseEntities("countable");
            _properties = getProperties();
        }

        private Dictionary<string,string> getProperties() {
            Dictionary<string, string> retval = new Dictionary<string, string>();

            using (SqlCeCommand cmd = _conn.CreateCommand()) {
                cmd.CommandText = "select * from z3datasource";
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                        retval.Add(r["name"].ToString(), r["value"].ToString());
                    }
                }
                return retval;
            }
        }

        public String getFileName() {
            return _fn;
        }
        
        public void makeWorkspace() {
            foreach (Z3Level l in Z3Level.Types("container")) {
                createTable(l.Name + "Container", l.Fields);
            }
            foreach (Z3Level l in Z3Level.Types("container")) {
                createTable(l.Name + "Countable", l.Fields);
            }
            executeDDL(CREATE_INDIVIDUAL_TABLE);
            executeDDL(CREATE_MEASUREMENT_TABLE);
        }

        private void createTable(string name, List<Z3Field> fields) {
            StringBuilder sb = new StringBuilder();
            sb.Append("create table ");
            sb.Append(name);
            sb.Append(" (internalid int identity primary key, parent int not null default -1");
            foreach (Z3Field f in fields) {
                sb.Append(", ");
                sb.Append(f.Name);
                sb.Append(' ');
                sb.Append(f.Type);
                sb.Append(" not null default ");
                sb.Append(f.Default);
            }
            sb.Append(");");
            executeDDL(sb.ToString());
        }

        private void executeDDL(String query) {
            using (SqlCeCommand cmd = _conn.CreateCommand()) {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }

        public String getName() {
            return _properties["Name"];
        }

        public List<Z3Measurement> getMeasurementTypes() {
            List<Z3Measurement> retval = new List<Z3Measurement>();
            if (_conn == null) return null;

            using (SqlCeCommand cmd = _conn.CreateCommand()) {
                cmd.CommandText = "select * from z3measurementtypes";
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                        retval.Add(Z3Measurement.fromReader(r));
                    }
                }
                return retval;
            }
        }

        public List<Z3Entity> getBaseEntities(string table) {
            List<Z3Entity> retval = new List<Z3Entity>();

            foreach (Z3Level l in Z3Level.Types(table)) {
                retval.AddRange(getBaseEntitiesForLevel(table, l));
            }

            return retval;
        }

        public List<Z3Entity> getBaseEntitiesForLevel(string table, Z3Level l) {
            List<Z3Entity> retval = new List<Z3Entity>();

            using (SqlCeCommand cmd = _conn.CreateCommand()) {
                cmd.CommandText = "select * from " + l.Name + table;
                using (SqlCeDataReader r = cmd.ExecuteReader()) {
                    while (r.Read()) {
                        retval.Add(Z3Entity.Factory(table).fromFactoryReader(l.ID, r));
                    }
                }
                return retval;
            }
        }

        public int getIdentity() {
            using (SqlCeCommand cmd = _conn.CreateCommand()) {
                cmd.CommandText = "select @@IDENTITY";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
