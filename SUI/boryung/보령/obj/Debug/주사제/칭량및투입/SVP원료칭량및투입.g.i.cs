﻿#pragma checksum "D:\2.업무\1.보령제약\00.보령제약_SUI\보령\보령\주사제\칭량및투입\SVP원료칭량및투입.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "74C38F68BB421E4BE4F7030B9D8C7C5A"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class SVP원료칭량및투입 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Controls.BusyIndicator BusyIn;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button btnMTRLScan;
        
        internal System.Windows.Controls.TextBox tstStdQty;
        
        internal System.Windows.Controls.TextBox txtScaleValue;
        
        internal C1.Silverlight.DataGrid.C1DataGrid dgProductionOutput;
        
        internal System.Windows.Controls.Button btnCharging;
        
        internal System.Windows.Controls.Button btnConfirm;
        
        internal System.Windows.Controls.Button btnClose;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%A3%BC%EC%82%AC%EC%A0%9C/%EC%B9%AD%EB%9F%89%EB%B" +
                        "0%8F%ED%88%AC%EC%9E%85/SVP%EC%9B%90%EB%A3%8C%EC%B9%AD%EB%9F%89%EB%B0%8F%ED%88%AC" +
                        "%EC%9E%85.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.BusyIn = ((System.Windows.Controls.BusyIndicator)(this.FindName("BusyIn")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.btnMTRLScan = ((System.Windows.Controls.Button)(this.FindName("btnMTRLScan")));
            this.tstStdQty = ((System.Windows.Controls.TextBox)(this.FindName("tstStdQty")));
            this.txtScaleValue = ((System.Windows.Controls.TextBox)(this.FindName("txtScaleValue")));
            this.dgProductionOutput = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("dgProductionOutput")));
            this.btnCharging = ((System.Windows.Controls.Button)(this.FindName("btnCharging")));
            this.btnConfirm = ((System.Windows.Controls.Button)(this.FindName("btnConfirm")));
            this.btnClose = ((System.Windows.Controls.Button)(this.FindName("btnClose")));
        }
    }
}

