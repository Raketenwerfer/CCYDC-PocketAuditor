using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_APExternalResources
    {
        public string ExtRes_URL { get; set; }
        public int ActionPlanID_fk { get; set; }

        public mdl_APExternalResources(string extRes_URL, int actionPlanID_fk)
        {
            ExtRes_URL = extRes_URL;
            ActionPlanID_fk = actionPlanID_fk;
        }
    }
}
