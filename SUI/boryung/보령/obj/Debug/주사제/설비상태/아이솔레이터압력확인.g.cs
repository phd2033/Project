﻿#pragma checksum "D:\006.Project\Source\boryung\보령\주사제\설비상태\아이솔레이터압력확인.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DDE713A3128ED1E30D7F54B38340EB9A"
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
    
    
    public partial class 아이솔레이터압력확인 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Controls.BusyIndicator BusyIn;
        
        internal System.Windows.Controls.Grid MainSection;
        
        internal System.Windows.Controls.Grid Footer;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%A3%BC%EC%82%AC%EC%A0%9C/%EC%84%A4%EB%B9%84%EC%8" +
                        "3%81%ED%83%9C/%EC%95%84%EC%9D%B4%EC%86%94%EB%A0%88%EC%9D%B4%ED%84%B0%EC%95%95%EB" +
                        "%A0%A5%ED%99%95%EC%9D%B8.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.BusyIn = ((System.Windows.Controls.BusyIndicator)(this.FindName("BusyIn")));
            this.MainSection = ((System.Windows.Controls.Grid)(this.FindName("MainSection")));
            this.Footer = ((System.Windows.Controls.Grid)(this.FindName("Footer")));
            this.btnConfirm = ((System.Windows.Controls.Button)(this.FindName("btnConfirm")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
        }
    }
}

