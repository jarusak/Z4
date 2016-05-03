using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;

namespace Z3.Workspace
{
    public class ReportType : IDisposable
    {
        private SqlCeConnection conn;
        private bool _disposed = false;

        public ReportType(int id, SqlCeConnection conn)
        {
            _id = id;
            this.conn = conn;
        }

        public SqlCeCommand CreateCommand()
        {
            if (_disposed) throw new ObjectDisposedException("ReportType");

            SqlCeCommand cmd = conn.CreateCommand();
            cmd.CommandText = _query;
            return cmd;
        }

        private int _id;

        public int ID
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ReportType");

                return _id;
            }
        }

        private string _name;

        public string Name
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ReportType");

                return _name;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ReportType");

                _name = value;
            }
        }
        private string _query;

        public string Query
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ReportType");

                return _query;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ReportType");

                _query = value;
            }
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("ReportType");

            _disposed = true;
            conn = null;
        }
    }

    public class ReportTypeManager
    {
        private WorkspaceInternals _ws;
        private List<ReportType> _coll = new List<ReportType>();
        private Dictionary<int, ReportType> _byid = new Dictionary<int, ReportType>();
        private bool _disposed = false;

        public ReportTypeManager(WorkspaceInternals ws)
        {
            if (_disposed) throw new ObjectDisposedException("ReportTypeManager");

            _ws = ws;
            loadTypes();
        }

        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("ReportTypeManager");
            _disposed = true;

            _ws = null;
            foreach (ReportType rt in _coll)
            {
                rt.Dispose();
            }
            _coll.Clear();
            _byid.Clear();
        }

        public List<ReportType> All
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ReportTypeManager");

                loadTypes();
                return _coll;
            }
        }

        public ReportType ByID(int id)
        {
            if (_disposed) throw new ObjectDisposedException("ReportTypeManager");

            if (_byid.ContainsKey(id)) return _byid[id];

            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "select * from Z3Reports where internalid=" + id;
                using (SqlCeDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        throw new System.Data.RowNotInTableException();
                    return readItem(dr);
                }
            }
        }

        public void Save(ReportType rt)
        {
            if (_disposed) throw new ObjectDisposedException("ReportTypeManager");

            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "update Z3Reports set name=?, query=? where internalid=" + rt.ID;
                cmd.Parameters.AddWithValue("@name", rt.Name);
                cmd.Parameters.AddWithValue("@query", rt.Query);
                if (cmd.ExecuteNonQuery() != 1)
                    throw new System.Data.RowNotInTableException();
            }
        }

        private void loadTypes()
        {
            _coll.Clear();
            _byid.Clear();
            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "select * from Z3Reports";
                using (SqlCeDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        readItem(dr);
                    }
                }
            }
        }

        private ReportType readItem(SqlCeDataReader dr)
        {
            ReportType r;
            if (_byid.ContainsKey(Convert.ToInt32(dr["internalid"])))
            {
                r = _byid[Convert.ToInt32(dr["internalid"])];
            }
            else
            {
                r = new ReportType(
                Convert.ToInt32(dr["internalid"]),
                _ws.Connection
                );
                _coll.Add(r);
                _byid[r.ID] = r;
            }
            
            r.Name = dr["name"].ToString();
            r.Query = dr["query"].ToString();
            
            return r;
        }

        public ReportType Create()
        {
            if (_disposed) throw new ObjectDisposedException("ReportTypeManager");

            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "insert into Z3Reports (name) values('New Type')";
                cmd.ExecuteNonQuery();
                ReportType r = ByID(_ws.getIdentity());
                return r;
            }
        }

        public void Delete(ReportType rt)
        {
            if (_disposed) throw new ObjectDisposedException("ReportTypeManager");

            _byid.Remove(rt.ID);
            _coll.Remove(rt);
            
            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "delete from Z3Reports where internalid=" + rt.ID;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
