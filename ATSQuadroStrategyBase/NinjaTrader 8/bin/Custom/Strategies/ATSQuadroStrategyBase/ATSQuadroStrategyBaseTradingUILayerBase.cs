using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ATSQuadroStrategyBase.GUI;
using System.Windows.Media;
using NinjaTrader.NinjaScript.Indicators;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using NinjaTrader.NinjaScript.DrawingTools;
using NinjaTrader.Gui;
using NinjaTrader.Cbi;
using NinjaTrader.Core;
using System.Reflection;
using System.Diagnostics;

namespace NinjaTrader.NinjaScript.Strategies
{
    /// <summary>
    /// AlgoSystemTradingUILayerBase contains hybrid algo trading user intreface control features for realtime trading interaction for fuly and semi automated modes.
    /// </summary>
    public abstract partial class AlgoSystemTradingUILayerBase : AlgoSystemTradingRulesLayerBase
    {
        protected override void OnStateChange()
        {
            base.OnStateChange();
            try
            {
                if (State == State.DataLoaded)
                {

                    #region Ninjabuddy Tool bar
                    activeBackgroundDarkGray = new System.Windows.Media.SolidColorBrush(Color.FromRgb(30, 30, 30));
                    activeBackgroundDarkGray.Freeze();
                    backGroundMediumGray = new System.Windows.Media.SolidColorBrush(Color.FromRgb(45, 45, 47));
                    backGroundMediumGray.Freeze();
                    controlLightGray = new System.Windows.Media.SolidColorBrush(Color.FromRgb(64, 63, 69));
                    controlLightGray.Freeze();
                    textColor = new System.Windows.Media.SolidColorBrush(Color.FromRgb(204, 204, 204));
                    textColor.Freeze();
                    #endregion

                    //create and add visual indicators
                    aTSIndicatorQSBStrategyVisualiser = ATSIndicatorQSBStrategyVisualiser(BarsArray[0]);
                    AddChartIndicator(aTSIndicatorQSBStrategyVisualiser);

                    aTSIndicatorQSBStrategyInfoBar = ATSIndicatorQSBStrategyInfoBar(BarsArray[0]);
                    AddChartIndicator(aTSIndicatorQSBStrategyInfoBar);

                }
                else if (State == State.Realtime)
                {
                    if (ChartControl != null)
                    {
                        ChartControl.Dispatcher.InvokeAsync((Action)(() =>
                        {
                            WPFControlsInit();
                            WPFControlsCreate();
                            if (WPFControlsChartTabSelected())
                                WPFControlsInsert();
                        }));
                    }
                }
                else if (State == State.Terminated)
                {
                    if (ChartControl != null)
                    {
                        ChartControl.Dispatcher.InvokeAsync((Action)(() =>
                        {
                            WPFControlsDispose();
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Print(this.Name + " > OnStateChange > Error:" + ex.ToString());
            }
        }

        #region methods
        #region NinjaBuddy, Config & Toolbar
        #region WPFControls

        public virtual void WPFControlsInit()
        {
            if (IsTracingMode)
                Print("WPFControlsInit");
            try
            {
                wPFControlsStrategyId = Guid.NewGuid().ToString();
                wPFControlsButtonNinjaBuddyAutomationID = string.Format("buttonNinjaBuddy{0}", wPFControlsStrategyId);
                wPFControlsButtonEasyConfigAutomationID = string.Format("buttonEasyConfig{0}", wPFControlsStrategyId);
                wPFControlsButtonNewsAutomationID = string.Format("buttonNewsAutomation{0}", wPFControlsStrategyId);

                wPFControlsChartWindow = System.Windows.Window.GetWindow(ChartControl.Parent) as Chart;
                wPFControlsChartWindow.MainTabControl.SelectionChanged += WPFControlsChartTabChangedHandler;

                wPFControlsChartGrid = wPFControlsChartWindow.MainTabControl.Parent as System.Windows.Controls.Grid;
                wPFControlsChartTrader = wPFControlsChartWindow.FindFirst("ChartWindowChartTraderControl") as Gui.Chart.ChartTrader;
                if (wPFControlsChartTrader != null)
                {
                    if (NinjaTrader.Gui.Chart.ChartHelper.GetChartTraderVisibility(this.ChartControl) == ChartTraderVisibility.Collapsed)
                    {
                        NinjaTrader.Gui.Chart.ChartHelper.SetChartTraderVisibility(this.ChartControl, ChartTraderVisibility.VisibleCollapsed);
                    }
                }
                wPFControlsChartWindowToolBar = wPFControlsChartWindow.FindFirst("ChartWindowToolBar") as System.Windows.Controls.ToolBar;

            }
            catch (Exception ex)
            {
                Print("WPFControlsInit > Error: " + ex.ToString());

            }


        }

        public virtual void WPFControlsCreate()
        {
            if (IsTracingMode)
                Print("WPFControlsCreate");

            try
            {

                WPFControlsToolBarCreate();
                WPFControlsNinjaBuddyCreate();

            }
            catch (Exception ex)
            {
                Print("WPFControlsCreate > Error: " + ex.ToString());

            }

        }

        public virtual void WPFControlsInsert()
        {
            try
            {
                if (wPFControlsIsPanelActive)
                    return;

                WPFControlsToolBarInsert();
                WPFControlsNinjaBuddyInsert();
            }
            catch (Exception ex)
            {
                Print("WPFControlsInsert > Error >" + ex.ToString());
            }
            wPFControlsIsPanelActive = true;
        }

        public virtual void WPFControlsToolBarCreate()
        {
            try
            {
                wPFControlsChartWindow = Window.GetWindow(ChartControl.Parent) as Chart;
                if (wPFControlsChartWindow == null) return;

                wPFControlsButtonNinjaBuddy = wPFControlsChartWindow.FindFirst(wPFControlsButtonNinjaBuddyAutomationID) as System.Windows.Controls.Button;

                if (wPFControlsButtonNinjaBuddy == null)
                {
                    wPFControlsButtonNinjaBuddy = new Button();
                    wPFControlsButtonNinjaBuddy.Content = "Trade Man.";
                    AutomationProperties.SetAutomationId(wPFControlsButtonNinjaBuddy, wPFControlsButtonNinjaBuddyAutomationID);
                    wPFControlsButtonNinjaBuddy.Click += WPFControlsButtonNinjaBuddy_Click;
                }

                wPFControlsButtonEasyConfig = wPFControlsChartWindow.FindFirst(wPFControlsButtonEasyConfigAutomationID) as System.Windows.Controls.Button;

                if (wPFControlsButtonEasyConfig == null)
                {
                    wPFControlsButtonEasyConfig = new Button();
                    wPFControlsButtonEasyConfig.Content = "Config.";
                    AutomationProperties.SetAutomationId(wPFControlsButtonEasyConfig, wPFControlsButtonEasyConfigAutomationID);
                    wPFControlsButtonEasyConfig.Click += WPFControlsButtonEasyConfig_Click;
                }


                wPFControlsButtonNews = wPFControlsChartWindow.FindFirst(wPFControlsButtonNewsAutomationID) as System.Windows.Controls.Button;

                if (wPFControlsButtonNews == null)
                {
                    wPFControlsButtonNews = new Button();
                    wPFControlsButtonNews.Content = "Eco. News";
                    AutomationProperties.SetAutomationId(wPFControlsButtonNews, wPFControlsButtonNewsAutomationID);
                    wPFControlsButtonNews.Click += WPFControlsButtonNews_Click;
                }

            }
            catch (Exception ex)
            {
                Print("WPFControlsCreateToolBar > Error >" + ex.ToString());
            }


        }

        public virtual void WPFControlsToolBarInsert()
        {

            try
            {

                if (wPFControlsChartWindow.FindFirst(wPFControlsButtonNinjaBuddyAutomationID) == null)
                    wPFControlsChartWindow.MainMenu.Add(wPFControlsButtonNinjaBuddy);

                if (wPFControlsChartWindow.FindFirst(wPFControlsButtonEasyConfigAutomationID) == null)
                    wPFControlsChartWindow.MainMenu.Add(wPFControlsButtonEasyConfig);

                if (wPFControlsChartWindow.FindFirst(wPFControlsButtonNewsAutomationID) == null)
                    wPFControlsChartWindow.MainMenu.Add(wPFControlsButtonNews);

            }
            catch (Exception ex)
            {
                Print("WPFControlsInsertToolBar > Error >" + ex.ToString());
            }

        }

        public virtual void WPFControlsToolBarRemove()
        {
            #region ToolBar
            try
            {
                if (wPFControlsChartWindow == null) return;

                if (wPFControlsButtonNinjaBuddy != null)
                {
                    wPFControlsChartWindow.MainMenu.Remove(wPFControlsButtonNinjaBuddy);
                }
                if (wPFControlsButtonEasyConfig != null)
                {
                    wPFControlsChartWindow.MainMenu.Remove(wPFControlsButtonEasyConfig);
                }
                if (wPFControlsButtonNews != null)
                {
                    wPFControlsChartWindow.MainMenu.Remove(wPFControlsButtonNews);
                }
            }
            catch (Exception ex)
            {
                Print("WPFControlsRemove > ToolBar > Error: " + ex.ToString());

            }
            #endregion
        }

        public virtual void WPFControlsRemove()
        {
            if (!wPFControlsIsPanelActive)
                return;

            try
            {
                WPFControlsToolBarRemove();
                WPFControlsNinjaBuddyRemove();
                //WPFControlsEasyConfigClose();
            }
            catch (Exception ex)
            {
                Print("WPFControlsRemove > Error >" + ex.ToString());
            }

            wPFControlsIsPanelActive = false;
        }

        public virtual void WPFControlsDispose()
        {
            try
            {
                if (wPFControlsChartWindow != null)
                    wPFControlsChartWindow.MainTabControl.SelectionChanged -= WPFControlsChartTabChangedHandler;

                WPFControlsRemove();
                WPFControlsNinjaBuddyDispose();
                //WPFControlsEasyConfigClose();

            }
            catch (Exception ex)
            {
                Print("WPFControlsDispose > Error >" + ex.ToString());
            }

        }

        public virtual bool WPFControlsChartTabSelected()
        {
            bool chartTabSelected = false;
            try
            {
                // loop through each tab and see if the tab this is added to is the selected item
                foreach (System.Windows.Controls.TabItem tab in wPFControlsChartWindow.MainTabControl.Items)
                    if ((tab.Content as ChartTab).ChartControl == ChartControl && tab == wPFControlsChartWindow.MainTabControl.SelectedItem)
                        chartTabSelected = true;

            }
            catch (Exception ex)
            {
                Print("ChartTabSelected > Error >" + ex.ToString());
            }

            return chartTabSelected;
        }

        public virtual void WPFControlsChartTabChangedHandler(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            try
            {

                if (e.AddedItems.Count <= 0)
                    return;

                wPFControlsTabItem = e.AddedItems[0] as System.Windows.Controls.TabItem;
                if (wPFControlsTabItem == null)
                    return;

                chartTab = wPFControlsTabItem.Content as NinjaTrader.Gui.Chart.ChartTab;
                if (chartTab == null)
                    return;

                if (WPFControlsChartTabSelected())
                    WPFControlsInsert();
                else
                    WPFControlsRemove();
            }
            catch (Exception ex)
            {
                Print("ChartTabChangedHandler > Error >" + ex.ToString());
            }
        }

        public virtual void WPFControlsButtonNews_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://alphawebtrader.com/econews");
        }

        public virtual void WPFControlsButtonEasyConfig_Click(object sender, RoutedEventArgs e)
        {
            //create and show the window ATSQuadroBaseStrategyPropertyGridPopUp with the strategy properties displayed inthe PropertyGrid 
            Globals.RandomDispatcher.BeginInvoke(new Action(() => new ATSQuadroBaseStrategyPropertyGridPopUp().Show(this)));

        }

        public virtual void WPFControlsButtonNinjaBuddy_Click(object sender, RoutedEventArgs e)
        {
            if (wPFControlsUserControlNinjaBuddy1 == null)
            {
                WPFControlsNinjaBuddyInsert();
            }

            WPFControlsNinjaBuddyVisibilty(!(wPFControlsUserControlNinjaBuddy1.IsVisible));
        }

        #endregion
        #region NinjaBuddy
        public virtual void WPFControlsNinjaBuddyCreate()
        {
            #region NinjaBuddy

            try
            {
                wPFControlsUserControlNinjaBuddy1 = new UserControlNinjaBuddy();
                wPFControlsUserControlNinjaBuddy1.Padding = new Thickness(0);
                wPFControlsUserControlNinjaBuddy1.Margin = new Thickness(0, -6, -6, -6);
                wPFControlsUserControlNinjaBuddy1.Height = wPFControlsChartTrader.Height;
                System.Windows.Controls.Grid.SetRow(wPFControlsUserControlNinjaBuddy1, 0);

                wPFControlsUserControlNinjaBuddy1.DataContext = this;

                wPFControlsUserControlNinjaBuddy1.OnAutoClick += WPFControlsUserControlNinjaBuddy1_OnAutoClick;
                wPFControlsUserControlNinjaBuddy1.OnAutoLongClick += WPFControlsUserControlNinjaBuddy1_OnAutoLongClick;
                wPFControlsUserControlNinjaBuddy1.OnAutoShortClick += WPFControlsUserControlNinjaBuddy1_OnAutoShortClick;
                wPFControlsUserControlNinjaBuddy1.OnBoxClearClick += WPFControlsUserControlNinjaBuddy1_OnBoxClearClick;
                wPFControlsUserControlNinjaBuddy1.OnBoxLowerClick += WPFControlsUserControlNinjaBuddy1_OnBoxLowerClick;
                wPFControlsUserControlNinjaBuddy1.OnBoxUpperClick += WPFControlsUserControlNinjaBuddy1_OnBoxUpperClick;
                wPFControlsUserControlNinjaBuddy1.OnBreakEvenClick += WPFControlsUserControlNinjaBuddy1_OnBreakEvenClick;
                wPFControlsUserControlNinjaBuddy1.OnBuyClick += WPFControlsUserControlNinjaBuddy1_OnBuyClick;
                wPFControlsUserControlNinjaBuddy1.OnCloseClick += WPFControlsUserControlNinjaBuddy1_OnCloseClick;
                wPFControlsUserControlNinjaBuddy1.OnOCOBreakoutClick += WPFControlsUserControlNinjaBuddy1_OnOCOBreakoutClick;
                wPFControlsUserControlNinjaBuddy1.OnSellClick += WPFControlsUserControlNinjaBuddy1_OnSellClick;
                wPFControlsUserControlNinjaBuddy1.OnTrail1Click += WPFControlsUserControlNinjaBuddy1_OnTrail1Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail2Click += WPFControlsUserControlNinjaBuddy1_OnTrail2Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail3Click += WPFControlsUserControlNinjaBuddy1_OnTrail3Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail4Click += WPFControlsUserControlNinjaBuddy1_OnTrail4Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail50Click += WPFControlsUserControlNinjaBuddy1_OnTrail50Click;
                wPFControlsUserControlNinjaBuddy1.OnTrailHiLoClick += WPFControlsUserControlNinjaBuddy1_OnTrailHiLoClick;
                wPFControlsUserControlNinjaBuddy1.OnTrailTriggerClick += WPFControlsUserControlNinjaBuddy1_OnTrailTriggerClick;

                if (ChartControl != null)
                {
                    foreach (ChartScale scale in ChartPanel.Scales)
                        if (scale.ScaleJustification == ScaleJustification)
                            wPFControlsChartScale = scale;

                    ChartPanel.MouseLeftButtonDown += WPFControlsChartPanel_MouseLeftButtonDown;
                }


            }
            catch (Exception ex)
            {
                Print("WPFControlsCreateNinjaBuddy > Error >" + ex.ToString());
            }

            #endregion
        }
        public virtual void WPFControlsNinjaBuddyInsert()
        {

            try
            {
                if (wPFControlsUserControlNinjaBuddy1 == null) WPFControlsNinjaBuddyCreate();

                chartTraderStartColumn = System.Windows.Controls.Grid.GetColumn(wPFControlsChartTrader);

                // a new column is added to the right of ChartTrader
                wPFControlsChartGrid.ColumnDefinitions.Insert((chartTraderStartColumn + 2), new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(165) });

                // all items to the right of the ChartTrader are shifted to the right
                for (int i = 0; i < wPFControlsChartGrid.Children.Count; i++)
                    if (System.Windows.Controls.Grid.GetColumn(wPFControlsChartGrid.Children[i]) > chartTraderStartColumn)
                        System.Windows.Controls.Grid.SetColumn(wPFControlsChartGrid.Children[i], System.Windows.Controls.Grid.GetColumn(wPFControlsChartGrid.Children[i]) + 1);

                // and then we set our new grid to be within the new column of the chart grid (and on the same row as the MainTabControl)
                System.Windows.Controls.Grid.SetColumn(wPFControlsUserControlNinjaBuddy1, System.Windows.Controls.Grid.GetColumn(wPFControlsChartTrader) + 2);
                System.Windows.Controls.Grid.SetRow(wPFControlsUserControlNinjaBuddy1, System.Windows.Controls.Grid.GetRow(wPFControlsChartWindow.MainTabControl));

                //add Ninjabuddy
                wPFControlsChartGrid.Children.Add(wPFControlsUserControlNinjaBuddy1);

                if (wPFControlsChartTrader != null)
                {
                    //set account to enable viewing of Trade Visuals orders/postion on chart
                    AccountSelector cbxAccounts = wPFControlsChartTrader.FindFirst("ChartTraderControlAccountSelector") as Gui.Tools.AccountSelector;
                    if (cbxAccounts != null)
                    {
                        cbxAccounts.SelectedAccount = this.Account;
                    }
                }


            }
            catch (Exception ex)
            {
                Print("WPFControlsInsertNinjaBuddy > Error >" + ex.ToString());
            }


        }
        public virtual void WPFControlsNinjaBuddyVisibilty(bool visible)
        {
            try
            {
                if (visible)
                {
                    wPFControlsChartGrid.ColumnDefinitions[System.Windows.Controls.Grid.GetColumn(wPFControlsUserControlNinjaBuddy1)].Width = new GridLength(165);
                    wPFControlsUserControlNinjaBuddy1.Visibility = Visibility.Visible;

                    if (wPFControlsChartTrader != null)
                    {
                        //set account to enable viewing of Trade Visuals orders/postion on chart
                        AccountSelector cbxAccounts = wPFControlsChartTrader.FindFirst("ChartTraderControlAccountSelector") as Gui.Tools.AccountSelector;
                        if (cbxAccounts != null)
                        {
                            cbxAccounts.SelectedAccount = this.Account;
                        }
                    }

                }
                else
                {
                    wPFControlsUserControlNinjaBuddy1.Visibility = Visibility.Hidden;
                    wPFControlsChartGrid.ColumnDefinitions[System.Windows.Controls.Grid.GetColumn(wPFControlsUserControlNinjaBuddy1)].Width = new GridLength(0);
                }
            }
            catch (Exception ex)
            {
                Print("WPFControlsNinjaBuddyVisibilty > Error >" + ex.ToString());
            }

        }
        public virtual void WPFControlsNinjaBuddyDispose()
        {
            try
            {

                if (wPFControlsUserControlNinjaBuddy1 == null) return;

                wPFControlsUserControlNinjaBuddy1.OnAutoClick -= WPFControlsUserControlNinjaBuddy1_OnAutoClick;
                wPFControlsUserControlNinjaBuddy1.OnAutoLongClick -= WPFControlsUserControlNinjaBuddy1_OnAutoLongClick;
                wPFControlsUserControlNinjaBuddy1.OnAutoShortClick -= WPFControlsUserControlNinjaBuddy1_OnAutoShortClick;
                wPFControlsUserControlNinjaBuddy1.OnBoxClearClick -= WPFControlsUserControlNinjaBuddy1_OnBoxClearClick;
                wPFControlsUserControlNinjaBuddy1.OnBoxLowerClick -= WPFControlsUserControlNinjaBuddy1_OnBoxLowerClick;
                wPFControlsUserControlNinjaBuddy1.OnBoxUpperClick -= WPFControlsUserControlNinjaBuddy1_OnBoxUpperClick;
                wPFControlsUserControlNinjaBuddy1.OnBreakEvenClick -= WPFControlsUserControlNinjaBuddy1_OnBreakEvenClick;
                wPFControlsUserControlNinjaBuddy1.OnBuyClick -= WPFControlsUserControlNinjaBuddy1_OnBuyClick;
                wPFControlsUserControlNinjaBuddy1.OnCloseClick -= WPFControlsUserControlNinjaBuddy1_OnCloseClick;
                wPFControlsUserControlNinjaBuddy1.OnOCOBreakoutClick -= WPFControlsUserControlNinjaBuddy1_OnOCOBreakoutClick;
                wPFControlsUserControlNinjaBuddy1.OnSellClick -= WPFControlsUserControlNinjaBuddy1_OnSellClick;
                wPFControlsUserControlNinjaBuddy1.OnTrail1Click -= WPFControlsUserControlNinjaBuddy1_OnTrail1Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail2Click -= WPFControlsUserControlNinjaBuddy1_OnTrail2Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail3Click -= WPFControlsUserControlNinjaBuddy1_OnTrail3Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail4Click -= WPFControlsUserControlNinjaBuddy1_OnTrail4Click;
                wPFControlsUserControlNinjaBuddy1.OnTrail50Click -= WPFControlsUserControlNinjaBuddy1_OnTrail50Click;
                wPFControlsUserControlNinjaBuddy1.OnTrailHiLoClick -= WPFControlsUserControlNinjaBuddy1_OnTrailHiLoClick;
                wPFControlsUserControlNinjaBuddy1.OnTrailTriggerClick -= WPFControlsUserControlNinjaBuddy1_OnTrailTriggerClick;


                if (ChartControl != null)
                {
                    ChartPanel.MouseLeftButtonDown -= WPFControlsChartPanel_MouseLeftButtonDown;

                }



                wPFControlsUserControlNinjaBuddy1 = null;

            }
            catch (Exception ex)
            {
                Print("WPFControlsRemove > NinmjaBuddy > Error: " + ex.ToString());
            }


        }
        public virtual void WPFControlsNinjaBuddyRemove()
        {
            try
            {
                // remove the column of our added grid
                wPFControlsChartGrid.ColumnDefinitions.RemoveAt(System.Windows.Controls.Grid.GetColumn(wPFControlsUserControlNinjaBuddy1));
                // then remove the grid
                wPFControlsChartGrid.Children.Remove(wPFControlsUserControlNinjaBuddy1);

                // if the childs column is 1 (so we can move it to 0) and the column is to the right of the column we are removing, shift it left
                for (int i = 0; i < wPFControlsChartGrid.Children.Count; i++)
                    if (System.Windows.Controls.Grid.GetColumn(wPFControlsChartGrid.Children[i]) > 0 && System.Windows.Controls.Grid.GetColumn(wPFControlsChartGrid.Children[i]) > System.Windows.Controls.Grid.GetColumn(wPFControlsUserControlNinjaBuddy1))
                        System.Windows.Controls.Grid.SetColumn(wPFControlsChartGrid.Children[i], System.Windows.Controls.Grid.GetColumn(wPFControlsChartGrid.Children[i]) - 1);
            }
            catch (Exception ex)
            {
                Print("WPFControlsRemove > NinmjaBuddy > Error: " + ex.ToString());
            }
        }
        public virtual void WPFControlsChartTopMenuItemNinjaBuddy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (wPFControlsUserControlNinjaBuddy1.IsVisible)
                {
                    WPFControlsNinjaBuddyVisibilty(false);
                }
                else
                {
                    WPFControlsNinjaBuddyVisibilty(true);
                }
            }
            catch (Exception ex)
            {
                Print("ChartTopMenuItemNinjaBuddy_Click > Error >" + ex.ToString());
            }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnTrailTriggerClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState == AlgoSystemState.HisTradeRT) return;
            if (Position.MarketPosition == MarketPosition.Flat) return;
            //if (!this.IsUseTrailing) return;
            //if (!this.trail1Active) { this.trail1Active = true; return; }
            //if (!this.trail2Active) { this.trail2Active = true; return; }
            //if (!this.trail3Active) { this.trail3Active = true; return; }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnTrailHiLoClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Flat) return;


            double low = Math.Min(base.GetCurrentBid(0) - TickSize, Bars.GetLow(CurrentBar + 1));



            double high = Math.Max(base.GetCurrentAsk(0) + TickSize, Bars.GetHigh(CurrentBar + 1));


            if (IsOrderIsActive(orderStop1) && IsOrderActiveCanChangeOrCancel(orderStop1))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop1, orderStop1.Quantity, 0, low);
                }
                else
                    ChangeOrder(orderStop1, orderStop1.Quantity, 0, high);
            }
            if (IsOrderIsActive(orderStop2) && IsOrderActiveCanChangeOrCancel(orderStop2))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop2, orderStop2.Quantity, 0, low);
                }
                else
                    ChangeOrder(orderStop2, orderStop2.Quantity, 0, high);
            }
            if (IsOrderIsActive(orderStop3) && IsOrderActiveCanChangeOrCancel(orderStop3))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop3, orderStop3.Quantity, 0, low);
                }
                else
                    ChangeOrder(orderStop3, orderStop3.Quantity, 0, high);
            }
            if (IsOrderIsActive(orderStop4) && IsOrderActiveCanChangeOrCancel(orderStop4))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop4, orderStop4.Quantity, 0, low);
                }
                else
                    ChangeOrder(orderStop4, orderStop4.Quantity, 0, high);
            }


        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnTrail50Click(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Flat) return;

            if (IsOrderIsActive(orderStop1) && IsOrderActiveCanChangeOrCancel(orderStop1))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop1, orderStop1.Quantity, 0, (GetCurrentBid(0) + orderStop1.StopPrice) / 2);
                }
                else
                    ChangeOrder(orderStop1, orderStop1.Quantity, 0, (GetCurrentAsk(0) + orderStop1.StopPrice) / 2);
            }
            if (IsOrderIsActive(orderStop2) && IsOrderActiveCanChangeOrCancel(orderStop2))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop2, orderStop2.Quantity, 0, (GetCurrentBid(0) + orderStop2.StopPrice) / 2);
                }
                else
                    ChangeOrder(orderStop2, orderStop2.Quantity, 0, (GetCurrentAsk(0) + orderStop2.StopPrice) / 2);
            }
            if (IsOrderIsActive(orderStop3) && IsOrderActiveCanChangeOrCancel(orderStop3))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop3, orderStop3.Quantity, 0, (GetCurrentBid(0) + orderStop3.StopPrice) / 2);
                }
                else
                    ChangeOrder(orderStop3, orderStop3.Quantity, 0, (GetCurrentAsk(0) + orderStop3.StopPrice) / 2);
            }
            if (IsOrderIsActive(orderStop4) && IsOrderActiveCanChangeOrCancel(orderStop4))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop4, orderStop4.Quantity, 0, (GetCurrentBid(0) + orderStop4.StopPrice) / 2);
                }
                else
                    ChangeOrder(orderStop4, orderStop4.Quantity, 0, (GetCurrentAsk(0) + orderStop4.StopPrice) / 2);
            }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnTrail4Click(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Flat) return;
            if (IsOrderIsActive(orderStop4) && IsOrderActiveCanChangeOrCancel(orderStop4))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop4, orderStop4.Quantity, 0, GetCurrentBid(0));
                }
                else
                    ChangeOrder(orderStop4, orderStop4.Quantity, 0, GetCurrentAsk(0));
            }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnTrail3Click(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Flat) return;
            if (IsOrderIsActive(orderStop3) && IsOrderActiveCanChangeOrCancel(orderStop3))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop3, orderStop3.Quantity, 0, GetCurrentBid(0));
                }
                else
                    ChangeOrder(orderStop3, orderStop3.Quantity, 0, GetCurrentAsk(0));
            }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnTrail2Click(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Flat) return;

            if (IsOrderIsActive(orderStop2) && IsOrderActiveCanChangeOrCancel(orderStop2))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop2, orderStop2.Quantity, 0, GetCurrentBid(0));
                }
                else
                    ChangeOrder(orderStop2, orderStop2.Quantity, 0, GetCurrentAsk(0));
            }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnTrail1Click(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Flat) return;

            if (IsOrderIsActive(orderStop1) && IsOrderActiveCanChangeOrCancel(orderStop1))
            {
                if (Position.MarketPosition == MarketPosition.Long)
                {
                    ChangeOrder(orderStop1, orderStop1.Quantity, 0, GetCurrentBid(0));
                }
                else
                    ChangeOrder(orderStop1, orderStop1.Quantity, 0, GetCurrentAsk(0));
            }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnBuyClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Long) return;

            if (base.IsTradeWorkFlowCanGoLong())
            {
                TriggerCustomEvent(base.TradeWorkFlowNewOrderCustom, AlgoSystemUserActions.Buy);
            }

        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnSellClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            if (Position.MarketPosition == MarketPosition.Short) return;

            if (base.IsTradeWorkFlowCanGoShort())
            {
                TriggerCustomEvent(base.TradeWorkFlowNewOrderCustom, AlgoSystemUserActions.Sell);
            }

        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnOCOBreakoutClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;

            if (base.IsTradeWorkFlowCanEntryOCO())
            {
                TriggerCustomEvent(base.TradeWorkFlowNewOrderCustom, AlgoSystemUserActions.EntryOCO);
            }
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnCloseClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            TriggerCustomEvent(base.FlattenAndReset, null);
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnBreakEvenClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;

            base.Position.BreakEven(OrdersStopLoss);
        }
        public virtual void WPFControlsChartPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!wPFControlsIsPanelActive || base.ATSAlgoSystemState != AlgoSystemState.Realtime) return;

            wPFControlsClickPoint.X = ChartingExtensions.ConvertToHorizontalPixels(e.GetPosition(ChartControl as IInputElement).X, ChartControl.PresentationSource);
            wPFControlsClickPoint.Y = ChartingExtensions.ConvertToVerticalPixels(e.GetPosition(ChartControl as IInputElement).Y, ChartControl.PresentationSource);
            wPFControlsConvertedPrice = Instrument.MasterInstrument.RoundToTickSize(wPFControlsChartScale.GetValueByY((float)wPFControlsClickPoint.Y));
            //convertedTime = ChartControl.GetTimeByX((int)clickPoint.X);
            //convertedTime = ChartControl.GetTimeBySlotIndex((int)ChartControl.GetSlotIndexByX((int)clickPoint.X));

            //>>TO DO which checkbox is SelectedV or none

            if (PriceZoneFilterUpperIsChecked)
            {
                Draw.HorizontalLine(this, "PZU", wPFControlsConvertedPrice, Brushes.Lime, DashStyleHelper.Dash, 5);
                PriceZoneFilterUpperIsChecked = false;
                PriceZoneFilterUpper = wPFControlsConvertedPrice;
            }

            if (PriceZoneFilterLowerIsChecked)
            {
                Draw.HorizontalLine(this, "PZL", wPFControlsConvertedPrice, Brushes.Red, DashStyleHelper.Dash, 5);
                PriceZoneFilterLowerIsChecked = false;
                PriceZoneFilterLower = wPFControlsConvertedPrice;
            }

            // trigger the chart invalidate so that the render loop starts even if there is no data being received
            ForceRefresh();
            e.Handled = true;
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnBoxUpperClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            //throw new NotImplementedException("Sorry Box Breakout Not Implemented Yet. Coming Soon in an update.");
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnBoxLowerClick(object sender, RoutedEventArgs e)
        {
            if (ATSAlgoSystemState != AlgoSystemState.Realtime) return;
            //throw new NotImplementedException("Sorry Box Breakout Not Implemented Yet. Coming Soon in an update.");
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnBoxClearClick(object sender, RoutedEventArgs e)
        {
            PriceZoneFilterUpperIsChecked = false;
            PriceZoneFilterLowerIsChecked = false;
            PriceZoneFilterUpper = 0;
            PriceZoneFilterLower = 0;

            RemoveDrawObject("PZU");
            RemoveDrawObject("PZL");
            ForceRefresh();
        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnAutoShortClick(object sender, RoutedEventArgs e)
        {

        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnAutoLongClick(object sender, RoutedEventArgs e)
        {

        }
        public virtual void WPFControlsUserControlNinjaBuddy1_OnAutoClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        #endregion
        #endregion
    }

}

