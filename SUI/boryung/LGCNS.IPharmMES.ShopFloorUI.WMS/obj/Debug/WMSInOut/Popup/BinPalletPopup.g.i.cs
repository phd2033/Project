﻿#pragma checksum "D:\Root\iPharmMES_ShopFloorUI\LGCNS.IPharmMES.ShopFloorUI.WMS\WMSInOut\Popup\BinPalletPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "35E27A27DB460C77E4D95933EE0C8263"
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


namespace WMS {
    
    
    public partial class BinPalletPopup : LGCNS.iPharmMES.Common.iPharmMESChildWindow {
        
        internal LGCNS.iPharmMES.Common.iPharmMESChildWindow Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtIBCIDp;
        
        internal System.Windows.Controls.TextBox txtBintType;
        
        internal System.Windows.Controls.TextBox txtWSDTTM;
        
        internal System.Windows.Controls.TextBox txtBintWt;
        
        internal System.Windows.Controls.Button LabelPrint;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.Button CancelButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WMS;component/WMSInOut/Popup/BinPalletPopup.xaml", System.UriKind.Relative));
            this.Main = ((LGCNS.iPharmMES.Common.iPharmMESChildWindow)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtIBCIDp = ((System.Windows.Controls.TextBox)(this.FindName("txtIBCIDp")));
            this.txtBintType = ((System.Windows.Controls.TextBox)(this.FindName("txtBintType")));
            this.txtWSDTTM = ((System.Windows.Controls.TextBox)(this.FindName("txtWSDTTM")));
            this.txtBintWt = ((System.Windows.Controls.TextBox)(this.FindName("txtBintWt")));
            this.LabelPrint = ((System.Windows.Controls.Button)(this.FindName("LabelPrint")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
        }
    }
}

