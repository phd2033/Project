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
using LGCNS.iPharmMES.Common;

namespace 보령
{
    public partial class InputWeightpopup : iPharmMESChildWindow
    {
        public InputWeightpopup()
        {
            InitializeComponent();

            System.Text.StringBuilder empty = new System.Text.StringBuilder();
            LGCNS.iPharmMES.Common.UIObject.SetObjectLang(this, ref empty, LGCNS.EZMES.Common.LogInInfo.LangID);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            double convertedWeight = 0;

            if (this.txtUOM.Text.Equals("kg") || this.txtUOM.Text.Equals("g") || this.txtUOM.Text.Equals("mg"))
            {
                if (double.TryParse(this.txtWeight.Text, out convertedWeight))
                {
                    if (this.LayoutRoot.DataContext is 평균질량측정ViewModel)
                    {
                        (this.LayoutRoot.DataContext as 평균질량측정ViewModel).popupWeight = this.txtWeight.Text;
                        (this.LayoutRoot.DataContext as 평균질량측정ViewModel).popupUOM = this.txtUOM.Text;
                        (this.LayoutRoot.DataContext as 평균질량측정ViewModel).curWeighing = string.Format("{0:#.###}{1}", Double.Parse(this.txtWeight.Text), this.txtUOM.Text);
                    }
                    else if (this.LayoutRoot.DataContext is 평균질량측정_2ViewModel)
                    {
                        (this.LayoutRoot.DataContext as 평균질량측정_2ViewModel).popupWeight = this.txtWeight.Text;
                        (this.LayoutRoot.DataContext as 평균질량측정_2ViewModel).popupUOM = this.txtUOM.Text;
                        (this.LayoutRoot.DataContext as 평균질량측정_2ViewModel).curWeighing = string.Format("{0:#.###}{1}", Double.Parse(this.txtWeight.Text), this.txtUOM.Text);
                    }                    
                    this.DialogResult = true;
                }
            }
        }

    }
}
