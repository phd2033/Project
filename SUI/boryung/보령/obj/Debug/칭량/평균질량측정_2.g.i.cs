﻿#pragma checksum "D:\Root\PJ_201804_보령제약_예산공장\보령\칭량\평균질량측정_2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8A5EE4E9C8B9368A573A39FC5FCD64DC"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using LGCNS.EZMES.ControlsLib;
using ShopFloorUI;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace 보령 {
    
    
    public partial class 평균질량측정_2 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtEQPTID;
        
        internal System.Windows.Controls.NumericUpDown txtSample;
        
        internal LGCNS.EZMES.ControlsLib.CNSDataGrid dgWeighing;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%B9%AD%EB%9F%89/%ED%8F%89%EA%B7%A0%EC%A7%88%EB%9" +
                        "F%89%EC%B8%A1%EC%A0%95_2.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtEQPTID = ((System.Windows.Controls.TextBox)(this.FindName("txtEQPTID")));
            this.txtSample = ((System.Windows.Controls.NumericUpDown)(this.FindName("txtSample")));
            this.dgWeighing = ((LGCNS.EZMES.ControlsLib.CNSDataGrid)(this.FindName("dgWeighing")));
        }
    }
}

