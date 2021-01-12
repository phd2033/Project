﻿using System;
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

namespace 보령
{
    public partial class 반제품무게측정 : ShopFloorCustomWindow
    {
        public 반제품무게측정()
        {
            InitializeComponent();
        }

        public override string TableTypeName
        {
            get { return "TABLE,반제품무게측정"; }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void txtVesselId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((반제품무게측정ViewModel)LayoutRoot.DataContext).WeighingCommandAsync.Execute(txtVesselId.Text);
            }
        }
    }
}
