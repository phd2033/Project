﻿#pragma checksum "D:\006.Project\Source\boryung\보령\수율계산\포장공정최종생산수율.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "89D0DF578AFB9A8AB2C75F27F99A5F52"
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
    
    
    public partial class 포장공정최종생산수율 : ShopFloorUI.ShopFloorCustomWindow {
        
        internal ShopFloorUI.ShopFloorCustomWindow Main;
        
        internal 보령.포장공정최종생산수율ViewModel ViewModel;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Border PrintArea;
        
        internal System.Windows.Controls.Grid FormulaGrid;
        
        internal System.Windows.Controls.Grid InputValueGrid;
        
        internal System.Windows.Controls.Grid OutputValueGrid;
        
        internal System.Windows.Controls.Button Cancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/%EB%B3%B4%EB%A0%B9;component/%EC%88%98%EC%9C%A8%EA%B3%84%EC%82%B0/%ED%8F%AC%EC%9" +
                        "E%A5%EA%B3%B5%EC%A0%95%EC%B5%9C%EC%A2%85%EC%83%9D%EC%82%B0%EC%88%98%EC%9C%A8.xam" +
                        "l", System.UriKind.Relative));
            this.Main = ((ShopFloorUI.ShopFloorCustomWindow)(this.FindName("Main")));
            this.ViewModel = ((보령.포장공정최종생산수율ViewModel)(this.FindName("ViewModel")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PrintArea = ((System.Windows.Controls.Border)(this.FindName("PrintArea")));
            this.FormulaGrid = ((System.Windows.Controls.Grid)(this.FindName("FormulaGrid")));
            this.InputValueGrid = ((System.Windows.Controls.Grid)(this.FindName("InputValueGrid")));
            this.OutputValueGrid = ((System.Windows.Controls.Grid)(this.FindName("OutputValueGrid")));
            this.Cancel = ((System.Windows.Controls.Button)(this.FindName("Cancel")));
        }
    }
}

