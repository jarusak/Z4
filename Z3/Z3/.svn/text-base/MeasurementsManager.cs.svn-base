using System;
using System.Collections.Generic;
using System.Text;

namespace Z3.View
{
    class MeasurementsManager
    {
        //TODO everything
        internal void SetItems(List<Z3.Model.Z3Measurement> types)
        {
            measurementCombo.Items.Clear();
            foreach (Z3Measurement m in types)
            {
                measurementCombo.Items.Add(m);
            }
        }

        internal void EnableData(bool p)
        {
            measurementCombo.Enabled = p;
        }
    }
}
