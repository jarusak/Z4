using System;

using Z3.Workspace;
using Z3.Model;
using Z3.Util;


namespace Z3.State
{
    public class CalibrationState
    {
        private double _factor;
        public double CalibrationFactor
        {
            get
            {
                return _factor;
            }
            set
            {
                _factor = value;
            }
        }

    }

    public class ProgramState
    {
        private Watcher<ZIndividual> _zi = new WatcherImpl<ZIndividual>();
        public Watcher<ZIndividual> CurrentIndividual
        {
            get
            {
                return _zi;
            }
        }

        private Watcher<IWorkspace> _cw = new WatcherImpl<IWorkspace>();
        public Watcher<IWorkspace> CurrentWorkspace
        {
            get
            {
                return _cw;
            }
        }

        private Watcher<ZDataSet> _cds = new WatcherImpl<ZDataSet>();
        public Watcher<ZDataSet> CurrentDataSet
        {
            get
            {
                return _cds;
            }
        }

        private Watcher<ZDataPoint> _cdp = new WatcherImpl<ZDataPoint>();
        public Watcher<ZDataPoint> CurrentDataPoint
        {
            get
            {
                return _cdp;
            }
        }

        private Watcher<ZCountable> _cc = new WatcherImpl<ZCountable>();
        public Watcher<ZCountable> CurrentCountable
        {
            get
            {
                return _cc;
            }
        }

        private Watcher<ZMeasurement> _cm = new WatcherImpl<ZMeasurement>();
        public Watcher<ZMeasurement> CurrentMeasurement
        {
            get
            {
                return _cm;
            }
        }
    }


    /// <summary>
    /// Watches a particular variable, and notifies listeners when the value changes
    /// or is replaced.
    /// </summary>
    /// <typeparam name="T">Type to watch</typeparam>
    public interface Watcher<T> where T : Watchable
    {
        /// <summary>
        /// Notifies listeners when the value of the object, not the object reference, is changed.
        /// </summary>
        event EventHandler<ObjectEventArgs<Watcher<T>>> Modified;
        event EventHandler<ObjectEventArgs<Watcher<T>>> ReplacedPreview;
        /// <summary>
        /// Notifies listeners when the object reference itself is changed.
        /// </summary>
        event EventHandler<ObjectEventArgs<Watcher<T>>> Replaced;
        /// <summary>
        /// Notifies listeners when the watched object is cleared or becomes invalidated.
        /// </summary>
        event EventHandler<ObjectEventArgs<Watcher<T>>> Cleared;
        /// <summary>
        /// Gets a boolean representing whether a value has been loaded or not.
        /// </summary>
        bool Loaded { get; }
        /// <summary>
        /// Get or set the current value.
        /// </summary>
        T Value { get; set; }
        /// <summary>
        /// Clears the current value.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// A generic Watcher implmentation.
    /// </summary>
    /// <typeparam name="T">Watchable Class</typeparam>
    public class WatcherImpl<T> : Watcher<T> where T : class, Watchable
    {
        protected T obj;

        public event EventHandler<ObjectEventArgs<Watcher<T>>> Modified;
        public event EventHandler<ObjectEventArgs<Watcher<T>>> ReplacedPreview;
        public event EventHandler<ObjectEventArgs<Watcher<T>>> Replaced;
        public event EventHandler<ObjectEventArgs<Watcher<T>>> Cleared;

        public bool Loaded
        {
            get
            {
                return (obj != null);
            }
        }

        private EventHandler modHandler;
        private EventHandler delHandler;

        public T Value
        {
            get { return obj; }
            set
            {
                if (value == null) throw new NullReferenceException();
                if (value == obj) return;
                
                Clear();

                obj = value;
                modHandler = new EventHandler(obj_Modified);
                delHandler = new EventHandler(obj_Disposed);
                obj.Modified += modHandler;
                obj.Disposed += delHandler;
                if (ReplacedPreview != null)
                    ReplacedPreview(this, new ObjectEventArgs<Watcher<T>>(this));
                if (Replaced != null)
                    Replaced(this, new ObjectEventArgs<Watcher<T>>(this));
            }
        }

        void obj_Disposed(object sender, EventArgs e)
        {
            Clear();
        }

        private bool shouldWait = false;
        private bool dirty = false;

        void obj_Modified(object sender, EventArgs e)
        {
            // Every time a Modified event is received from the object, we set dirty = true
            // But if one object modifies it and the modify event listeners also modify it...
            // then we should just wait and notify everyone again at the end.  Not threadsafe;
            // this code just handles reentrancy and prevents infinite stack growth.
            dirty = true;
            if (shouldWait) return;

            shouldWait = true;
            while (dirty)
            {
                dirty = false;
                if (Modified != null)
                    Modified(this, new ObjectEventArgs<Watcher<T>>(this));
            }
            shouldWait = false;
        }

