﻿#pragma checksum "D:\006.Project\Source\boryung\보령\원료칭량\Popup\TrustScaleWeightPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FF904B200A22B7D5D8ECB88F175F7047"
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
using 보령.UserControls;


namespace 보령 {
    
    
    public partial class TrustScaleWeightPopup : LGCNS.iPharmMES.Common.iPharmMESChildWindow {
        
        internal LGCNS.iPharmMES.Common.iPharmMESChildWindow rootWindow;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtWeight;
        
        internal System.Windows.Controls.Button btnKeyTrPad;
        
        internal 보령.UserControls.BarcodeTextBox txtBarcode;
        
        internal System.Windows.Controls.TextBlock tbCurrentWeight;
        
        internal System.Windows.Controls.Button btnTare;
        
        internal LGCNS.EZMES.ControlsLib.CNSDataGrid dgSourceList;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal LGCNS.iPharmMES.Common.iPharmAuthButton trustDispense;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%9B%90%EB%A3%8C%EC%B9%AD%EB%9F%89/Popup/TrustSca" +
                        "leWeightPopup.xaml", System.UriKind.Relative));
            this.rootWindow = ((LGCNS.iPharmMES.Common.iPharmMESChildWindow)(this.FindName("rootWindow")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtWeight = ((System.Windows.Controls.TextBox)(this.FindName("txtWeight")));
            this.btnKeyTrPad = ((System.Windows.Controls.Button)(this.FindName("btnKeyTrPad")));
            this.txtBarcode = ((보령.UserControls.BarcodeTextBox)(this.FindName("txtBarcode")));
            this.tbCurrentWeight = ((System.Windows.Controls.TextBlock)(this.FindName("tbCurrentWeight")));
            this.btnTare = ((System.Windows.Controls.Button)(this.FindName("btnTare")));
            this.dgSourceList = ((LGCNS.EZMES.ControlsLib.CNSDataGrid)(this.FindName("dgSourceList")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.trustDispense = ((LGCNS.iPharmMES.Common.iPharmAuthButton)(this.FindName("trustDispense")));
        }
    }
}

