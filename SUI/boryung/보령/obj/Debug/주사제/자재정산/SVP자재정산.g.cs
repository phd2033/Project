﻿#pragma checksum "D:\006.Project\Source\boryung\보령\주사제\자재정산\SVP자재정산.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4AE7E1D3D22A5BADF8A0370B2ACA0DB0"
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
    
    
    public partial class SVP자재정산 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal C1.Silverlight.DataGrid.C1DataGrid gridPackingInfo;
        
        internal System.Windows.Controls.Button btnConfirm;
        
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%A3%BC%EC%82%AC%EC%A0%9C/%EC%9E%90%EC%9E%AC%EC%A" +
                        "0%95%EC%82%B0/SVP%EC%9E%90%EC%9E%AC%EC%A0%95%EC%82%B0.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.gridPackingInfo = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("gridPackingInfo")));
            this.btnConfirm = ((System.Windows.Controls.Button)(this.FindName("btnConfirm")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
        }
    }
}

