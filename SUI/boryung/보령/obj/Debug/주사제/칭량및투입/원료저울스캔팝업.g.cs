﻿#pragma checksum "D:\006.Project\Source\boryung\보령\주사제\칭량및투입\원료저울스캔팝업.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6FA6D19FED4B8F2C1BBB85B1DD2052D6"
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


namespace 보령 {
    
    
    public partial class 원료저울스캔팝업 : LGCNS.iPharmMES.Common.iPharmMESChildWindow {
        
        internal LGCNS.iPharmMES.Common.iPharmMESChildWindow Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox tbBarcode;
        
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
                        "0%8F%ED%88%AC%EC%9E%85/%EC%9B%90%EB%A3%8C%EC%A0%80%EC%9A%B8%EC%8A%A4%EC%BA%94%ED" +
                        "%8C%9D%EC%97%85.xaml", System.UriKind.Relative));
            this.Main = ((LGCNS.iPharmMES.Common.iPharmMESChildWindow)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tbBarcode = ((System.Windows.Controls.TextBox)(this.FindName("tbBarcode")));
            this.btnClose = ((System.Windows.Controls.Button)(this.FindName("btnClose")));
        }
    }
}

