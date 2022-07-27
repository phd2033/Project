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
using System.Windows.Navigation;
using ShopFloorUI;
using System.ComponentModel;

namespace 보령
{
    [Description("비정량적시험")]
    public partial class 비정량적시험 : ShopFloorCustomWindow
    {
        public 비정량적시험()
        {
            InitializeComponent();
        }
        public override string TableTypeName
        {
            get { return "TABLE,비정량적시험"; }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
