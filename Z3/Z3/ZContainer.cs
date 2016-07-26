using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using Z3.Workspace;

namespace Z3.Model {
    public  class ZDataSet : BasicHierarchicalEntity {
        internal ZDataSet(WorkspaceInternals workspace, HierarchicalEntityFactory factory) : base("Container", workspace, factory) {
        }

        //private static ZContainer _instance = new ZContainer();
        //public static ZContainer FactoryInstance { get { return _instance; } }

        //private static Dictionary<int, Dictionary<int, ZContainer>> _cache = new Dictionary<int, Dictionary<int, ZContainer>>();

        //public static ZContainer GetContainer(int level, int id) {
        //    if (!_cache.ContainsKey(level))
        //        _cache[level] = new Dictionary<int, ZContainer>();
        //    if (!_cache[level].ContainsKey(id))
        //        _cache[level][id] = readFromDB(level, id);
        //    return _cache[level][id];
        //}

        //protected override ZEntity fromFactoryReader(int type, SqlCeDataReader r) { return fromReader(type, r); }
        //public static ZContainer fromReader(int type, SqlCeDataReader r) {
        /*    int i = Convert.ToInt32(r["internalid"]);
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
        }*/
        /*private static ZContainer readFromDB(int level, int id) {
            ZContainer c = new ZContainer();
            c.getExisting(level, id);
            return c;
        }*/

        //protected override void readDelegate(System.Data.SqlServerCe.SqlCeDataReader r) {
            
        //}

        /*protected override void deleteDelegate() {
            if (_cache.ContainsKey(Type.ID))
                _cache[Type.ID].Remove(ID);
        }

        protected override ZEntity factorize(int level, int parent) {
            ZContainer m = new ZContainer();
            m.getNew(level, parent, "", new object[] {});

            return m;
        }*/

        /*private List<ZEntity> all() {
            List<ZEntity> retval = new List<ZEntity>();
            foreach (Dictionary<int, ZContainer> d in _cache.Values)
                foreach (ZContainer z in d.Values)
                    retval.Add(z);
            return retval;
        }*/

        protected override void readDelegate(SqlCeDataReader r)
        {
            
        }

        public override string ToString() {
            return Name;
        }

        public List<ZIndividual> Individuals
        {
            get
            {
                List<ZIndividual> _indivs = new List<ZIndividual>();

                using (SqlCeCommand cmd = Workspace.Connection.CreateCommand())
                {
                    cmd.CommandText =
                        "select * from z3individuals" +
                        " where Containerlevel=" + TypeID +
                        " and Container=" + ID;
                    using (SqlCeDataReader r = cmd.ExecuteReader())
                        while (r.Read())
                            _indivs.Add(Workspace.Individuals.fromReader(r));
                }

                return _indivs;
            }
        }

        public ZIndividual AddIndividual(ZCountable species, ZDataSet dataset)
        {
            if (!species.Type.Measurable)
            {
                throw new OperationCanceledException("This schema does not allow you to count a " + species.Type.Name);
            }

            if (!Type.Measurable)
            {
                throw new OperationCanceledException("This schema does not allow you to record counts into a " + Type.Name);
            }

            return Workspace.Individuals.insert(dataset, species, "");
        }


        public void hasBeenModified()
        {
            base.wasModified();
        }
    }
}
