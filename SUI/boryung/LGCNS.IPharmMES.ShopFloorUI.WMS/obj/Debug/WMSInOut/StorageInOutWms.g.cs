﻿#pragma checksum "D:\006.Project\Source\Project\SUI\boryung\LGCNS.IPharmMES.ShopFloorUI.WMS\WMSInOut\StorageInOutWms.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "94B7BCFA97BBBA9EE1EF78844B7E84B5"
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
    
    
    public partial class StorageInOutWms : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.UserControl Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid LayoutRootInner;
        
        internal System.Windows.Controls.TabItem TabInput;
        
        internal System.Windows.Controls.TextBox txtBarcodes;
        
        internal C1.Silverlight.DataGrid.C1DataGrid dgList;
        
        internal C1.Silverlight.DataGrid.C1DataGrid dgDetail;
        
        internal System.Windows.Controls.Button btnInput;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.Button btnInitial;
        
        internal System.Windows.Controls.TextBox txtBinWt;
        
        internal System.Windows.Controls.TextBox txtRawWt;
        
        internal System.Windows.Controls.TextBox txtTotalWts;
        
        internal System.Windows.Controls.TabItem TabOutput;
        
        internal System.Windows.Controls.Grid grdStorageOut;
        
        internal System.Windows.Controls.TextBox txtProdInfoOut;
        
        internal System.Windows.Controls.TextBox txtOrderNoOut;
        
        internal System.Windows.Controls.TextBox txtBatchNoOut;
        
        internal System.Windows.Controls.TextBox txtProcessOut;
        
        internal System.Windows.Controls.Button btnScanOut;
        
        internal C1.Silverlight.DataGrid.C1DataGrid dgOutputList;
        
        internal C1.Silverlight.DataGrid.C1DataGrid dgOutPutList2;
        
        internal System.Windows.Controls.TextBox txtIBCIDOutput;
        
        internal System.Windows.Controls.TextBox txtStdWt;
        
        internal System.Windows.Controls.TextBox txtWeightWt;
        
        internal System.Windows.Controls.Button btnOutPut;
        
        internal System.Windows.Controls.Button btnPrintOutput;
        
        internal System.Windows.Controls.Button btnWeightOut;
        
        internal System.Windows.Controls.Button btnInitialOutput;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WMS;component/WMSInOut/StorageInOutWms.xaml", System.UriKind.Relative));
            this.Main = ((System.Windows.Controls.UserControl)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LayoutRootInner = ((System.Windows.Controls.Grid)(this.FindName("LayoutRootInner")));
            this.TabInput = ((System.Windows.Controls.TabItem)(this.FindName("TabInput")));
            this.txtBarcodes = ((System.Windows.Controls.TextBox)(this.FindName("txtBarcodes")));
            this.dgList = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("dgList")));
            this.dgDetail = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("dgDetail")));
            this.btnInput = ((System.Windows.Controls.Button)(this.FindName("btnInput")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.btnInitial = ((System.Windows.Controls.Button)(this.FindName("btnInitial")));
            this.txtBinWt = ((System.Windows.Controls.TextBox)(this.FindName("txtBinWt")));
            this.txtRawWt = ((System.Windows.Controls.TextBox)(this.FindName("txtRawWt")));
            this.txtTotalWts = ((System.Windows.Controls.TextBox)(this.FindName("txtTotalWts")));
            this.TabOutput = ((System.Windows.Controls.TabItem)(this.FindName("TabOutput")));
            this.grdStorageOut = ((System.Windows.Controls.Grid)(this.FindName("grdStorageOut")));
            this.txtProdInfoOut = ((System.Windows.Controls.TextBox)(this.FindName("txtProdInfoOut")));
            this.txtOrderNoOut = ((System.Windows.Controls.TextBox)(this.FindName("txtOrderNoOut")));
            this.txtBatchNoOut = ((System.Windows.Controls.TextBox)(this.FindName("txtBatchNoOut")));
            this.txtProcessOut = ((System.Windows.Controls.TextBox)(this.FindName("txtProcessOut")));
            this.btnScanOut = ((System.Windows.Controls.Button)(this.FindName("btnScanOut")));
            this.dgOutputList = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("dgOutputList")));
            this.dgOutPutList2 = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("dgOutPutList2")));
            this.txtIBCIDOutput = ((System.Windows.Controls.TextBox)(this.FindName("txtIBCIDOutput")));
            this.txtStdWt = ((System.Windows.Controls.TextBox)(this.FindName("txtStdWt")));
            this.txtWeightWt = ((System.Windows.Controls.TextBox)(this.FindName("txtWeightWt")));
            this.btnOutPut = ((System.Windows.Controls.Button)(this.FindName("btnOutPut")));
            this.btnPrintOutput = ((System.Windows.Controls.Button)(this.FindName("btnPrintOutput")));
            this.btnWeightOut = ((System.Windows.Controls.Button)(this.FindName("btnWeightOut")));
            this.btnInitialOutput = ((System.Windows.Controls.Button)(this.FindName("btnInitialOutput")));
        }
    }
}

