using System;

using Z3.State;
using Z3.Workspace;

namespace Z3.Model{
    public class ZDataPoint : Watchable {
        private int _individualID;
        private int _id;
        private int _mtype;
        private double _value;
        private WorkspaceInternals _ws;

        public ZDataPoint(int id, int iid, int mtype, double value, WorkspaceInternals ws)
        {
            _id = id;
            _individualID = iid;
            _mtype = mtype;
            _value = value;
            _ws = ws;
        }

        #region Properties
        public int IndividualID {
            get
            {
                return _individualID;
            }
        }

        public int ID {
            get
            {
                return _id;
            }

        }

        public int MeasurementTypeID {
            get
            {
                return _mtype;
            }

        }

        public ZMeasurement MeasurementType
        {
            get
            {
                return _ws.getMeasurementType(_mtype);
            }
        }

        public double Value {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZDataPoint");
                return _value;
            }
            set {
                if (_disposed) throw new ObjectDisposedException("ZDataPoint");
                _value = value;
                _ws.DataPoints.set(_id, "value", value);
                if (Modified != null)
                    Modified(this, new EventArgs());
            }
        }

        public ZIndividual Individual
        {
            get
            {
                return _ws.Individuals.byID(_individualID);
            }
        }
        #endregion

        private bool _disposed = false;
        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("ZDataPoint");
            _disposed = true;
            if (Disposed != null)
                Disposed(this, new EventArgs());
        }

        public event EventHandler Modified;
        public event EventHandler Disposed;

        internal void Delete()
        {
            Individual.DataPoints.Delete(this);
        }
    }
}