        public void Clear()
        {
            if (obj != null)
            {
                obj.Modified -= modHandler;
                obj.Disposed -= delHandler;

                T oldObj = obj;
                obj = null;

                if (Cleared != null)
                {
                    Cleared(this, new ObjectEventArgs<Watcher<T>>(this, oldObj));
                }
            }
        }
    }
}

namespace Z3.State.StatusMessages
{
    public interface StatusMessageState
    {
        StatusMessageState FileLoaded();
        StatusMessageState FileClosed();
        StatusMessageState Calibrated();
        StatusMessageState Decalibrated();
        StatusMessageState DataSetPicked();
        StatusMessageState DataSetCleared();
        StatusMessageState CountablePicked();
        StatusMessageState CountableCleared();
        StatusMessageState MeasurementPicked();
        StatusMessageState MeasurementCleared();
        StatusMessageState StartCounting();
        StatusMessageState StopCounting();

        string Message { get; }
    }

    public class FileLoadedState : StatusMessageState
    {
        private bool _cal = false;
        private bool _ds = false;
        private bool _spec = false;
        private bool _m = false;
        private bool _start = false;

        public FileLoadedState(bool Calibrated, bool DataSet, bool Species, bool Measurement, bool Start) {
            _cal = Calibrated;
            _ds = DataSet;
            _spec = Species;
            _m = Measurement;
            _start = Start;
        }

        #region StatusMessageState Members

        public StatusMessageState FileLoaded()
        {
            return this;
        }

        public StatusMessageState FileClosed()
        {
            if (_cal)
                return new InitialCalState();
            else
                return new InitialState();
        }

        public StatusMessageState Calibrated()
        {
            _cal = true;
            return this;
        }

        public StatusMessageState Decalibrated()
        {
            _cal = false;
            return this;
        }

        public StatusMessageState DataSetPicked()
        {
            _ds = true;
            return this;
        }

        public StatusMessageState DataSetCleared()
        {
            _ds = false;
            return this;
        }

        public StatusMessageState CountablePicked()
        {
            _spec = true;
            return this;
        }

        public StatusMessageState CountableCleared()
        {
            _spec = false;
            return this;
        }

        public StatusMessageState MeasurementPicked()
        {
            _m = true;
            return this;
        }

        public StatusMessageState MeasurementCleared()
        {
            _m = false;
            return this;
        }

        public StatusMessageState StartCounting()
        {
            _start = true;
            return this;
        }

        public StatusMessageState StopCounting()
        {
            _start = false;
            return this;
        }

        public string Message
        {
            get
            {
                if (!_cal)
                {
                    return "Please Calibrate the Display.";
                }
                else if (!_ds)
                {
                    return "Please Select or Create a Data Set.";
                }
                else if (!_m)
                {
                    return "Please Select a Measurement Type.";
                }
                else if (!_spec)
                {
                    if (_start)
                    {
                        return "Please select a Countable or press a Hotkey.";
                    }
                    else
                    {
                        return "Please select a Countable.";
                    }
                }
                else // Ready to go
                {
                    return "You are ready to Count/Measure by Mouse or Hotkey.";
                }
            }
        }

        #endregion
    }

    public class InitialCalState : StatusMessageState
    {

        #region StatusMessageState Members

        public StatusMessageState FileLoaded()
        {
            return new FileLoadedState(true, false, false, false, false);
        }

        public StatusMessageState FileClosed()
        {
            return this;
        }

        public StatusMessageState Calibrated()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState Decalibrated()
        {
            return new InitialState();
        }

        public StatusMessageState DataSetPicked()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState DataSetCleared()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState CountablePicked()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState CountableCleared()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState MeasurementPicked()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState MeasurementCleared()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState StartCounting()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState StopCounting()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string Message
        {
            get {
                return "Open or create a Workspace to begin.";
            }
        }

        #endregion
    }

    public class InitialState : StatusMessageState
    {

        #region StatusMessageState Members
        public string Message
        {
            get
            {
                return "Open or create a Workspace, or calibrate the display.";
            }
        }

        public StatusMessageState FileLoaded()
        {
            return new FileLoadedState(false,false,false,false,false);
        }

        public StatusMessageState FileClosed()
        {
            return this;
        }

        public StatusMessageState Calibrated()
        {
            return new InitialCalState();
        }

        public StatusMessageState Decalibrated()
        {
            return this;
        }

        public StatusMessageState DataSetPicked()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState DataSetCleared()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState CountablePicked()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState CountableCleared()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState MeasurementPicked()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState MeasurementCleared()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState StartCounting()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StatusMessageState StopCounting()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
