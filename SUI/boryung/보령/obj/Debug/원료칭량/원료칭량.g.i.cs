﻿#pragma checksum "D:\Root\PJ_201804_보령제약_예산공장\보령\원료칭량\원료칭량.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "56FE80FFC3E3D8F60C7B837845EC0E50"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using LGCNS.iPharmMES.Common;
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
using 보령.UserControls;


namespace 보령 {
    
    
    public partial class 원료칭량 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Media.Animation.Storyboard StepMessageChangeAnimation;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal BusyIndicator busyIndicator;
        
        internal System.Windows.Controls.Grid grdDispContainer;
        
        internal System.Windows.Controls.TextBlock btnComponentSelect;
        
        internal System.Windows.Controls.TextBlock SourceBarcode;
        
        internal System.Windows.Controls.TextBlock tbCurrentWeight;
        
        internal System.Windows.Controls.Grid grdScaleGauge;
        
        internal 보령.UserControls.ScaleControlDW scaleControl1;
        
        internal System.Windows.Controls.TextBlock tbVessel;
        
        internal System.Windows.Controls.TextBlock tbTareWeight;
        
        internal System.Windows.Controls.Button btnReset;
        
        internal System.Windows.Controls.Button btnTare;
        
        internal System.Windows.Controls.Button btnScaleChange;
        
        internal System.Windows.Controls.Button btnTrust;
        
        internal LGCNS.iPharmMES.Common.iPharmAuthButton btnDispense;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%9B%90%EB%A3%8C%EC%B9%AD%EB%9F%89/%EC%9B%90%EB%A" +
                        "3%8C%EC%B9%AD%EB%9F%89.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.StepMessageChangeAnimation = ((System.Windows.Media.Animation.Storyboard)(this.FindName("StepMessageChangeAnimation")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.busyIndicator = ((BusyIndicator)(this.FindName("busyIndicator")));
            this.grdDispContainer = ((System.Windows.Controls.Grid)(this.FindName("grdDispContainer")));
            this.btnComponentSelect = ((System.Windows.Controls.TextBlock)(this.FindName("btnComponentSelect")));
            this.SourceBarcode = ((System.Windows.Controls.TextBlock)(this.FindName("SourceBarcode")));
            this.tbCurrentWeight = ((System.Windows.Controls.TextBlock)(this.FindName("tbCurrentWeight")));
            this.grdScaleGauge = ((System.Windows.Controls.Grid)(this.FindName("grdScaleGauge")));
            this.scaleControl1 = ((보령.UserControls.ScaleControlDW)(this.FindName("scaleControl1")));
            this.tbVessel = ((System.Windows.Controls.TextBlock)(this.FindName("tbVessel")));
            this.tbTareWeight = ((System.Windows.Controls.TextBlock)(this.FindName("tbTareWeight")));
            this.btnReset = ((System.Windows.Controls.Button)(this.FindName("btnReset")));
            this.btnTare = ((System.Windows.Controls.Button)(this.FindName("btnTare")));
            this.btnScaleChange = ((System.Windows.Controls.Button)(this.FindName("btnScaleChange")));
            this.btnTrust = ((System.Windows.Controls.Button)(this.FindName("btnTrust")));
            this.btnDispense = ((LGCNS.iPharmMES.Common.iPharmAuthButton)(this.FindName("btnDispense")));
        }
    }
}

