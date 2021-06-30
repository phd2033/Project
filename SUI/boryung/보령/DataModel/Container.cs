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
    public abstract class Container : BizActorDataSetBase
    {
        private string _MsubLotId;
        public string MsubLotId
        {
            get { return _MsubLotId; }
            set
            {
                _MsubLotId = value;
                OnPropertyChanged("MsubLotId");
            }
        }
        private string _Barcode;
        public string Barcode
        {
            get { return _Barcode; }
            set
            {
                _Barcode = value;
                OnPropertyChanged("Barcode");
            }
        }
        private string _VesselId;
        public string VesselId
        {
            get { return _VesselId; }
            set
            {
                _VesselId = value;
                OnPropertyChanged("VesselId");
            }
        }
        private int _Precision;
        public int Precision
        {
            get { return _Precision; }
            set
            {
                _Precision = value;
                OnPropertyChanged("Precision");
            }
        }
        private string _Uom;
        public string Uom
        {
            get { return _Uom; }
            set
            {
                _Uom = value;
                OnPropertyChanged("Uom");
            }
        }
        private decimal _NetWeight;
        public decimal NetWeight
        {
            get { return _NetWeight; }
            set
            {
                _NetWeight = value;
                OnPropertyChanged("NetWeight");
            }
        }
        private decimal _TareWeight;
        public decimal TareWeight
        {
            get { return _TareWeight; }
            set
            {
                _TareWeight = value;
                OnPropertyChanged("TareWeight");
            }
        }
        public decimal GrossWeight
        {
            get { return _NetWeight + _TareWeight; }
        }

    }

    public abstract class WIPContainer : Container
    {
        private string _PoId;
        public string PoId
        {
            get { return _PoId; }
            set
            {
                _PoId = value;
                OnPropertyChanged("PoId");
            }
        }
        private string _OpsgGuid;
        public string OpsgGuid
        {
            get { return _OpsgGuid; }
            set
            {
                _OpsgGuid = value;
                OnPropertyChanged("OpsgGuid");
            }
        }
    }
}
