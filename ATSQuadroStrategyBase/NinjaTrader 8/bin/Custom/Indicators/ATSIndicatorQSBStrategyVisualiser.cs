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
    public class ATSIndicatorQSBStrategyVisualiser : Indicator
    {
        protected override void OnStateChange()
        {

            try
            {
                if (State == State.SetDefaults)
                {
                    Description = @"ATSIndicatorQSBStrategyVisualiser";
                    Name = "ATS.QSB.StrategyVisualiser";
                    Calculate = Calculate.OnBarClose;
                    IsOverlay = true;
                    DisplayInDataBox = true;
                    DrawOnPricePanel = true;
                    DrawHorizontalGridLines = true;
                    DrawVerticalGridLines = true;
                    PaintPriceMarkers = true;
                    ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                    //Disable this property if your indicator requires custom values that cumulate with each new market data event. 
                    //See Help Guide for additional information.
                    IsSuspendedWhileInactive = false;
                    PaintPriceMarkers = false;

                    AddPlot(new Stroke(Brushes.DarkGreen, 3), PlotStyle.TriangleRight, "Long Pos");
                    AddPlot(new Stroke(Brushes.DarkRed, 3), PlotStyle.TriangleRight, "Short Pos");

                    AddPlot(new Stroke(Brushes.HotPink, 1), PlotStyle.Dot, "Stop Loss 1");
                    AddPlot(new Stroke(Brushes.HotPink, 1), PlotStyle.Dot, "Stop Loss 2");
                    AddPlot(new Stroke(Brushes.HotPink, 1), PlotStyle.Dot, "Stop Loss 3");
                    AddPlot(new Stroke(Brushes.HotPink, 1), PlotStyle.Dot, "Stop Loss 4");

                    AddPlot(new Stroke(Brushes.LimeGreen, 1), PlotStyle.Dot, "Profit Target 1");
                    AddPlot(new Stroke(Brushes.LimeGreen, 1), PlotStyle.Dot, "Profit Target 2");
                    AddPlot(new Stroke(Brushes.LimeGreen, 1), PlotStyle.Dot, "Profit Target 3");
                    AddPlot(new Stroke(Brushes.LimeGreen, 1), PlotStyle.Dot, "Profit Target 4");

                    AddPlot(new Stroke(Brushes.DarkGreen, 2), PlotStyle.TriangleUp, "Entry Long");
                    AddPlot(new Stroke(Brushes.DarkRed, 2), PlotStyle.TriangleDown, "Entry Short");
                    AddPlot(new Stroke(Brushes.Gray, 4), PlotStyle.Dot, "Exit");

                    IsAutoScale = false;

                }
                else if (State == State.Configure)
                {
                    //                IsAutoScale = false;
                    IsAutoScale = false;
                }
            }
            catch (Exception ex)
            {
                Print(ex.ToString());
            }


        }

        protected override void OnBarUpdate()
        {

            //Add your custom indicator logic here.
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
        public Series<double> LongPos
        {
            get { return Values[0]; }
            set { Values[0] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> ShortPos
        {
            get { return Values[1]; }
            set { Values[1] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> StopLoss1
        {
            get { return Values[2]; }
            set { Values[2] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> StopLoss2
        {
            get { return Values[3]; }
            set { Values[3] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> StopLoss3
        {
            get { return Values[4]; }
            set { Values[4] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> StopLoss4
        {
            get { return Values[5]; }
            set { Values[5] = value; }
        }



        [Browsable(false)]
        [XmlIgnore]
        public Series<double> ProfitTarget1
        {
            get { return Values[6]; }
            set { Values[6] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> ProfitTarget2
        {
            get { return Values[7]; }
            set { Values[7] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> ProfitTarget3
        {
            get { return Values[8]; }
            set { Values[8] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> ProfitTarget4
        {
            get { return Values[9]; }
            set { Values[9] = value; }
        }





        [Browsable(false)]
        [XmlIgnore]
        public Series<double> EntryLong
        {
            get { return Values[10]; }
            set { Values[10] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> EntryShort
        {
            get { return Values[11]; }
            set { Values[11] = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Series<double> Exit
        {
            get { return Values[12]; }
            set { Values[12] = value; }
        }

    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private ATSIndicatorQSBStrategyVisualiser[] cacheATSIndicatorQSBStrategyVisualiser;
		public ATSIndicatorQSBStrategyVisualiser ATSIndicatorQSBStrategyVisualiser()
		{
			return ATSIndicatorQSBStrategyVisualiser(Input);
		}

		public ATSIndicatorQSBStrategyVisualiser ATSIndicatorQSBStrategyVisualiser(ISeries<double> input)
		{
			if (cacheATSIndicatorQSBStrategyVisualiser != null)
				for (int idx = 0; idx < cacheATSIndicatorQSBStrategyVisualiser.Length; idx++)
					if (cacheATSIndicatorQSBStrategyVisualiser[idx] != null &&  cacheATSIndicatorQSBStrategyVisualiser[idx].EqualsInput(input))
						return cacheATSIndicatorQSBStrategyVisualiser[idx];
			return CacheIndicator<ATSIndicatorQSBStrategyVisualiser>(new ATSIndicatorQSBStrategyVisualiser(), input, ref cacheATSIndicatorQSBStrategyVisualiser);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.ATSIndicatorQSBStrategyVisualiser ATSIndicatorQSBStrategyVisualiser()
		{
			return indicator.ATSIndicatorQSBStrategyVisualiser(Input);
		}

		public Indicators.ATSIndicatorQSBStrategyVisualiser ATSIndicatorQSBStrategyVisualiser(ISeries<double> input )
		{
			return indicator.ATSIndicatorQSBStrategyVisualiser(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.ATSIndicatorQSBStrategyVisualiser ATSIndicatorQSBStrategyVisualiser()
		{
			return indicator.ATSIndicatorQSBStrategyVisualiser(Input);
		}

		public Indicators.ATSIndicatorQSBStrategyVisualiser ATSIndicatorQSBStrategyVisualiser(ISeries<double> input )
		{
			return indicator.ATSIndicatorQSBStrategyVisualiser(input);
		}
	}
}

#endregion
