using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_ActionPlans
    {
        public int ActionPlanID { get; set; }
        public string ActionPlanName { get; set; }
        public string ActionPlanDetails { get; set; }
        public string ActionPlanType { get; set; }
        public string ActionPlanStatus { get; set; }

        public mdl_ActionPlans(int actionPlanID, string actionPlanName, string actionPlanDetails, string actionPlanType, string actionPlanStatus)
        {
            ActionPlanID = actionPlanID;
            ActionPlanName = actionPlanName;
            ActionPlanDetails = actionPlanDetails;
            ActionPlanType = actionPlanType;
            ActionPlanStatus = actionPlanStatus;
        }
    }
}
