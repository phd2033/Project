﻿#pragma checksum "D:\2.업무\1.보령제약\00.보령제약_SUI\보령\보령\공통\자재확인\불출라벨발행.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FAEE23CD4B04730471BCB63A05CD92C6"
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
    
    
    public partial class 불출라벨발행 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid Header;
        
        internal System.Windows.Controls.Grid MainSection;
        
        internal C1.Silverlight.DataGrid.C1DataGrid dgMaterials;
        
        internal System.Windows.Controls.Grid PrinterSection;
        
        internal System.Windows.Controls.TextBox txtPrinter;
        
        internal C1.Silverlight.DataGrid.C1DataGrid PrinterList;
        
        internal System.Windows.Controls.Grid Footer;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.Button btnConfirm;
        
        internal System.Windows.Controls.Button btnCacel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EA%B3%B5%ED%86%B5/%EC%9E%90%EC%9E%AC%ED%99%95%EC%9" +
                        "D%B8/%EB%B6%88%EC%B6%9C%EB%9D%BC%EB%B2%A8%EB%B0%9C%ED%96%89.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Header = ((System.Windows.Controls.Grid)(this.FindName("Header")));
            this.MainSection = ((System.Windows.Controls.Grid)(this.FindName("MainSection")));
            this.dgMaterials = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("dgMaterials")));
            this.PrinterSection = ((System.Windows.Controls.Grid)(this.FindName("PrinterSection")));
            this.txtPrinter = ((System.Windows.Controls.TextBox)(this.FindName("txtPrinter")));
            this.PrinterList = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("PrinterList")));
            this.Footer = ((System.Windows.Controls.Grid)(this.FindName("Footer")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.btnConfirm = ((System.Windows.Controls.Button)(this.FindName("btnConfirm")));
            this.btnCacel = ((System.Windows.Controls.Button)(this.FindName("btnCacel")));
        }
    }
}

