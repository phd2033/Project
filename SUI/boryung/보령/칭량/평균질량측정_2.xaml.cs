﻿using ShopFloorUI;
using System.Windows.Controls;
using System.Linq;
using LGCNS.EZMES.ControlsLib;
using System.Collections.Generic;
using LGCNS.iPharmMES.Common;
using System.Windows.Media;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;

namespace 보령
{
    [Description("공정검사(평균질량) 측정 (수율계산시 반영 안 됨)")]
    public partial class 평균질량측정_2 : ShopFloorCustomWindow
    {
        public 평균질량측정_2()
        {
            InitializeComponent();
        }

        public override string TableTypeName
        {
            get { return "TABLE,평균질량측정"; }
        }

        private void txtSample_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(txtSample != null)
                (this.LayoutRoot.DataContext as 평균질량측정_2ViewModel).sampleCount = txtSample.Value.ToString();
        }
    }
}
