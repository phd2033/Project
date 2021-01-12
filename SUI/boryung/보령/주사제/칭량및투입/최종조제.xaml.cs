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
using ShopFloorUI;
using System.ComponentModel;

namespace 보령
{
    [Description("주사용수를 투입하여 최종 조제액 조제")]
    public partial class 최종조제 : ShopFloorCustomWindow
    {
        public 최종조제()
        {
            InitializeComponent();
        }
        public override string TableTypeName
        {
            get { return "TABLE,최종조제"; }
        }
        private void btnCansel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (BusyIn.DataContext as 최종조제ViewModel).StopTimer();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                
        }
        private void txtScaleId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && BusyIn.DataContext is 최종조제ViewModel)
                    (BusyIn.DataContext as 최종조제ViewModel).ConnectScaleCommand.Execute(txtScaleId.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void txtBeakerId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && BusyIn.DataContext is 최종조제ViewModel)
                    (BusyIn.DataContext as 최종조제ViewModel).SetBeakerTareCommand.Execute(txtBeakerId.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
