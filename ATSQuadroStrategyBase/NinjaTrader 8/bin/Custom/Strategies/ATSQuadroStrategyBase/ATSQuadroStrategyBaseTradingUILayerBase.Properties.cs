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
using System.Xml.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NinjaTrader.NinjaScript.Strategies
{
    /// <summary>
    /// AlgoSystemTradingUILayerBase contains properties, fields etc of partial class
    /// </summary>
    public abstract partial class AlgoSystemTradingUILayerBase : AlgoSystemTradingRulesLayerBase
    {

        #region NinjaBuddy Panel, Easy Config and toolbar
        private System.Windows.Media.SolidColorBrush activeBackgroundDarkGray;
        private System.Windows.Media.SolidColorBrush backGroundMediumGray;
        private System.Windows.Media.SolidColorBrush controlLightGray;
        private System.Windows.Media.SolidColorBrush textColor;

        private string wPFControlsStrategyId = string.Empty;
        private System.Windows.Controls.Button wPFControlsButtonNinjaBuddy, wPFControlsButtonEasyConfig, wPFControlsButtonNews;
        private string wPFControlsButtonNinjaBuddyAutomationID = string.Empty, wPFControlsButtonEasyConfigAutomationID = string.Empty, wPFControlsButtonNewsAutomationID = string.Empty;

        private System.Windows.Controls.Grid wPFControlsChartGrid;
        private NinjaTrader.Gui.Chart.ChartTab chartTab;
        private Gui.Chart.ChartTrader wPFControlsChartTrader;
        private int chartTraderStartColumn;
        private NinjaTrader.Gui.Chart.Chart wPFControlsChartWindow;
        private System.Windows.Controls.ToolBar wPFControlsChartWindowToolBar;
        private bool wPFControlsIsPanelActive = false;
        private System.Windows.Controls.TabItem wPFControlsTabItem;
        private UserControlNinjaBuddy wPFControlsUserControlNinjaBuddy1;

        private ChartScale wPFControlsChartScale;
        private Point wPFControlsClickPoint = new Point();
        private double wPFControlsConvertedPrice;
        #endregion

        #region Visual Indicators
        private ATSIndicatorQSBStrategyInfoBar aTSIndicatorQSBStrategyInfoBar;
        private ATSIndicatorQSBStrategyVisualiser aTSIndicatorQSBStrategyVisualiser;
        #endregion

        #region NinjaBuddyControls



        [Display(Order = 0, Name = "Trade Management Enabled", Description = "Trade Management Enabled", GroupName = "Auto Trade Control")]
        public override bool IsTradeManagementEnabled
        {
            get
            {
                return base.IsTradeManagementEnabled;
            }
            set
            {
                base.IsTradeManagementEnabled = value;
            }
        }


        [Browsable(true)]
        [Display(Name = "Strategy Auto Trading", Description = "AutoTrading On/Off", Order = 2, GroupName = "Auto Trade Control")]
        public override bool StratCanTrade
        {
            get
            {
                return base.StratCanTrade;
            }
            set
            {
                base.StratCanTrade = value;
            }
        }


        [Display(Order = 1, Name = "StratCanTradeLong", Description = "StratCanTradeLong", GroupName = "Auto Trade Control")]
        public override bool StratCanTradeLong
        {
            get
            {
                return base.StratCanTradeLong;
            }
            set
            {
                base.StratCanTradeLong = value;
            }
        }


        [Display(Order = 1, Name = "StratCanTradeShort", Description = "StratCanTradeShort", GroupName = "Auto Trade Control")]
        public override bool StratCanTradeShort
        {
            get
            {
                return base.StratCanTradeShort;
            }
            set
            {
                base.StratCanTradeShort = value;
            }
        }

        [Display(GroupName = "NinjaBuddy Trade Manager", Order = 1, Name = "Show AST.QSB Trade Manager", Description = "Show AST.QSB Trade Manager Trade UI")]
        public bool ShowNinjaBuddyUI
        {
            get;set;
        }

        [XmlIgnore, Browsable(false)]
        public string PriceZoneFilterUpperString
        {
            get { return FormatPriceMarker(PriceZoneFilterUpper); }
        }

        [XmlIgnore, Browsable(false)]
        public string PriceZoneFilterLowerString
        {
            get { return FormatPriceMarker(PriceZoneFilterLower); }
        }

        private bool priceZoneFilterUpperIsChecked = false;

        [XmlIgnore, Browsable(false)]
        public bool PriceZoneFilterUpperIsChecked
        {
            get
            {
                return this.priceZoneFilterUpperIsChecked;
            }
            set
            {
                if (value != this.priceZoneFilterUpperIsChecked)
                {
                    this.priceZoneFilterUpperIsChecked = value;
                    this.NotifyPropertyChanged("PriceZoneFilterUpperIsChecked");
                }
                if (value)
                    PriceZoneFilterLowerIsChecked = false;
            }
        }

        private bool priceZoneFilterLowerIsChecked = false;

        [XmlIgnore, Browsable(false)]
        public bool PriceZoneFilterLowerIsChecked
        {
            get
            {
                return this.priceZoneFilterLowerIsChecked;
            }
            set
            {
                if (value != this.priceZoneFilterLowerIsChecked)
                {
                    this.priceZoneFilterLowerIsChecked = value;
                    this.NotifyPropertyChanged("PriceZoneFilterLowerIsChecked");
                }
                if (value)
                    PriceZoneFilterUpperIsChecked = false;

            }
        }


        #endregion

        #region BoxBreakout
        private int priceZoneBoxBreakoutModuleOnOff = 0;

        [Display(Name = "Box Breakout Mode", Description = "Box Breakout Mode 0:Off, 1:On", Order = 4, GroupName = "Trade Entry BoxBreakout")]
        public int PriceZoneBoxBreakoutModuleOnOff
        {
            get
            {
                return this.priceZoneBoxBreakoutModuleOnOff;
            }
            set
            {
                this.priceZoneBoxBreakoutModuleOnOff = Math.Max(0, Math.Min(1, value));
            }
        }

        private int priceZoneBoxBreakoutLookBack = 1;
        [Display(Name = "Box Breakout Look Back", Description = "Box Breakout Signal Look Back 1 current bar closed to 50 bars ago - Scan for the most recent signal long or short within n bars looking backwards from the current closed bar to the last box Break out Signal. Useful for loosening the window for logic when using entry filters in addition to the Box Break out Entry Signal", Order = 1, GroupName = "Trade Entry BoxBreakout")]
        public int PriceZoneBoxBreakoutLookBack
        {
            get
            {
                return this.priceZoneBoxBreakoutLookBack;
            }
            set
            {
                this.priceZoneBoxBreakoutLookBack = Math.Max(0, Math.Min(50, value));
            }
        }

        private double priceZoneFilterUpper = 0;

        [XmlIgnore]
        [Display(Name = "Box Breakout Upper Price", Description = "Price Zone Box Break Out Upper Price - realtime trading only", Order = 2, GroupName = "Trade Entry Box Break Out")]
        public double PriceZoneFilterUpper
        {
            get
            {
                return this.priceZoneFilterUpper;
            }
            set
            {
                if (value != this.priceZoneFilterUpper)
                {
                    this.priceZoneFilterUpper = value;
                    this.NotifyPropertyChanged("PriceZoneFilterUpperString");
                }
            }
        }

        private double priceZoneFilterLower = 0;

        [XmlIgnore]
        [Display(Name = "Box Breakout Lower Price", Description = "Price Zone Box Break Out Lower Price - realtime trading only", Order = 3, GroupName = "Trade Entry Box Break Out")]
        public double PriceZoneFilterLower
        {
            get
            {
                return this.priceZoneFilterLower;
            }
            set
            {
                if (value != this.priceZoneFilterLower)
                {
                    this.priceZoneFilterLower = value;
                    this.NotifyPropertyChanged("PriceZoneFilterLowerString");
                }
            }
        }
        #endregion


    }

}

