using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;

namespace Z3.Model {
    public class ZContainer : Z3Entity {
        private static ZContainer _instance = new ZContainer();
        public static ZContainer FactoryInstance { get { return _instance; } }

        private ZContainer()
            : base("container") {

        }

        private static Dictionary<int, Dictionary<int, ZContainer>> _cache = new Dictionary<int, Dictionary<int, ZContainer>>();

        public static ZContainer GetContainer(int level, int id) {
            if (!_cache.ContainsKey(level))
                _cache[level] = new Dictionary<int, ZContainer>();
            if (!_cache[level].ContainsKey(id))
                _cache[level][id] = readFromDB(level, id);
            return _cache[level][id];
        }

        public override Z3Entity fromFactoryReader(int type, SqlCeDataReader r) { return fromReader(type, r); }
        public static ZContainer fromReader(int type, SqlCeDataReader r) {
            int i = Convert.ToInt32(r["internalid"]);
            if (!_cache.ContainsKey(type))
                _cache[type] = new Dictionary<int, ZContainer>();
            if (!_cache[type].ContainsKey(i))
                _cache[type][i] = new ZContainer();
            ZContainer m = _cache[type][i];

            m.ID = i;
            m.Name = r["name"].ToString();
            m.ParentID = Convert.ToInt32(r["parent"]);
            m.TypeID = type;

            return m;
        }
        private static ZContainer readFromDB(int level, int id) {
            ZContainer c = new ZContainer();
            c.getExisting(level, id);
            return c;
        }

        protected override void readDelegate(System.Data.SqlServerCe.SqlCeDataReader r) {
            
        }

        protected override void deleteDelegate() {
            _cache[Type.ID].Remove(ID);
        }

        protected override Z3Entity factorize(int level, int parent) {
            ZContainer m = new ZContainer();
            m.getNew(level, parent, "", new object[] {});

            return m;
        }

        private static List<EntityWatcher> _watchers = new List<EntityWatcher>();
        public override void Subscribe(EntityWatcher e) {
            _watchers.Add(e);
            e.LoadBase(all());
        }

        private List<Z3Entity> all() {
            List<Z3Entity> retval = new List<Z3Entity>();
            foreach (Dictionary<int, ZContainer> d in _cache.Values)
                foreach (ZContainer z in d.Values)
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

        public override string ToString() {
            return Name;
        }
    }
}
