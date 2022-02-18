#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
    public sealed class ATSIndicatorQSBStrategyInfoBar : Indicator
    {

        private SimpleFont simpleFont1 = null;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"ATSIndicatorQSBStrategyInfoBar";
                Name = "ATS.QSB.StrategyInfoBar";
                Calculate = Calculate.OnBarClose;
                IsOverlay = false;
                DisplayInDataBox = true;
                DrawOnPricePanel = false;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                //Disable this property if your indicator requires custom values that cumulate with each new market data event. 
                //See Help Guide for additional information.
                IsSuspendedWhileInactive = false;
                this.IsChartOnly = false;
                AddPlot(new Stroke(Brushes.White, 2), PlotStyle.Line, "PL");
                AddLine(Brushes.White, 0, "0");
                DrawOnPricePanel = false;
            }
            else if (State == State.DataLoaded)
            {
                if (ChartControl == null) return;
                DrawOnPricePanel = false;

                ChartControl.Dispatcher.InvokeAsync((Action)(() =>
                {
                    simpleFont1 = new SimpleFont();
                    simpleFont1.Family = ChartControl.ChartPanels[0].FontFamily;
                    simpleFont1.Size = ChartControl.ChartPanels[0].FontSize;
                }));
            }
            else if (State == State.Realtime)
            {
                
            }
            Calculate = Calculate.OnBarClose;
        }

      


        public override void OnCalculateMinMax()
        {
            try
            {
                base.OnCalculateMinMax();

            }
            catch (Exception ex)
            {
                Print("ATSIndicatorQSBStrategyInfoBar > OnCalculateMinMax > " + ex.ToString());
            }

        }




        protected override void OnBarUpdate()
        {
            if (State != State.Realtime || ChartControl == null) return;
            

        }

       

        public override string DisplayName
        {
            get
            {
                return string.Empty;
            }
        }
        public override string ToString()
        {
            return this.Name;
        }


        [Browsable(false)]
        [XmlIgnore]
        public Series<double> PNL
        {
            get { return Values[0]; }
            set { Values[0] = value; }
        }

   
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private ATSIndicatorQSBStrategyInfoBar[] cacheATSIndicatorQSBStrategyInfoBar;
		public ATSIndicatorQSBStrategyInfoBar ATSIndicatorQSBStrategyInfoBar()
		{
			return ATSIndicatorQSBStrategyInfoBar(Input);
		}

		public ATSIndicatorQSBStrategyInfoBar ATSIndicatorQSBStrategyInfoBar(ISeries<double> input)
		{
			if (cacheATSIndicatorQSBStrategyInfoBar != null)
				for (int idx = 0; idx < cacheATSIndicatorQSBStrategyInfoBar.Length; idx++)
					if (cacheATSIndicatorQSBStrategyInfoBar[idx] != null &&  cacheATSIndicatorQSBStrategyInfoBar[idx].EqualsInput(input))
						return cacheATSIndicatorQSBStrategyInfoBar[idx];
			return CacheIndicator<ATSIndicatorQSBStrategyInfoBar>(new ATSIndicatorQSBStrategyInfoBar(), input, ref cacheATSIndicatorQSBStrategyInfoBar);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.ATSIndicatorQSBStrategyInfoBar ATSIndicatorQSBStrategyInfoBar()
		{
			return indicator.ATSIndicatorQSBStrategyInfoBar(Input);
		}

		public Indicators.ATSIndicatorQSBStrategyInfoBar ATSIndicatorQSBStrategyInfoBar(ISeries<double> input )
		{
			return indicator.ATSIndicatorQSBStrategyInfoBar(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.ATSIndicatorQSBStrategyInfoBar ATSIndicatorQSBStrategyInfoBar()
		{
			return indicator.ATSIndicatorQSBStrategyInfoBar(Input);
		}

		public Indicators.ATSIndicatorQSBStrategyInfoBar ATSIndicatorQSBStrategyInfoBar(ISeries<double> input )
		{
			return indicator.ATSIndicatorQSBStrategyInfoBar(input);
		}
	}
}

#endregion
