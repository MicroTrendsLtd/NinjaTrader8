using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.WpfPropertyGrid;
using System.Windows.Markup;
using System.Xml.Linq;
using NinjaTrader.Core;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.NinjaScript;
using NinjaTrader.Gui.Tools;
using NinjaTrader.NinjaScript;

namespace ATSQuadroStrategyBase.GUI
{

    public class ATSQuadroBaseStrategyPropertyGridPopUp : NTWindow
    {
        public event RoutedEventHandler OnOkClick;
        private Button BtnOk;
        private PropertyGrid propertyGrid;
        private StrategyBase selectedStrategy;
        private Label selectedStrategyLabel;

        public ATSQuadroBaseStrategyPropertyGridPopUp()
        {
            Caption = "ATSQuadroBase Strategy Settings";
            Width = 600;
            Height = 600;
            Content = LoadXaml();
        }

        public void Show(StrategyBase strategy)
        {
            selectedStrategy = strategy;

            Dispatcher.InvokeAsync(() =>
            {
                if (selectedStrategyLabel != null)
                    selectedStrategyLabel.Content = strategy.Name;
            });


            propertyGrid.Dispatcher.InvokeAsync(() =>
            {
                propertyGrid.SelectedObject = strategy;
            });
            
            Show();
        }



        private DependencyObject LoadXaml()
        {
            try
            {
                StreamReader mysr = new StreamReader(Path.Combine(NinjaTrader.Core.Globals.UserDataDir, "bin", "Custom", "Strategies", "ATSQuadroStrategyBase.GUI", "ATSQuadroBaseStrategyPropertyGridPopUp.xaml"));
                Page page = XamlReader.Load(mysr.BaseStream) as Page;

                if (page == null)
                    return null;

                BtnOk = LogicalTreeHelper.FindLogicalNode(page, "BtnOk") as Button;
                if (BtnOk != null)
                    BtnOk.Click += BtnOk_Click; ;

                selectedStrategyLabel = LogicalTreeHelper.FindLogicalNode(page, "SelectedStrategyLabel") as Label;
                


                propertyGrid = LogicalTreeHelper.FindLogicalNode(page, "PropertyGrid") as PropertyGrid;

                DependencyObject pageContent = page.Content as DependencyObject;

                return pageContent;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            //send to anything attached
            Dispatcher.InvokeAsync(() =>
                {
                    if (OnOkClick != null) OnOkClick.Invoke(this, e);
                });

            Close();

        }
    }
}
