﻿#pragma checksum "D:\Root\PJ_201804_보령제약_예산공장\보령\완제품\포장자재정산.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "841272B49E3F9F4EC1523377218EE93F"
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
    
    
    public partial class 포장자재정산 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal C1DataGrid gridPackingInfo;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%99%84%EC%A0%9C%ED%92%88/%ED%8F%AC%EC%9E%A5%EC%9" +
                        "E%90%EC%9E%AC%EC%A0%95%EC%82%B0.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.gridPackingInfo = ((C1DataGrid)(this.FindName("gridPackingInfo")));
            this.btnConfirm = ((System.Windows.Controls.Button)(this.FindName("btnConfirm")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
        }
    }
}

