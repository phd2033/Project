﻿#pragma checksum "D:\2.업무\1.보령제약\00.보령제약_솔루션\Boryung_Board\Board\View\DetailSearch\FacilitiesOperationStatusView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3EAFF2B0D175286CECCDFF9BBAA50C5D"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using C1.Silverlight.DataGrid;
using LGCNS.EZMES.ControlsLib;
using LGCNS.iPharmMES.Common.UserCtl;
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


namespace Board {
    
    
    public partial class FacilitiesOperationStatusView : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.UserControl Main;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal LGCNS.EZMES.ControlsLib.MainPanel_Top mainPanel;
        
        internal System.Windows.Controls.Grid searchPanel;
        
        internal LGCNS.iPharmMES.Common.UserCtl.iPharmCodeName cnEquipment;
        
        internal C1.Silverlight.DataGrid.C1DataGrid dgEQWorkTime;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Board;component/View/DetailSearch/FacilitiesOperationStatusView.xaml", System.UriKind.Relative));
            this.Main = ((System.Windows.Controls.UserControl)(this.FindName("Main")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.mainPanel = ((LGCNS.EZMES.ControlsLib.MainPanel_Top)(this.FindName("mainPanel")));
            this.searchPanel = ((System.Windows.Controls.Grid)(this.FindName("searchPanel")));
            this.cnEquipment = ((LGCNS.iPharmMES.Common.UserCtl.iPharmCodeName)(this.FindName("cnEquipment")));
            this.dgEQWorkTime = ((C1.Silverlight.DataGrid.C1DataGrid)(this.FindName("dgEQWorkTime")));
        }
    }
}

