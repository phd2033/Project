﻿#pragma checksum "D:\Root\iPharmMES_ShopFloorUI\LGCNS.IPharmMES.ShopFloorUI.WMS\Part Washisng\PartWashing.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2E13FD6E6B8F6A33E420E77E48D2ACA0"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class PartWashing : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.UserControl Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TabItem tabWashing;
        
        internal System.Windows.Controls.BusyIndicator tktBusyIndctWashing;
        
        internal System.Windows.Controls.Grid WashingGrid;
        
        internal System.Windows.Controls.TextBox tbHederN;
        
        internal C1.Silverlight.DataGrid.C1DataGrid CleanRecipeDataGrid;
        
        internal System.Windows.Controls.TextBox txtMain;
        
        internal System.Windows.Controls.TabItem tbDry;
        
        internal System.Windows.Controls.Grid DryGrid;
        
        internal System.Windows.Controls.BusyIndicator tktBusyIndctDry;
        
        internal System.Windows.Controls.Grid Container;
        
        internal System.Windows.Controls.TextBox FromPartText;
        
        internal System.Windows.Controls.TextBox ToPartText;
        
        internal System.Windows.Controls.Button OpenCalendarPart;
        
        internal System.Windows.Controls.Primitives.Popup PopupPart;
        
        internal System.Windows.Controls.Grid PopupRootPart;
        
        internal System.Windows.Controls.Calendar FromPartCalendar;
        
        internal System.Windows.Controls.Calendar ToPartCalendar;
        
        internal C1.Silverlight.DataGrid.C1DataGrid DryDataGrid;
        
        internal System.Windows.Controls.Button btnOpen;
        
        internal System.Windows.Controls.BusyIndicator tktBusyindctDry2;
        
        internal System.Windows.Controls.TextBox txtPartWasher;
        
        internal System.Windows.Controls.TextBox txtCleanRecipe;
        
        internal System.Windows.Controls.TextBox txtDryStartDate;
        
        internal System.Windows.Controls.TextBox txtDryEndDate;
        
        internal C1.Silverlight.DataGrid.C1DataGrid DryDetailGrid;
        
        internal System.Windows.Controls.Button btnPrev;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WMS;component/Part%20Washisng/PartWashing.xaml", System.UriKind.Relative));
            this.Main = ((System.Windows.Controls.UserControl)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tabWashing = ((System.Windows.Controls.TabItem)(this.FindName("tabWashing")));
            this.tktBusyIndctWashing = ((System.Windows.Controls.BusyIndicator)(this.FindName("tktBusyIndctWashing")));
            this.WashingGrid = ((System.Windows.Controls.Grid)(this.FindName("WashingGrid")));
            this.tbHederN = ((System.Windows.Controls.TextBox)(this.FindName("tbHederN")));
            this.CleanRecipeDataGrid = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("CleanRecipeDataGrid")));
            this.txtMain = ((System.Windows.Controls.TextBox)(this.FindName("txtMain")));
            this.tbDry = ((System.Windows.Controls.TabItem)(this.FindName("tbDry")));
            this.DryGrid = ((System.Windows.Controls.Grid)(this.FindName("DryGrid")));
            this.tktBusyIndctDry = ((System.Windows.Controls.BusyIndicator)(this.FindName("tktBusyIndctDry")));
            this.Container = ((System.Windows.Controls.Grid)(this.FindName("Container")));
            this.FromPartText = ((System.Windows.Controls.TextBox)(this.FindName("FromPartText")));
            this.ToPartText = ((System.Windows.Controls.TextBox)(this.FindName("ToPartText")));
            this.OpenCalendarPart = ((System.Windows.Controls.Button)(this.FindName("OpenCalendarPart")));
            this.PopupPart = ((System.Windows.Controls.Primitives.Popup)(this.FindName("PopupPart")));
            this.PopupRootPart = ((System.Windows.Controls.Grid)(this.FindName("PopupRootPart")));
            this.FromPartCalendar = ((System.Windows.Controls.Calendar)(this.FindName("FromPartCalendar")));
            this.ToPartCalendar = ((System.Windows.Controls.Calendar)(this.FindName("ToPartCalendar")));
            this.DryDataGrid = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("DryDataGrid")));
            this.btnOpen = ((System.Windows.Controls.Button)(this.FindName("btnOpen")));
            this.tktBusyindctDry2 = ((System.Windows.Controls.BusyIndicator)(this.FindName("tktBusyindctDry2")));
            this.txtPartWasher = ((System.Windows.Controls.TextBox)(this.FindName("txtPartWasher")));
            this.txtCleanRecipe = ((System.Windows.Controls.TextBox)(this.FindName("txtCleanRecipe")));
            this.txtDryStartDate = ((System.Windows.Controls.TextBox)(this.FindName("txtDryStartDate")));
            this.txtDryEndDate = ((System.Windows.Controls.TextBox)(this.FindName("txtDryEndDate")));
            this.DryDetailGrid = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("DryDetailGrid")));
            this.btnPrev = ((System.Windows.Controls.Button)(this.FindName("btnPrev")));
        }
    }
}

