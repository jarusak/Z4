using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Text;
using Z3.Leger;

namespace Z3.Model{
    public class Z3DataPoint {
        private int _individualID;

        public int IndividualID {
            get { return _individualID; }
            set { _individualID = value; }
        }
        private int _id;

        public int ID {
            get { return _id; }
            set { _id = value; }
        }
        private int _mtype;

        public int Mtype {
            get { return _mtype; }
            set { _mtype = value; }
        }
        private double _value;

        public double Value {
            get { return _value; }
            set { _value = value; }
        }

        internal static Z3DataPoint insert(int m, int dslvl, int dsid, int splvl, int spid, double value) {

            Z3DataPoint d = new Z3DataPoint();

            d._individualID = ZIndividual.createNew(dslvl, dsid, splvl, spid, "");
            
            using (SqlCeCommand cmd = Z3Schema.CreateCommand()) {
                cmd.CommandText = "insert into Z3Measurements (Individual, Measurement, Value) values(" +
                d._individualID + ", " + m + ", " + value + ")";

                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Could not insert measurement record");
            }
            d._id = Z3Schema.getInstance().getIdentity();
            d._value = value;
            d._mtype = m;
            return d;
        }
    }
}
