using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using Z3.Leger;

namespace Z3.Model {
    public class Z3Countable : Z3Entity {
        #region Static Members

        private static Z3Countable _instance = new Z3Countable();
        public static Z3Countable FactoryInstance { get { return _instance; } }
        
        private static List<EntityWatcher> _watchers = new List<EntityWatcher>();
        private static Dictionary<int, Dictionary<int, Z3Countable>> _cache = new Dictionary<int, Dictionary<int, Z3Countable>>();
        
        public static Z3Countable GetCountable(int level, int id) {
            if (!_cache.ContainsKey(level))
                _cache[level] = new Dictionary<int, Z3Countable>();
            if (!_cache[level].ContainsKey(id))
                _cache[level][id] = readFromDB(level, id);
            return _cache[level][id];
        }

        private static Z3Countable readFromDB(int level, int id) {
            Z3Countable c = new Z3Countable();
            c.getExisting(level, id);
            return c;
        }
        #endregion

        protected override void readDelegate(SqlCeDataReader r) {
            Hotkey = Convert.ToChar(r["hotkey"]);
        }

        protected override void deleteDelegate() {
            _cache[Type.ID].Remove(ID);
        }

        private char _hotkey;
        public char Hotkey {
            get { return _hotkey; }
            set { _hotkey = value; }
        }

        private Z3Countable() : base("Countable") { }

        public override Z3Entity fromFactoryReader(int type, SqlCeDataReader r) { return fromReader(type, r); }
        public static Z3Countable fromReader(int type, SqlCeDataReader r) {
            int i = Convert.ToInt32(r["internalid"]);
            if (!_cache.ContainsKey(type))
                _cache[type] = new Dictionary<int, Z3Countable>();
            if (!_cache[type].ContainsKey(i))
                _cache[type][i] = new Z3Countable();
            Z3Countable m = _cache[type][i];

            m.ID = i;
            m.Name = r["name"].ToString();
            m.ParentID = Convert.ToInt32(r["parent"]);
            m.Hotkey = Convert.ToChar(r["hotkey"]);
            m.TypeID = type;
            
            return m;
        }
        
        protected override Z3Entity factorize(int level, int parent) {
            Z3Countable m = new Z3Countable();
            m.getNew(level, parent, ", hotkey", new object[]{0});

            return m;
        }

        public override String ToString() {
            return Name;
        }

        public override void Subscribe(EntityWatcher e) {
            _watchers.Add(e);
            e.LoadBase(all());
        }

        private List<Z3Entity> all() {
            List<Z3Entity> retval = new List<Z3Entity>();
            foreach (Dictionary<int, Z3Countable> d in _cache.Values)
                foreach (Z3Countable z in d.Values)
                    retval.Add(z);
            return retval;
        }

        public override void Unsubscribe(EntityWatcher e) { _watchers.Remove(e); }

        protected override void listenerAdd() {
            foreach (EntityWatcher e in _watchers)
                e.EntityAdded(this);
        }

        protected override void listenerEdit() {
            foreach (EntityWatcher e in _watchers)
                e.EntityEdited(this);
        }

        protected override void listenerDelete() {
            foreach (EntityWatcher e in _watchers)
                e.EntityDeleted(this);
        }
    }
}
