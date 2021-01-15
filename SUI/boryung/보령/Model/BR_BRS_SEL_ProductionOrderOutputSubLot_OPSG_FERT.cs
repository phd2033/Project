using LGCNS.iPharmMES.Common;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace 보령
{
    public partial class BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT : BizActorRuleBase
    {
        public partial class OUTDATA
        {
            private bool _ISSELECTED = false;
            public bool ISSELECTED
            {
                get { return _ISSELECTED; }
                set
                {
                    _ISSELECTED = value;
                    OnPropertyChanged("ISSELECTED");
                }
            }
            private ScaleWeight _TARE = new ScaleWeight();
            public ScaleWeight TARE
            {
                get { return _TARE; }
                set
                {
                    _TARE = value;
                    OnPropertyChanged("TARE");
                }
            }
            private ScaleWeight _GROSS = new ScaleWeight();
            public ScaleWeight GROSS
            {
                get { return _GROSS; }
                set
                {
                    _GROSS = value;
                    OnPropertyChanged("GROSS");
                }
            }
            private ScaleWeight _TOTAL = new ScaleWeight();
            public ScaleWeight TOTAL
            {
                get { return _TOTAL; }
                set
                {
                    _TOTAL = value;
                    OnPropertyChanged("TOTAL");
                }
            }
        }
    }
}
