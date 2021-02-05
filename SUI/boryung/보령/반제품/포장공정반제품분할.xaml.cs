using ShopFloorUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace 보령
{
    [Description("포장 공정에서 반제품 투입 전 분할 작업")]
    public partial class 포장공정반제품분할 : ShopFloorCustomWindow
    {
        public 포장공정반제품분할()
        {
            InitializeComponent();
        }
        public override string TableTypeName
        {
            get
            {
                return "TABLE,포장공정반제품분할";
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (availableWIPList.ItemsSource != null && availableWIPList.ItemsSource is BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection)
                {
                    BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection items = availableWIPList.ItemsSource as BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection;
                    foreach (BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA item in items)
                        item.ISSELECTED = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (availableWIPList.ItemsSource != null && availableWIPList.ItemsSource is BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection)
                {
                    BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection items = availableWIPList.ItemsSource as BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATACollection;
                    foreach (BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA item in items)
                        item.ISSELECTED = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void txtSrcVessel_GotFocus(object sender, RoutedEventArgs e)
        {
            (BusyIn.DataContext as 포장공정반제품분할ViewModel).SearchWIPCommandAsync.Execute((sender as TextBox).Text);
        }
        //private void txtSrcVessel_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if(e.Key == Key.Enter && sender is TextBox)
        //        (BusyIn.DataContext as 포장공정반제품분할ViewModel).SearchWIPCommandAsync.Execute((sender as TextBox).Text);
        //}
        private void txtEmptyVessel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox)
                (BusyIn.DataContext as 포장공정반제품분할ViewModel).CheckEMPTYVesselCommandAsync.Execute((sender as TextBox).Text);
        }
        private void availableWIPList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(sender is C1.Silverlight.DataGrid.C1DataGrid)
            {
                var temp = sender as C1.Silverlight.DataGrid.C1DataGrid;
                if (temp.SelectedItem != null && temp.SelectedItem is BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA)
                {
                    var curRow = (sender as C1.Silverlight.DataGrid.C1DataGrid).SelectedItem as BR_BRS_SEL_ProductionOrderOutputSubLot_OPSG_FERT.OUTDATA;
                    curRow.ISSELECTED = !curRow.ISSELECTED;                    
                    temp.SelectedItem = null;
                }                
            }
        }
    }
}
