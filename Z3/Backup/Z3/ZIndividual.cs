using System;

using Z3.State;
using Z3.Workspace;
using Z3.Util;

namespace Z3.Model {
    public class ZIndividual : Watchable, IDisposable {
        private int _id;
        private int _dataSetTypeID;
        private int _dataSetID;
        private int _CountableTypeID;
        private int _CountableID;
        private string _comments;
        private IndividualDataPointsManager _ptsmgr;

        #region Properties
        public IndividualDataPointsManager DataPoints
        {
            get { return _ptsmgr; }
        }

        public int ID {
            get { return _id; }
        //    set { _id = value; }
        }


        public int DataSetTypeID {
            get { return _dataSetTypeID; }
            //set { _dataSetTypeID = value; }
        }

        public ZDataSet DataSet
        {
            get
            {
                return _ws.DataSetStore.ByID(_dataSetTypeID, _dataSetID);
            }
        }

        public int DataSetID {
            get { return _dataSetID; }
            //set { _dataSetID = value; }
        }


        public int CountableTypeID {
            get { return _CountableTypeID; }
            //set { _CountableTypeID = value; }
        }


        public int CountableID {
            get { return _CountableID; }
            //set { _CountableID = value; }
        }

        public ZCountable Countable
        {
            get
            {
                return _ws.CountableStore.ByID(CountableTypeID, CountableID);
            }
        }

        public string Comments {
            get { return _comments; }
        //    set { _comments = value; }
        }
        #endregion

        private bool _disposed;
        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("ZIndividual[" + _id);
            _disposed = true;
            if (Disposed != null)
                Disposed(this, new EventArgs());
        }

        public event EventHandler Modified;
        public event EventHandler Disposed;
        public event EventHandler<ObjectEventArgs<ZDataPoint>> PointAdded;
        public event EventHandler<ObjectEventArgs<ZDataPoint>> PointModified;
        public event EventHandler<ObjectEventArgs<ZDataPoint>> PointDeleted;

        public ZIndividual(int id, int dstid, int dsid, int ctid, int cid, string com, WorkspaceInternals ws)
        {
            _id = id;
            _dataSetTypeID = dstid;
            _dataSetID = dsid;
            _CountableTypeID = ctid;
            _CountableID = cid;
            _comments = com;
            _ws = ws;

            _ptsmgr = new IndividualDataPointsManager(this, ws);

            _ptsmgr.PointAdded += new EventHandler<ObjectEventArgs<ZDataPoint>>(_ptsmgr_PointAdded);
            _ptsmgr.PointDeleted += new EventHandler<ObjectEventArgs<ZDataPoint>>(_ptsmgr_PointDeleted);
            _ptsmgr.PointModified += new EventHandler<ObjectEventArgs<ZDataPoint>>(_ptsmgr_PointModified);
        }

        void _ptsmgr_PointModified(object sender, ObjectEventArgs<ZDataPoint> e)
        {
            if (PointModified != null)
                PointModified(sender, e);
            if (Modified != null)
                Modified(sender, e);
        }

        void _ptsmgr_PointDeleted(object sender, ObjectEventArgs<ZDataPoint> e)
        {
            if (PointDeleted != null)
                PointDeleted(sender, e);
            if (Modified != null)
                Modified(sender, e);
        }

        void _ptsmgr_PointAdded(object sender, ObjectEventArgs<ZDataPoint> e)
        {
            if (PointAdded != null)
                PointAdded(sender, e);
            if (Modified != null)
                Modified(sender, e);
        }
        WorkspaceInternals _ws;
    }
}