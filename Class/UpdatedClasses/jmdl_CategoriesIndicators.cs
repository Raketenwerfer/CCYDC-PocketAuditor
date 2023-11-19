using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class jmdl_CategoriesIndicators
    {
        public int CategoryID { get; set; }
        public string CategoryTitle { get; set; }
        public int IndicatorID { get; set; }
        public string Indicator { get; set; }
        public int IndicatorNumber { get; set; }
        public string IndicatorType { get; set; }
        public double ScoreValue { get; set; }


        public jmdl_CategoriesIndicators(int catID, string catTitle, int indID, string indicator, int indNo,
            string indType, double scoreVal)
        {
            CategoryID = catID;
            CategoryTitle = catTitle;
            IndicatorID = indID;
            Indicator = indicator;
            IndicatorNumber = indNo;
            IndicatorType = indType;
            ScoreValue = scoreVal;
        }
    }
}
