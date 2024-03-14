using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class jmdl_IndicatorSubCat
    {
        public int IndicatorID_fk { get; set; }
        public string Indicator { get; set; }
        public int CategoryID_fk { get; set; }
        public int SubCategoryID_fk { get; set; }
        public string SubCategoryTitle { get; set; }
        public string SubCategoryStatus { get; set; }


        public jmdl_IndicatorSubCat(int indIDfk, string ind, int catIDfk, int SubCatIDfk,
            string SubCat, string SubCatStatus)
        {
            IndicatorID_fk = indIDfk;
            Indicator = ind;
            CategoryID_fk = catIDfk;
            SubCategoryID_fk = SubCatIDfk;
            SubCategoryTitle = SubCat;
            SubCategoryStatus = SubCatStatus;
        }
    }

}
