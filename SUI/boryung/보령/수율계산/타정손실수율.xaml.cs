using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ShopFloorUI;
using C1.Silverlight.DataGrid;
using LGCNS.EZMES.ControlsLib;

namespace 보령
{
    public partial class 타정손실수율 : ShopFloorCustomWindow
    {
        C1.Silverlight.DataGrid.DataGridColumn[] _headerRowColumns;
        C1.Silverlight.DataGrid.DataGridRow[] _headerColumnRows;

        public 타정손실수율()
        {
            InitializeComponent();

            System.Text.StringBuilder empty = new System.Text.StringBuilder();
            LGCNS.iPharmMES.Common.UIObject.SetObjectLang(this, ref empty, LGCNS.EZMES.Common.LogInInfo.LangID);

            _headerRowColumns = dgTabletingInfo.Columns.Take(3).ToArray();
            _headerColumnRows = dgTabletingInfo.TopRows.Take(2).ToArray();
        }
        public override string TableTypeName
        {
            get { return "TABLE,타정손실수율"; }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void gridTabletingInfo_AutoGeneratingColumn(object sender, C1.Silverlight.DataGrid.DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Name)
            {
                case "GUBUN":
                    e.Cancel = false;
                    e.Column.IsReadOnly = true;
                    e.Column.Header = "구분";
                    break;
                case "InitSettingLoss":
                    e.Cancel = false;
                    //e.Column.IsReadOnly = true;
                    e.Column.Header = "초기세팅손실";
                    break;
                case "inspectionLoss":
                    e.Cancel = false;
                    //e.Column.IsReadOnly = true;
                    e.Column.Header = "공정 검사";
                    break;
                case "RemainingAmountLoss":
                    e.Cancel = false;
                    //e.Column.IsReadOnly = true;
                    e.Column.Header = "타정 후 진량";
                    break;
                case "BadTabletsLoss":
                    e.Cancel = false;
                    //e.Column.IsReadOnly = true;
                    e.Column.Header = "불량 정제";
                    break;
                case "UnknownCauseLoss":
                    e.Cancel = false;
                    e.Column.Header = "원인불명손실";
                    break;
                case "CurrentCollectorLoss":
                    e.Cancel = false;
                    e.Column.Header = "집전장치손실";
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }

        private void gridTabletingInfo_MergingCells(object sender, C1.Silverlight.DataGrid.DataGridMergingCellsEventArgs e)
        {
            try
            {
                var nonHeadersViewportCols = dgTabletingInfo.Viewport.Columns.Where(c => !_headerRowColumns.Contains(c)).ToArray();
                var nonHeadersViewportRows = dgTabletingInfo.Viewport.Rows.Where(r => !_headerColumnRows.Contains(r)).ToArray();

                // merge column & rows headers
                foreach (var range in MergingHelper.Merge(Orientation.Vertical, _headerColumnRows, nonHeadersViewportCols, true))
                {
                    e.Merge(range);
                }
                foreach (var range in MergingHelper.Merge(Orientation.Horizontal, nonHeadersViewportRows, _headerRowColumns, true))
                {
                    e.Merge(range);
                }
                // merge header intersection as we want, in this case, horizontally
                foreach (var range in MergingHelper.Merge(Orientation.Horizontal, _headerColumnRows, _headerRowColumns, true))
                {
                    e.Merge(range);
                }

                //var list = new List<C1.Silverlight.DataGrid.DataGridRow>();
                //foreach (var row in nonHeadersViewportRows)
                //{
                //    if (list.Count <= 0)
                //    {
                //        list.Add(row);
                //        continue;
                //    }
                //    else
                //    {
                //        var holderItem = list[0].DataItem as BR_PHR_SEL_ProductionOrder_Component_Summary_CHGSEQ.OUTDATA;
                //        var currentItem = row.DataItem as BR_PHR_SEL_ProductionOrder_Component_Summary_CHGSEQ.OUTDATA;

                //        if (holderItem.MTRLID == currentItem.MTRLID)
                //        {
                //            list.Add(row);
                //            continue;
                //        }
                //        else
                //        {

                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // 수정값 입력 완료
        private void gridTabletingInfo_CommittedEdit(object sender, DataGridCellEventArgs e)
        {
            (this.Root.DataContext as 타정손실수율ViewModel).ConvertResult();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
