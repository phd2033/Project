﻿#pragma checksum "D:\Root\iPharmMES_ShopFloorUI\LGCNS.IPharmMES.ShopFloorUI.WMS\WMSInOut\Popup\MaterialPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E7974EB9617A273F5AF636101728212B"
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
    
    
    public partial class MaterialPopup : LGCNS.iPharmMES.Common.iPharmMESChildWindow {
        
        internal LGCNS.iPharmMES.Common.iPharmMESChildWindow Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtProductCode;
        
        internal System.Windows.Controls.TextBox txtProdName;
        
        internal System.Windows.Controls.TextBox txtOrderNo;
        
        internal System.Windows.Controls.TextBox txtBatchoNop;
        
        internal System.Windows.Controls.TextBox txtOp;
        
        internal System.Windows.Controls.TextBox txtProductDate;
        
        internal System.Windows.Controls.TextBox txtIBCIDp;
        
        internal System.Windows.Controls.TextBox txtTotalWt;
        
        internal System.Windows.Controls.TextBox txtBintWt;
        
        internal System.Windows.Controls.TextBox txtGoodWt;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WMS;component/WMSInOut/Popup/MaterialPopup.xaml", System.UriKind.Relative));
            this.Main = ((LGCNS.iPharmMES.Common.iPharmMESChildWindow)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtProductCode = ((System.Windows.Controls.TextBox)(this.FindName("txtProductCode")));
            this.txtProdName = ((System.Windows.Controls.TextBox)(this.FindName("txtProdName")));
            this.txtOrderNo = ((System.Windows.Controls.TextBox)(this.FindName("txtOrderNo")));
            this.txtBatchoNop = ((System.Windows.Controls.TextBox)(this.FindName("txtBatchoNop")));
            this.txtOp = ((System.Windows.Controls.TextBox)(this.FindName("txtOp")));
            this.txtProductDate = ((System.Windows.Controls.TextBox)(this.FindName("txtProductDate")));
            this.txtIBCIDp = ((System.Windows.Controls.TextBox)(this.FindName("txtIBCIDp")));
            this.txtTotalWt = ((System.Windows.Controls.TextBox)(this.FindName("txtTotalWt")));
            this.txtBintWt = ((System.Windows.Controls.TextBox)(this.FindName("txtBintWt")));
            this.txtGoodWt = ((System.Windows.Controls.TextBox)(this.FindName("txtGoodWt")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
        }
    }
}

