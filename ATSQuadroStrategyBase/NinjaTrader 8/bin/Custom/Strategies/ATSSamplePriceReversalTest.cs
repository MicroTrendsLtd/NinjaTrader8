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
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion


namespace NinjaTrader.NinjaScript.Strategies
{

    /// <summary>
    /// A quick demo using the ATSQuadroStrategyBase as a basic NT8 unmanaged mode strategy foundation
    /// </summary>
    public class ATSSamplePriceReversalTest : ATSQuadroStrategyBase
    {
        private int signalBars = 3;
        private int trailLookBackBars = 3;
        private double longStopPrice = 0, shortStopPrice = 0;
        private int stopSize = 28;

        protected override void OnStateChange()
        {
            base.OnStateChange();

            if (State == State.SetDefaults)
            {
                Description = @"ATSSamplePriceReversalTest using the ATS.NT8.ATSQuadroStrategyBase Strategy Foundation";
                Name = "ATSSamplePriceReversalTest";

                TradeSignalType1 = 1;
            }
            else if (State == State.Configure)
            {
            }
            else if (State == State.DataLoaded)
            {

            }
        }


        protected override void OnAccountItemUpdate(Account account, AccountItem accountItem, double value)
        {
            base.OnAccountItemUpdate(account, accountItem, value);
        }

        protected override void OnConnectionStatusUpdate(ConnectionStatusEventArgs connectionStatusUpdate)
        {
            base.OnConnectionStatusUpdate(connectionStatusUpdate);
        }

        protected override void OnMarketData(MarketDataEventArgs marketDataUpdate)
        {
            base.OnMarketData(marketDataUpdate);
        }

        protected override void OnBarUpdate()
        {
            //Add your custom strategy logic here.
            if (CurrentBar < BarsRequiredToTrade)
                return;

            //Trade Engine Signal State to pass in
            AlgoSignalAction = AlgoSignalAction.None;

            //Price mode
            if (TradeSignalType1 == 1)
            {
                if (base.Position.MarketPosition != MarketPosition.Long && Close[0] > Open[0])
                    AlgoSignalAction = AlgoSignalAction.GoLong;
                else if (base.Position.MarketPosition != MarketPosition.Short && Close[0] < Open[0])
                    AlgoSignalAction = AlgoSignalAction.GoShort;
            }


            base.OnBarUpdate();

        }


        protected override void OnOrderUpdate(Order order, double limitPrice, double stopPrice, int quantity, int filled, double averageFillPrice, OrderState orderState, DateTime time, ErrorCode error, string comment)
        {
            base.OnOrderUpdate(order, limitPrice, stopPrice, quantity, filled, averageFillPrice, orderState, time, error, comment);
        }

        protected override void OnExecutionUpdate(Execution execution, string executionId, double price, int quantity, MarketPosition marketPosition, string orderId, DateTime time)
        {
            base.OnExecutionUpdate(execution, executionId, price, quantity, marketPosition, orderId, time);
        }

