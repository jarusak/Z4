using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using Z3.Workspace;
using System.Windows.Forms;
using Z3.Util;
using System.Diagnostics;

namespace Z3.Model {
    public class ZCountable : BasicHierarchicalEntity, Hotkeyable {
        #region Static Members

        //private static ZCountable _instance = new ZCountable();
        //public static ZCountable FactoryInstance { get { return _instance; } }
        public static ZField HOTKEY_FIELD = ZField.getDummyField("hotkey", "int", "0");
        private Dictionary<ZField, ZFieldValue> countableFields;
        private int _a;
        private int _b;
        //private static ZCountable readFromDB(int level, int id) {
        //    ZCountable c = new ZCountable();
        //    c.getExisting(level, id);
        //    return c;
        //}
        #endregion

        //private HierarchicalEntityFactory _factory;
        //protected override void readDelegate(SqlCeDataReader r) {
        //    _hotkey = Convert.ToChar(r["hotkey"]);
        //}

        //protected override void deleteDelegate() {
        //    _cache[Type.ID].Remove(ID);
        //}

        public int A
        {
            get
            {
                GetParamters();
                return _a;
            }
        }

        public int B
        {
            get {
                GetParamters();
                return _b;
            }
        }

        public void GetParamters()
        {
            countableFields = GetValues();

            foreach (ZFieldValue v in countableFields.Values)
            {
                if (v.Field.Name.Equals("A"))
                {
                    _a = Int32.Parse(v.ReadableValue.ToString());            
                } 
                else if (v.Field.Name.Equals("B"))
                {
                    _b = Int32.Parse(v.ReadableValue.ToString());
                }     
            }
        }

        private Keys _hotkey;

        public Keys Hotkey {
            get { return _hotkey; }
            set {
                if (_hotkey != value)
                {
                    _hotkey = value;
                    saveHotkey();
                }
            }
        }

        protected override void readDelegate(SqlCeDataReader r)
        {
            _hotkey = (Keys)(Convert.ToInt32(r["hotkey"]));
        }

        internal ZCountable(WorkspaceInternals ws, HierarchicalEntityFactory factory) : base("Countable", ws, factory) {
        }

        //protected override ZEntity fromFactoryReader(int type, SqlCeDataReader r) { return fromReader(type, r); }
        //protected static ZCountable fromReader(int type, SqlCeDataReader r) {
        //    int i = Convert.ToInt32(r["internalid"]);
        //    if (!_cache.ContainsKey(type))
        //        _cache[type] = new Dictionary<int, ZCountable>();
        //    if (!_cache[type].ContainsKey(i))
        //        _cache[type][i] = new ZCountable();
        //    ZCountable m = _cache[type][i];

        //    m.ID = i;
        //    m.Name = r["name"].ToString();
        //    m.ParentID = Convert.ToInt32(r["parent"]);
        //    m.TypeID = type;

        //    m._hotkey = Convert.ToChar(r["hotkey"]);

        //    return m;
        //}
        
        //protected override ZEntity factorize(int level, int parent) {
        //    ZCountable m = new ZCountable();
        //    m.getNew(level, parent, ", hotkey", new object[]{0});

        //    return m;
        //}

        public override String ToString() {
            return Name;
        }

        //public List<ZEntity> all() {
        //    List<ZEntity> retval = new List<ZEntity>();
        //    foreach (Dictionary<int, ZCountable> d in _cache.Values)
        //        foreach (ZCountable z in d.Values)
        //            retval.Add(z);
        //    return retval;
        //}

        /*protected override void SaveFieldsHook(Dictionary<ZField, ZFieldValue> fields)
        {
            foreach (ZField f in fields.Keys)
            {
                if (f.Name.Equals("hotkey"))
                    this.Hotkey = Convert.ToChar(fields[f].DatabaseValue);
            }
        }*/

        private void saveHotkey()
        {
            Dictionary<ZField, ZFieldValue> fields = new Dictionary<ZField,ZFieldValue>();
            fields.Add(ZCountable.HOTKEY_FIELD, new ZFieldValue(ZCountable.HOTKEY_FIELD, (int)_hotkey));
            SetValues(fields);
        }
    }
}
