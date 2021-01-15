using LGCNS.iPharmMES.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace 보령.UserControls
{
    public partial class CustomUIHeader : UserControl
    {
        public CustomUIHeader()
        {
            InitializeComponent();
        }

        #region Property
        private string _BatchNo;
        public string BatchNo
        {
            get { return _BatchNo; }
            set
            {
                _BatchNo = value;
                txtBatchNo.Text = _BatchNo;
            }
        }
        private string _ProductionOrderID;
        public string ProductionOrderID
        {
            get { return _ProductionOrderID; }
            set
            {
                _ProductionOrderID = value;
                txtOrderNo.Text = _ProductionOrderID;
            }
        }
        private string _ProcessName;
        public string ProcessName
        {
            get { return _ProcessName; }
            set
            {
                _ProcessName = value;
                txtProcessName.Text = _ProcessName;
            }
        }
        private string _WorkRoom;
        public string WorkRoom
        {
            get { return _WorkRoom; }
            set
            {
                _WorkRoom = value;
                txtWorkRoom.Text = _WorkRoom;
            }
        }
        #endregion

        #region Dependency Property   
        private CurrentOrderInfo _CurrentOrder;
        public CurrentOrderInfo CurrentOrder
        {
            get { return _CurrentOrder; }
            set
            {
                _CurrentOrder = value;
                if(_CurrentOrder != null)
                {
                    txtBatchNo.Text = _CurrentOrder.BatchNo;
                    txtOrderNo.Text = _CurrentOrder.OrderID;
                    txtProcessName.Text = _CurrentOrder.OrderProcessSegmentName;
                    txtWorkRoom.Text = AuthRepositoryViewModel.Instance.RoomID;
                }
            }
        }
        public static readonly DependencyProperty CurrentOrderProperty =
            DependencyProperty.Register("CurrentOrder", typeof(CurrentOrderInfo), typeof(CustomUIHeader), new PropertyMetadata(OnCurrentOrderProperty));
        private static void OnCurrentOrderProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomUIHeader parent = (CustomUIHeader)d;
            //값을 전달할 타겟지정
            parent.CurrentOrder = (CurrentOrderInfo)e.NewValue;
        }
        #endregion

    }
}