        public override bool OnPreTradeEntryValidate(bool isLong)
        {
            //if long or short trade coming then check accoutn balance or time or other rules and return true to allow the trade or false to block the trade
            if (isLong)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public override void OnStrategyTradeWorkFlowUpdated(StrategyTradeWorkFlowUpdatedEventArgs e)
        {
            //here you can do stuff based on the workflow state update
            if (base.TradeWorkFlow == StrategyTradeWorkFlowState.ExitTrade)
            {
                base.BackBrush = Brushes.Gray;
            }
        }


        public override Order SubmitShort(string signal)
        {
            orderEntry = SubmitOrderUnmanaged(0, OrderAction.SellShort, OrderType.Market, 4, 0, 0, String.Empty, signal);
            return orderEntry;
        }

        public override Order SubmitLong(string signal)
        {
            orderEntry = SubmitOrderUnmanaged(0, OrderAction.Buy, OrderType.Market, 4, 0, 0, String.Empty, signal);
            return orderEntry;
        }



        public override void SubmitProfitTarget(Order orderEntry, string oCOId)
        {
            if (orderEntry.OrderAction == OrderAction.Buy)
            {
                string str = (orderEntry != null) ? orderEntry.Name.Replace("↑", string.Empty) : "Long";
                str = str.Substring(3);
                double price = orderEntry.AverageFillPrice + (20 * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundToTickSize(price);

                base.orderTarget1 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO1.{1}", str, oCOId), "↓Trg1" + str);

                base.orderTarget2 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO2.{1}", str, oCOId), "↓Trg2" + str);


                price = orderEntry.AverageFillPrice + (30 * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundToTickSize(price);

                base.orderTarget3 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO3.{1}", str, oCOId), "↓Trg3" + str);


                price = orderEntry.AverageFillPrice + (50 * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundToTickSize(price);

                base.orderTarget4 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO4.{1}", str, oCOId), "↓Trg4" + str);

            }
            else if (orderEntry.OrderAction == OrderAction.SellShort)
            {
                string str2 = (orderEntry != null) ? orderEntry.Name.Replace("↓", string.Empty) : "Short";
                str2 = str2.Substring(3);
                double price = orderEntry.AverageFillPrice - (20 * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundToTickSize(price);

                base.orderTarget1 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO1.{1}", str2, oCOId), "↑Trg1" + str2);

                base.orderTarget2 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO2.{1}", str2, oCOId), "↑Trg2" + str2);


                price = orderEntry.AverageFillPrice - (30 * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundToTickSize(price);

                base.orderTarget3 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO3.{1}", str2, oCOId), "↑Trg3" + str2);


                price = orderEntry.AverageFillPrice - (50 * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundToTickSize(price);

                base.orderTarget4 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, price, 0.0, string.Format("{0}.OCO4.{1}", str2, base.oCOId), "↑Trg4" + str2);



            }

        }


        public override void SubmitStopLoss(Order orderEntry)
        {
            if (orderEntry.OrderAction == OrderAction.Buy)
            {
                string str = (orderEntry != null) ? orderEntry.Name.Replace("↑", string.Empty) : "Long";
                str = str.Substring(3);

                double price = orderEntry.AverageFillPrice - (stopSize * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundDownToTickSize(price);

                base.orderStop1 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO1.{1}", str, base.oCOId), "↓Stp1" + str);
                base.orderStop2 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO2.{1}", str, base.oCOId), "↓Stp2" + str);
                base.orderStop3 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO3.{1}", str, base.oCOId), "↓Stp3" + str);
                base.orderStop4 = base.SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO4.{1}", str, base.oCOId), "↓Stp4" + str);

            }
            else if (orderEntry.OrderAction == OrderAction.SellShort)
            {
                string str2 = (orderEntry != null) ? orderEntry.Name.Replace("↓", string.Empty) : "Short";
                str2 = str2.Substring(3);

                double price = orderEntry.AverageFillPrice + (stopSize * base.TickSize);
                price = base.Instrument.MasterInstrument.RoundToTickSize(price);
                base.orderStop1 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO1.{1}", str2, base.oCOId), "↑Stp1" + str2);
                base.orderStop2 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO2.{1}", str2, base.oCOId), "↑Stp2" + str2);
                base.orderStop3 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO3.{1}", str2, base.oCOId), "↑Stp3" + str2);
                base.orderStop4 = base.SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, price, price, string.Format("{0}.OCO4.{1}", str2, base.oCOId), "↑Stp4" + str2);

            }

        }


        public override bool SubmitStopLossWillOccur()
        {
            return true;
        }

        public override bool SubmitProfitTargetWillOccur()
        {

            return true;

        }



        public override void TradeManagement(double lastPrice)
        {
            //if some rule says to exit  you can call ito the workflow and execute an exit
            // base.TradeWorkFlow = base.ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTrade);
            // this.inManageCurrentPosition = false;  unlock
            //return before the next section if so... return

            try
            {

                if (base.Position.MarketPosition == MarketPosition.Long)
                {

                    //here we can test for excursion for trailing stops
                    double ticksExcursion = (int)Math.Round((double)((lastPrice - base.Position.AveragePrice) / base.TickSize), 0);
                    if (ticksExcursion > 0)
                    {
                        //get lowset low price within n bars set by trailLookBackBars
                        longStopPrice = Lows[0][LowestBar(base.Lows[0], Math.Min(trailLookBackBars, CurrentBars[0]))];
                        longStopPrice = Math.Min(GetCurrentBid(0) - TickSize, longStopPrice);
                        longStopPrice = Instrument.MasterInstrument.RoundToTickSize(longStopPrice);

                        //test the stoplosses are active and can be changed, compate stop price and then modify the order if required
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop1))
                        {
                            if (this.longStopPrice > base.orderStop1.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop1, base.orderStop1.Quantity, this.longStopPrice, this.longStopPrice);
                            }
                        }
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop2))
                        {
                            if (this.longStopPrice > base.orderStop2.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop2, base.orderStop2.Quantity, this.longStopPrice, this.longStopPrice);
                            }
                        }
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop3))
                        {
                            if (this.longStopPrice > base.orderStop3.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop3, base.orderStop3.Quantity, this.longStopPrice, this.longStopPrice);
                            }
                        }
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop4))
                        {
                            if (this.longStopPrice > base.orderStop4.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop4, base.orderStop4.Quantity, this.longStopPrice, this.longStopPrice);
                            }
                        }


                    }


                }
                else if (base.Position.MarketPosition == MarketPosition.Short)
                {
                    double ticksExcursion = (int)Math.Round((double)((base.Position.AveragePrice - lastPrice) / base.TickSize), 0);
                    if (ticksExcursion > 0)
                    {

                        //get highest high price within n bars set by trailLookBackBars
                        shortStopPrice = Highs[0][HighestBar(base.Highs[0], Math.Min(trailLookBackBars, CurrentBars[0]))];
                        shortStopPrice = Math.Max(GetCurrentAsk(0) + TickSize, shortStopPrice);
                        shortStopPrice = Instrument.MasterInstrument.RoundToTickSize(shortStopPrice);

                        //test the stoplosses are active and can be changed, compare stop price and then modify the order if required
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop1))
                        {
                            if (this.shortStopPrice < base.orderStop1.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop1, base.orderStop1.Quantity, this.shortStopPrice, this.shortStopPrice);
                            }
                        }
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop2))
                        {
                            if (this.shortStopPrice < base.orderStop2.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop2, base.orderStop2.Quantity, this.shortStopPrice, this.shortStopPrice);
                            }
                        }
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop3))
                        {
                            if (this.shortStopPrice < base.orderStop3.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop3, base.orderStop3.Quantity, this.shortStopPrice, this.shortStopPrice);
                            }
                        }
                        if (base.IsOrderActiveCanChangeOrCancel(base.orderStop4))
                        {
                            if (this.shortStopPrice < base.orderStop4.StopPrice)
                            {
                                base.ChangeOrder(base.orderStop4, base.orderStop4.Quantity, this.shortStopPrice, this.shortStopPrice);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Print(string.Format("TradeManagement >> Error >> {0}" + ex.ToString()));
            }


        }



        #region Properties



        [Range(0, 1), NinjaScriptProperty]
        [Display(ResourceType = typeof(Custom.Resource), Name = "SignalType1", Description = "0:off, 1:Double MA Crossover", GroupName = "NinjaScriptStrategyParameters", Order = 2)]
        public int TradeSignalType1
        { get; set; }





        #endregion

    }
}
