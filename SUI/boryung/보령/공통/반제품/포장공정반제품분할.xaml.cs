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
    [ShopFloorCustomHidden]
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
                if (SplitResultList.ItemsSource != null && SplitResultList.ItemsSource is BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection)
                {
                    BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection items = SplitResultList.ItemsSource as BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection;
                    foreach (BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATA item in items)
                        //item.ISSELECTED = true;

                    SplitResultList.Refresh();
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
                if (SplitResultList.ItemsSource != null && SplitResultList.ItemsSource is BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection)
                {
                    BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection items = SplitResultList.ItemsSource as BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATACollection;
                    foreach (BR_BRS_REG_MaterialSubLot_INV_Split.OUTDATA item in items)
                        //item.ISSELECTED = false;

                    SplitResultList.Refresh();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
