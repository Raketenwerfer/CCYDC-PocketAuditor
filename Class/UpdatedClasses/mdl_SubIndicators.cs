using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_SubIndicators
    {
        public int SubIndicatorID { get; set; }
        public string SubIndicator { get; set; }
        public string SubIndicatorType { get; set; }
        public string SubIndicatorStatus { get; set; }
        public double ScoreValue { get; set; }

        public mdl_SubIndicators(int subIndicatorID, string subIndicator, string subIndicatorType, string subIndicatorStatus, double scoreValue)
        {
            SubIndicatorID = subIndicatorID;
            SubIndicator = subIndicator;
            SubIndicatorType = subIndicatorType;
            SubIndicatorStatus = subIndicatorStatus;
            ScoreValue = scoreValue;
        }
    }
}
