using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.WpfPropertyGrid;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATSQuadroStrategyBase.GUI
{
    /// <summary>
    /// Interaction logic for UserControlEasyConfig.xaml
    /// </summary>
    public partial class UserControlEasyConfig : UserControl
    {
        //routedEvents
        public event RoutedEventHandler OnLoadClick;
        public event RoutedEventHandler OnRefreshClick;
        public event RoutedEventHandler OnSaveClick;
        public event RoutedEventHandler OnSaveAsClick;
        public event RoutedEventHandler OnOkClick;
        public event RoutedEventHandler OnCancelClick;
        //public event RoutedEventHandler OnCloseClick;// use ok
        public event RoutedEventHandler OnRestartClick;
        public event RoutedEventHandler OnApplyClick;

        public UserControlEasyConfig()
        {
            InitializeComponent();

        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {

            if (OnLoadClick != null)
                OnLoadClick.Invoke(this, e);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (OnRefreshClick != null)
                OnRefreshClick.Invoke(this, e);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveClick != null)
                OnSaveClick.Invoke(this, e);
        }

        private void BtnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveAsClick != null)
                OnSaveAsClick.Invoke(this, e);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {

            if (OnOkClick != null) OnOkClick.Invoke(this, e);
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {

            if (OnApplyClick != null) OnApplyClick.Invoke(this, e);

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

            if (OnCancelClick != null) OnCancelClick.Invoke(this, e);
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            if (OnRestartClick != null) OnRestartClick.Invoke(this, e);
        }


        #region Properties
        public object PropertyGridSetupSelectedObject
        {
            set
            {
                PropertyGrid1.SelectedObject = value;
            }
        }

        public object PropertyGridFiltersSelectedObject
        {
            set
            {
                PropertyGrid2.SelectedObject = value;
            }
        }

        public object PropertyGridExitsSelectedObject
        {
            set
            {
                PropertyGrid3.SelectedObject = value;
            }
        }

        public object PropertyGridHTFSelectedObject
        {
            set
            {
                PropertyGrid4.SelectedObject = value;
            }
        }

        public object PropertyGridTradeManSelectedObject
        {
            set
            {
                PropertyGrid5.SelectedObject = value;
            }
        }

        public PropertyGrid PropertyGridSetup
        {
            get
            {
                return PropertyGrid1;
            }
        }

        public PropertyGrid PropertyGridFilters
        {
            get
            {
                return PropertyGrid2;
            }
        }

        public PropertyGrid PropertyGridExits
        {
            get
            {
                return PropertyGrid3;
            }
        }

        public PropertyGrid PropertyGridHTF
        {
            get
            {
                return PropertyGrid4;
            }
        }

        public PropertyGrid PropertyGridTradeMan
        {
            get
            {
                return PropertyGrid5;
            }
        }
        #endregion
    }
}
