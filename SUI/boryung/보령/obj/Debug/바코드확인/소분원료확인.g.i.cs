﻿#pragma checksum "D:\Root\PJ_201804_보령제약_예산공장\보령\바코드확인\소분원료확인.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8C3D7CF0C34193E789CAD4B32C359F2A"
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
    
    
    public partial class 소분원료확인 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal System.Windows.Controls.Grid Root;
        
        internal C1DataGrid dgMaterials;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EB%B0%94%EC%BD%94%EB%93%9C%ED%99%95%EC%9D%B8/%EC%8" +
                        "6%8C%EB%B6%84%EC%9B%90%EB%A3%8C%ED%99%95%EC%9D%B8.xaml", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.Root = ((System.Windows.Controls.Grid)(this.FindName("Root")));
            this.dgMaterials = ((C1DataGrid)(this.FindName("dgMaterials")));
        }
    }
}

