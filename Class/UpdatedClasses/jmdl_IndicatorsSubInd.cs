using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class jmdl_IndicatorsSubInd
    {
        public int SubIndicatorID_fk { get; set; }
        public int IndicatorID_fk { get; set; }

        public jmdl_IndicatorsSubInd(int subIndIDfk, int indIDfk)
        {
            SubIndicatorID_fk = subIndIDfk;
            IndicatorID_fk = indIDfk;
        }
    }
}
