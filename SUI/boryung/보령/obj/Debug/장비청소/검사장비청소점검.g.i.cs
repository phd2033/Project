﻿#pragma checksum "D:\006.Project\Source\boryung\보령\장비청소\검사장비청소점검.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "73FB30BB53AD8912C011E446D38F49A0"
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
using 보령;


namespace 보령 {
    
    
    public partial class 검사장비청소점검 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal 보령.검사장비청소점검ViewModel ViewModel;
        
        internal System.Windows.DataTemplate CleanButtonTemplate;
        
        internal System.Windows.DataTemplate ChangeWorkRoomButtonTemplate;
        
        internal System.Windows.Controls.BusyIndicator BusyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid Body;
        
        internal C1.Silverlight.DataGrid.C1DataGrid EqptInfoGrid;
        
        internal System.Windows.Controls.Grid Footer;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%9E%A5%EB%B9%84%EC%B2%AD%EC%86%8C/%EA%B2%80%EC%8" +
                        "2%AC%EC%9E%A5%EB%B9%84%EC%B2%AD%EC%86%8C%EC%A0%90%EA%B2%80.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.ViewModel = ((보령.검사장비청소점검ViewModel)(this.FindName("ViewModel")));
            this.CleanButtonTemplate = ((System.Windows.DataTemplate)(this.FindName("CleanButtonTemplate")));
            this.ChangeWorkRoomButtonTemplate = ((System.Windows.DataTemplate)(this.FindName("ChangeWorkRoomButtonTemplate")));
            this.BusyIndicator = ((System.Windows.Controls.BusyIndicator)(this.FindName("BusyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Body = ((System.Windows.Controls.Grid)(this.FindName("Body")));
            this.EqptInfoGrid = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("EqptInfoGrid")));
            this.Footer = ((System.Windows.Controls.Grid)(this.FindName("Footer")));
            this.btnConfirm = ((System.Windows.Controls.Button)(this.FindName("btnConfirm")));
            this.btnCacel = ((System.Windows.Controls.Button)(this.FindName("btnCacel")));
        }
    }
}

