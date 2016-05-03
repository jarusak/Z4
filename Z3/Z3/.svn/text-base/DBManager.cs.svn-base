using System;
using System.Collections.Generic;
using System.Text;
using Z3.Model;
using System.Windows.Forms;

namespace Z3
{
    public interface DataSetStore
    {
        ZDataSet createObject();
        ZDataSet loadObject(int level, int id);
        ZDataSet saveObject(ZDataSet set);

        ZLevel createClass();
        ZLevel loadClass(int level);
        ZLevel saveClass(ZLevel cls);

        event EventHandler<EntityEventArgs> EntityChanged;
        event EventHandler<EntityEventArgs> EntityDeleted;
        event EventHandler<EntityEventArgs> EntityCreated;
    }

    public interface Entity
    {
        void Delete();
        Dictionary<ZField, ZFieldValue> GetValues();
        Dictionary<ZField, ZFieldValue> SetValues();

        event EventHandler<EntityEventArgs> Changed;
        event EventHandler<EntityEventArgs> Deleted;
    }

    public interface Level
    {
        Level Child { get; set; }
        int ChildID { get; set; }
        List<ZField> Fields { get; set; }
        bool Final { get; set; }
        int ID { get; set; }
        bool Measurable { get; set; }
        string Name { get; set; }
        Level Parent { get; set; }
        int ParentID { get; set; }
        bool Root { get; set; }
        void Delete();
    }

    public class EntityEventArgs : EventArgs {
        private Entity _topic;

        public Entity Topic
        {
            get { return _topic; }
            set { _topic = value; }
        }
    }
}

