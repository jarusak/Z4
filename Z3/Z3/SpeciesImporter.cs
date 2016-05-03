using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Z3.Model;
using System.Data.SqlServerCe;
using Z3.Workspace;

namespace Z3.Util
{
    class SpeciesImporter
    {
        private WorkspaceInternals _ws;
        public SpeciesImporter(WorkspaceInternals ws)
        {
            _ws = ws;
        }

        public void import(String filename) {
            String[] vals, tables, fields;
            
            using (StreamReader sr = new StreamReader(filename))
            {
                // Identify column headers
                String line = sr.ReadLine();
                if (line == null) throw new InvalidDataException();
                vals = line.Split(',');
                tables = new String[vals.Length];
                fields = new String[vals.Length];
                for (int i = 0; i < vals.Length; i++)
                {
                    tables[i] = vals[i].Split('.')[0];
                    fields[i] = vals[i].Split('.')[1];
                }

                SpeciesImportPlan plan = createPlan(tables, fields);

                while ((line = sr.ReadLine()) != null)
                {
                    String[] values = line.Split(',');
                    applyPlan(plan, values);
                }
            }
        }

        public SpeciesImportPlan createPlan(String[] tables, String[] fields)
        {
            ICollection<ZLevel> levels = _ws.Levels.Types("Countable");
            SpeciesImportPlan plan = new SpeciesImportPlan(null);
            SpeciesImportPlan current = plan;
            foreach (ZLevel l in levels)
            {
                current.next = new SpeciesImportPlan(l);
                current = current.next;

                for (int i = 0; i < tables.Length; i++)
                {
                    if (l.Name.Equals(tables[i], StringComparison.InvariantCultureIgnoreCase))
                    {
                        foreach (ZField f in l.Fields) {
                            if (f.Name.Equals(fields[i], StringComparison.InvariantCultureIgnoreCase)) {
                                current.fields.Add(new SpeciesImportPlanField(f, i));
                                break;
                            }
                        }
                    }
                }
            }
            return plan.next;
        }

        public void applyPlan(SpeciesImportPlan plan, String[] values)
        {
            ZCountable parent = null;
            for (SpeciesImportPlan p = plan; p != null; p = p.next)
            {
                ZCountable c = findSpecimen(p, values, parent);
                parent = c;
            }
        }

        public ZCountable findSpecimen(SpeciesImportPlan current, String[] values, ZCountable parent)
        {
            return findSpecimenHelper(current, values, parent, true);
        }

        public ZCountable findSpecimenHelper(SpeciesImportPlan current, String[] values, ZCountable parent, bool create) {
            if (current.fields.Count == 0)
            {
                throw new InvalidDataException("No fields given for the " + current.level.Name + " level");
            }
            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "select * from " + current.level.Name + "Countable where ";
                for (int i = 0; i < current.fields.Count; i++)
                {
                    if (i != 0) cmd.CommandText += " and ";
                    cmd.CommandText += current.fields[i].field.Name + "='" + values[current.fields[i].index] + "'";
                }
                if (parent != null)
                {
                    cmd.CommandText += " and parent='" + parent.ID + "'";
                }
                using (SqlCeDataReader r = cmd.ExecuteReader())
                {

                    if (r.Read())  // Found the item we were supposed to Import
                    {
                        ZCountable retval = _ws.CountableStore.FromReader(current.level.ID, r);
                        
                        if (r.Read())  // Found more than one!
                        {
                            throw new InvalidDataException("Found multiple results for query " + cmd.CommandText);
                        }
                        else
                        {
                            return retval;
                        }
                    }
                    else  // Did not find
                    {
                        if (create)  // Must import
                        {
                            return createSpecimen(current, values, parent);
                        }
                        else  // Well, that's odd, we should have found one
                        {
                            throw new InvalidDataException("Could not find record after creating; query was: " + cmd.CommandText);
                        }
                    }
                }
            }
        }

        public ZCountable createSpecimen(SpeciesImportPlan current, String[] values, ZCountable parent)
        {
            if (current.fields.Count == 0)
            {
                throw new InvalidDataException("No fields given for the " + current.level.Name + " level");
            }
            ZCountable newSpecies = _ws.CountableStore.Create(parent);
            Dictionary<ZField, ZFieldValue> importValues = new Dictionary<ZField, ZFieldValue>();
            foreach (SpeciesImportPlanField f in current.fields) {
                importValues.Add(f.field, new ZFieldValue(f.field, values[f.index]));
            }
            newSpecies.SetValues(importValues);
            return newSpecies;

            /*
            using (SqlCeCommand cmd = _ws.Connection.CreateCommand())
            {
                cmd.CommandText = "insert into " + current.level.Name + "Countable ( ";
                for (int i = 0; i < current.fields.Count; i++)
                {
                    if (i != 0) cmd.CommandText += ", ";
                    cmd.CommandText += current.fields[i].field.Name;
                }
                cmd.CommandText += ", parent ) values ( ";
                for (int i = 0; i < current.fields.Count; i++)
                {
                    if (i != 0) cmd.CommandText += ", ";
                    cmd.CommandText += "'" + values[current.fields[i].index] + "'";
                }
                cmd.CommandText += ", " + parent + " )";

                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new InvalidDataException("Could not create a new " + current.level.Name + " with:\n" + cmd.CommandText);
                }
                else return _ws.CountableStore.ByID(current.level, _ws.getIdentity());
             
            }*/
        }
    }

    internal class SpeciesImportPlan
    {
        public SpeciesImportPlan(ZLevel l) { level = l; }
        public SpeciesImportPlan next;
        public ZLevel level;
        public List<SpeciesImportPlanField> fields = new List<SpeciesImportPlanField>();
    }

    internal class SpeciesImportPlanField
    {
        public SpeciesImportPlanField(ZField f, int i) { field = f; index = i; }
        public ZField field;
        public int index;
    }
}
