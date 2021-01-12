using LGCNS.iPharmMES.Common;
using ShopFloorUI;
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

namespace 보령
{
    public partial class 정제체크마스터조회 : ShopFloorCustomWindow
    {
        public 정제체크마스터조회()
        {
            InitializeComponent();
        }

        public override string TableTypeName
        {
            get { return "TABLE,정제체크마스터조회"; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
        }

        //private void grid_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    BR_BRS_GET_Selector_Check_Master.OUTDATA row = e.Row.DataContext as BR_BRS_GET_Selector_Check_Master.OUTDATA;

        //    if (row.FLAG.Equals("NG"))
        //        e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 221, 0, 0));
            
        //    else if (row.FLAG.Equals("OK"))
        //    {
        //        if(Int32.Parse(row.RowIndex)%2 == 0)
        //            e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 241, 244, 248));
        //        else
        //            e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        //    }
            

        //}
    }
}
