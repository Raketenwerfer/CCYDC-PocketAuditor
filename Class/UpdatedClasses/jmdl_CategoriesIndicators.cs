using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class jmdl_CategoriesIndicators
    {
        public int CategoryID_fk { get; set; }
        public int IndicatorID_fk { get; set; }

        public jmdl_CategoriesIndicators(int catIDfk, int indIDfk)
        {
            CategoryID_fk = catIDfk;
            IndicatorID_fk = indIDfk;
        }
    }
}
