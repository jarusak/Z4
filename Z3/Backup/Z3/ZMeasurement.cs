using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using Z3.State;
using Z3.Workspace;
using Z3.Util;
using System.Windows.Forms;

namespace Z3.Model {
    public class ZMeasurement : Watchable, Hotkeyable {
        private ZMeasurement() { }

        private int _id;
        private bool _measured;
        private bool _counted;
        private bool _autocount;
        private bool _required;
        private String _name;
        private bool _increment;
        private bool _displayed;
        private int _counttype;
        private int _autotype;
        private Keys _hotkey;
        private bool _default;

        #region Properties
        
        public int ID {
            get { return _id; }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _id = value;
            }
        }
        
        public bool Measured {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _measured;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _measured = value;
            }
        }

        public int AutoTypeID
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _autotype;
            }
            set { _autotype = value; }
        }

        public ZMeasurement AutoType
        {
            get { return _ws.getMeasurementType(AutoTypeID); }
        }

        public bool Counted
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _counted;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _counted = value;
            }
        }
        
        public bool AutoCount {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _autocount;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _autocount = value;
            }
        }

        public bool Required
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _required;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _required = value;
            }
        }

        public String Name {
            get
            {
                return _name;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _name = value;
            }
        }

        public bool Increment
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _increment;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _increment = value;
            }
        }

        public bool Displayed
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _displayed;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _displayed = value;
            }
        }

        public bool Default
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _default;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _default = value;
            }
        }

        public int CountTypeID
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _counttype;
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                _counttype = value;
            }
        }

        public ZMeasurement CountType
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("ZMeasurement");
                return _ws.getMeasurementType(CountTypeID);
            }
        }
        #endregion

        event EventHandler Watchable.Modified { add { } remove { } }
        public event EventHandler Disposed;

        public override String ToString() {
            if (!Keyboard.IsMeaningful(_hotkey))
            {
                return _name;
            }
            else
            {
                return _name + " (" + Keyboard.GetReadableKey(_hotkey) + ")";
            }
        }

        private IWorkspace _ws;

        public static ZMeasurement fromReader(SqlCeDataReader r, IWorkspace ws) {
            ZMeasurement m = new ZMeasurement();
            m._id = Convert.ToInt32(r["internalid"]);
            m._name = r["name"].ToString();
            m._autocount = Convert.ToBoolean(r["autocount"]);
            m._measured = Convert.ToBoolean(r["measured"]);
            m._counted = Convert.ToBoolean(r["counted"]);
            m._required = Convert.ToBoolean(r["required"]);
            m._displayed = Convert.ToBoolean(r["displayed"]);
            m._increment = Convert.ToBoolean(r["increment"]);
            m._counttype = Convert.ToInt32(r["counttype"]);
            m._autotype = Convert.ToInt32(r["autotype"]);
            m._hotkey = (Keys)Convert.ToInt32(r["hotkey"]);
            m._default = Convert.ToBoolean(r["defaultmode"]);
            m._ws = ws;
            return m;
        }

        public Keys Hotkey
        {
            get
            {
                return _hotkey;
            }
            set
            {
                throw new OperationCanceledException("Cannot change this hotkey yet");
            }
        }

        private bool _disposed = false;
        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException("ZMeasurement");
            _disposed = true;

            Disposed(this, new EventArgs());
        }
    }
}
