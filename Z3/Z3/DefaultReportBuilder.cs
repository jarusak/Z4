using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Z3.Model;
using System.Data.Common;
using System.Diagnostics;

namespace Z3.Util
{
    class DefaultReportBuilder
    {
        public DefaultReportBuilder(Workspace.IWorkspace context)
        {
            rootContainer = context.Levels.getRootType("Container");
            rootCountable = context.Levels.getRootType("Countable");

            dataSetFields = getFields(rootContainer);
            countableFields = getFields(rootCountable);

            this.context = context;
        }

        private Workspace.IWorkspace context;
        private ZLevel rootContainer, rootCountable;
        private List<ZField> dataSetFields, countableFields;

        public String GetHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("IndividualID, ");
            foreach (ZField s in dataSetFields)
            {
                sb.Append(s.Level.Name);
                sb.Append(".");
                sb.Append(s.Label);
                sb.Append(", ");
            }
            foreach (ZField s in countableFields)
            {
                sb.Append(s.Label);
                sb.Append(", ");
            }
            sb.Append("Measurement, Value, Weight, Comments");
            return sb.ToString();
        }

        public List<string> BuildReportQueries() {
            List<String> retval = new List<string>();

            ZLevel i = rootContainer;
            while (i != null)
            {
                String iFields = getFieldString(rootContainer, i.Child, dataSetFields.Count);

                ZLevel j = rootCountable;
                while (j != null)
                {
                    String jFields = getFieldString(rootCountable, j.Child, countableFields.Count);

                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT indiv.internalid, \r\n");
                    query.Append(iFields);
                    query.Append(jFields);
                    query.Append("mtype.name, \r\nm.value, \r\nm.weight, \r\nindiv.Comments \r\n");

                    query.Append("FROM Z3Individuals indiv \r\n");
                    query.Append(getJoins(i, "container"));
                    query.Append(getJoins(j, "countable"));
                    query.Append("inner join Z3Measurements m on m.individual = indiv.internalid \r\n");
                    query.Append("inner join Z3MeasurementTypes mtype on mtype.internalid = m.measurement \r\n");
                    
                    query.Append("WHERE (indiv.CountableLevel = ");
                    query.Append(j.ID);
                    query.Append(") AND (indiv.ContainerLevel = ");
                    query.Append(i.ID);
                    query.Append(")\r\n");

                    retval.Add(query.ToString());

                    j = j.Child;
                }
                i = i.Child;
            }
            return retval;
        }

        private static String getJoins(ZLevel current, string type)
        {
            StringBuilder retval = new StringBuilder();
            retval.Append("inner join ");
            retval.Append(current.Table);
            retval.Append(" on indiv.");
            retval.Append(type);
            retval.Append(" = ");
            retval.Append(current.Table);
            retval.Append(".internalid \r\n");
            while (!current.Root)
            {
                retval.Append("inner join ");
                retval.Append(current.Parent.Table);
                retval.Append(" on ");
                retval.Append(current.Table);
                retval.Append(".parent = ");
                retval.Append(current.Parent.Table);
                retval.Append(".internalid \r\n");

                current = current.Parent;
            }
            return retval.ToString();
        }

        private static String getFieldString(ZLevel root, ZLevel current, int pad)
        {
            List<ZField> fields = getFields(root, current);
            StringBuilder retval = new StringBuilder();

            foreach (ZField f in fields)
            {
                retval.Append(f.Level.Table);
                retval.Append('.');
                retval.Append(f.Name);
                retval.Append(", \r\n");
            }

            for (int i = 0; i < (pad - fields.Count); i++)
            {
                retval.Append("\'n.a.\', \r\n");
            }

            return retval.ToString();
        }

        private static List<ZField> getFields(ZLevel root)
        {
            return getFields(root, null);
        }
        
        private static List<ZField> getFields(ZLevel root, ZLevel end)
        {
            List<ZField> retval = new List<ZField>();
            while (root != end)
            {
                ZField temp = new ZField();
                temp.Label = root.Name + "ID";
                temp.Level = root;
                temp.Name = "internalid";
                retval.Add(temp);
                
                foreach (ZField f in root.Fields)
                {
                    retval.Add(f);
                }

                root = root.Child;
            }

            return retval;
        }
    }
}
