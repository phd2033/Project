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
            private string PRECISION = "N0";

            public string TOTALWEIGHT
            {
                get
                {
                    if(_MSUBLOTQTY.HasValue && _TAREWEIGHT.HasValue)
                        return (_MSUBLOTQTY.Value + _TAREWEIGHT.Value).ToString(PRECISION);
                    else
                        return (-999999).ToString(PRECISION);
                }
            }
            public string GROSSWEIGHT
            {
                set
                {
                    decimal chk;
                    if (decimal.TryParse(value, out chk))
                        _MSUBLOTQTY = chk;
                }
                get
                {
                    if (_MSUBLOTQTY.HasValue)
                        return (_MSUBLOTQTY.Value).ToString(PRECISION);
                    else
                        return (-999999).ToString(PRECISION);
                }
            }
            public string VESSELWEIGHT
            {
                set
                {
                    decimal chk;
                    if (decimal.TryParse(value, out chk))
                        _TAREWEIGHT = chk;
                }
                get
                {
                    if (_TAREWEIGHT.HasValue)
                        return (_TAREWEIGHT.Value).ToString(PRECISION);
                    else
                        return (-999999).ToString(PRECISION);
                }
            }           
        }
    }
}
