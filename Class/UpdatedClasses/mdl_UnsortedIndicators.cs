using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_UnsortedIndicators
    {
        public int IndicatorID { get; set; }
        public double ScoreValue { get; set; }
        public string Indicator { get; set; }
        public string IndicatorStatus { get; set; }
        public int CategoryID { get; set; }

        public mdl_UnsortedIndicators(int indicatorID, double scoreValue, string indicator,
            string indicatorStatus, int categoryID)
        {
            IndicatorID = indicatorID;
            ScoreValue = scoreValue;
            Indicator = indicator;
            IndicatorStatus = indicatorStatus;
            CategoryID = categoryID;
        }
    }
}
