using LGCNS.iPharmMES.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace 보령
{
    public partial class BR_BRS_SEL_Charging_Solvent_to_Dispense : BizActorRuleBase
    {
        public partial class OUTDATA
        {
            private ScaleWeight _INVQTY = new ScaleWeight();
            public ScaleWeight INVQTY
            {
                get { return _INVQTY; }
                set
                {
                    _INVQTY = value;
                    OnPropertyChanged("INVQTY");
                }
            }
            private ScaleWeight _CHGQTY = new ScaleWeight();
            public ScaleWeight CHGQTY
            {
                get { return _CHGQTY; }   
                set
                {
                    _CHGQTY = value;
                    OnPropertyChanged("CHGQTY");
                }                           
            }

        }
    }
}
