//MIT License/OPEN SOURCE LICENSE
//Copyright(C) 2020, Algo Trading Systems LLC <www.algotradingsystems.net>
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notices, this permission notice shall be included in all copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//ABOUT ATSQuadroStrategyBase
//Algo Trading Systems (ATS) reserves the right to modify or overwrite this NinjaScript component with each release without notice.
//Legal Forum: In using this product you agree to the terms and NYC Jurisdiction
//Developer: Tom Leeson of MicroTrends LTd www.microtrends.pro
//GIT: https://github.com/MicroTrendsLtd/NinjaTrader8/
//Suport: support@microtrends.co or via GIT
//Bugs: please report at GIT
//Collaboration: All welcome at GIT
//Updates: Visit GIT open source project code for the latest.
//About: ATSQuadroStrategyBase is a NinjaTrader 8 Strategy unmanaged mode trade engine base foundation for futures, comprising of 4 Bracket capacity, all In scale out non position compounding,  prevents overfills and builds on functionality provided by the Managed approach for NinjaTrader Strategies. 
//Version: 8
//History: See gitHub history
//GitHub Collaborators: jmscraig


#region Using declarations
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using NinjaTrader.Core;
#endregion

namespace NinjaTrader.NinjaScript.Strategies
{
    #region enums

    //[TypeConverter(typeof(CoreEnumConverter))]
    public enum AlgoSystemState
    {
        None = 0,
        Starting = 1,
        DataLoaded = 2,
        Historical = 3,
        Transition = 4,
        HisTradeRT = 5,
        Realtime = 6,
        Terminated = 7
    }

    //[TypeConverter(typeof(CoreEnumConverter))]
    public enum AlgoSystemMode
    {
        UnKnown = 0,
        Sim = 1,
        Test = 2,
        Replay = 3,
        Live = 4
    }

    public enum StrategyTradeWorkFlowState
    {
        Waiting = 0,
        GoLong,
        GoLongCancelWorkingOrders,
        GoLongCancelWorkingOrdersPending,
        GoLongCancelWorkingOrdersConfirmed,
        GoLongClosePositions,
        GoLongClosedPositionsPending,
        GoLongClosedPositionsConfirmed,
        GoLongSubmitOrder,
        GoLongValidationRejected,
        GoLongSubmitOrderPending,
        GoLongSubmitOrderWorking,
        GoLongSubmitOrderFilled,
        GoLongFilledQuantityTest1,
        GoLongFilledQuantityTest1Ok,
        GoLongFilledQuantityTest1Fail,
        GoLongFilledQuantityTest2,
        GoLongFilledQuantityTest2Ok,
        GoLongFilledQuantityTest2Fail,
        GoLongFilledQuantityVerified,
        GoLongPlaceStops,
        GoLongPlaceStopsPending,
        GoLongPlaceStopsConfirmed,
        GoLongPlaceProfitTargets,
        GoLongPlaceProfitTargetsPending,
        GoLongPlaceProfitTargetsConfirmed,
        GoShort,
        GoShortCancelWorkingOrders,
        GoShortCancelWorkingOrdersPending,
        GoShortCancelWorkingOrdersConfirmed,
        GoShortClosePositions,
        GoShortClosedPositionsPending,
        GoShortClosedPositionsConfirmed,
        GoShortSubmitOrder,
        GoShortValidationRejected,
        GoShortSubmitOrderPending,
        GoShortSubmitOrderWorking,
        GoShortSubmitOrderFilled,
        GoShortFilledQuantityTest1,
        GoShortFilledQuantityTest1Ok,
        GoShortFilledQuantityTest1Fail,
        GoShortFilledQuantityTest2,
        GoShortFilledQuantityTest2Ok,
        GoShortFilledQuantityTest2Fail,
        GoShortFilledQuantityVerified,
        GoShortPlaceStops,
        GoShortPlaceStopsPending,
        GoShortPlaceStopsConfirmed,
        GoShortPlaceProfitTargets,
        GoShortPlaceProfitTargetsPending,
        GoShortPlaceProfitTargetsConfirmed,
        GoOCOLongShort,
        GoOCOLongShortCancelWorkingOrders,
        GoOCOLongShortCancelWorkingOrdersPending,
        GoOCOLongShortCancelWorkingOrdersConfirmed,
        GoOCOLongShortClosePositions,
        GoOCOLongShortClosedPositionsPending,
        GoOCOLongShortClosedPositionsConfirmed,
        GoOCOLongShortSubmitOrder,
        GoOCOLongShortValidationRejected,
        GoOCOLongShortSubmitOrderPending,
        GoOCOLongShortSubmitOrderWorking,
        ExitTradeLong,
        ExitTradeShort,
        ExitTrade,
        ExitTradeCancelWorkingOrders,
        ExitTradeCancelWorkingOrderPending,
        ExitTradeCancelWorkingOrderConfirmed,
        ExitTradeClosePositions,
        ExitTradeClosePositionsPending,
        ExitTradeClosePositionsConfirmed,
        ExitOnCloseOrderPending,
        ExitOnCloseWaitingConfirmation,
        ExitOnCloseOrderFilled,
        ExitOnCloseConfirmed,
        ExitOnTransition,
        ExitOnTransitionCancelWorkingOrders,
        ExitOnTransitionClosePositions,
        ExitOnTransitionWaitingConfirmation,
        ExitOnTransitionComplete,
        ErrorTimeOut = 1000,
        Error = 1001,
        ErrorFlattenAll,
        ErrorFlattenAllPending,
        ErrorFlattenAllConfirmed,
        CycleComplete = 10000,

    }
    #endregion
    #region Event Args
    public sealed class StrategyTradeWorkFlowUpdatedEventArgs : EventArgs
    {
        private StrategyTradeWorkFlowState strategyTradeWorkFlowState = StrategyTradeWorkFlowState.Waiting;

        public StrategyTradeWorkFlowUpdatedEventArgs(StrategyTradeWorkFlowState strategyTradeWorkFlowState)
        {
            this.strategyTradeWorkFlowState = strategyTradeWorkFlowState;
        }
        public StrategyTradeWorkFlowState StrategyTradeWorkFlowState
        {
            get { return strategyTradeWorkFlowState; }
            set { strategyTradeWorkFlowState = value; }
        }
    }
    public sealed class ATSAlgoSystemStateUpdatedEventArgs : EventArgs
    {
        public ATSAlgoSystemStateUpdatedEventArgs(AlgoSystemState aTSAlgoSystemState)
        {
            this.ATSAlgoSystemState = aTSAlgoSystemState;
        }
        public AlgoSystemState ATSAlgoSystemState
        {
            get; set;
        }
    }

    #endregion
    #region SignalActions

    public enum AlgoSignalAction
    {
        None = 0,
        ExitTrade = 1,
        GoLong = 2,
        GoShort = 3,
        ExitTradeLong = 4,
        ExitTradeShort = 5,
        GoOCOEntry = 6
    }

    public class AlgoSignalActionMsq
    {
        public AlgoSignalAction Action = AlgoSignalAction.None;
        public DateTime ActionDateTime = DateTime.Now;
        public string Reason = string.Empty;

        public AlgoSignalActionMsq(AlgoSignalAction action, DateTime actionDateTime, string reason)
        {
            this.Action = action;
            this.ActionDateTime = actionDateTime;
            this.Reason = reason;
        }

        public override string ToString()
        {
            return ActionDateTime.ToString() + " | " + Action.ToString() + " | " + Reason;
        }

    }

    #endregion
    #region DebugTrace
    public class DebugTraceHelper
    {

        private DefaultTraceListener tracing = new DefaultTraceListener();
        public static DebugTraceHelper Default = new DebugTraceHelper();

        public DefaultTraceListener Tracing
        {
            get
            {
                return tracing;

            }

            set
            {
                tracing = value;
            }
        }



        public static void WriteLine(string message)
        {

            Default.Tracing.LogFileName = string.Format("{0}\\trace\\ATS.NT8.{1}{2}{3}.Trace.txt", NinjaTrader.Core.Globals.UserDataDir, DateTime.Now.Year.ToString("d2"), DateTime.Now.Month.ToString("d2"), DateTime.Now.Day.ToString("d2"));
            Default.Tracing.WriteLine(message);
        }

        public static void OpenTraceFile()
        {

            if (File.Exists(Default.Tracing.LogFileName)) System.Diagnostics.Process.Start(Default.Tracing.LogFileName);

        }


        public DebugTraceHelper()
        {
            Tracing.Name = "ASB.NT8.Trace";
            Tracing.LogFileName = string.Format("{0}\\trace\\ASB.NT8.{1}{2}{3}.Trace", NinjaTrader.Core.Globals.UserDataDir, DateTime.Now.Year.ToString("d2"), DateTime.Now.Month.ToString("d2"), DateTime.Now.Day.ToString("d2"));
        }


    }
    #endregion
    #region partial StrategyBase Class
    public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
    {
        //can be handy to have this reference when creating indicator wrappers in derived class instances
        internal Indicators.Indicator StrategyIndicator { get { return indicator; } }
    }
    #endregion

    /// <summary>
    /// ATSQuadroStrategyBase is a NinjaTrader 8 Strategy unmanaged mode trade engine base foundation for futures design and developed by Tom Leeson, MicroTrends LTd, www.microtrends.pro
    /// </summary>
    public abstract class ATSQuadroStrategyBase : Strategy, INotifyPropertyChanged
    {
        #region variables,constants,EventHandlers

        private double lastPrice;
        private DateTime onMarketDataTimeNextAllowed;
        private DateTime onMarketDataBidTimeNextAllowed;
        private readonly object onOrderUpdateLockObject = new object();
        /// <summary>
        /// Role of ordersActiveLockObject: As a Public List OrdersActive requires us to lock on a seperate object token rather than the List object itself. 
        /// </summary>
        private readonly object ordersActiveLockObject = new object();

        private bool inOnMarketData;
        private readonly object inOnMarketDataLock = new object();


        [XmlIgnore]
        public AlgoSignalAction AlgoSignalAction = AlgoSignalAction.None;
        [XmlIgnore]
        public ConnectionStatus connectionStatusOrder = ConnectionStatus.Connected;
        [XmlIgnore]
        public ConnectionStatus connectionStatusPrice = ConnectionStatus.Connected;

        private StrategyTradeWorkFlowState tradeWorkFlow = StrategyTradeWorkFlowState.Waiting;
        private StrategyTradeWorkFlowState tradeWorkFlowPrior = StrategyTradeWorkFlowState.Waiting;

        internal bool IsTradeWorkFlowOnMarketData = false;
        internal bool IsTEQOnMarketData = false;

        protected bool tEQTimerInProgress = false;
        protected bool tEQTimerStarted = false;

        protected long tradeWorkFlowTimerTickCounter = 0;
        protected int tradeWorkFlowTimerInterval = 1000;
        protected int tradeWorkFlowTimerIntervalReset = 100;
        protected int tradeWorkFlowAlarmTimeOutSeconds = 10;
        protected int tradeWorkFlowRetryAlarm = 3;
        protected int tradeWorkFlowRetryCount = 0;

        protected readonly object lockTradeWorkFlowTimerObject = new Object();

        protected int tEQTimerInterval = 1000;
        protected readonly object lockTEQTimerObject = new Object();

        private bool realtimeTradingOnly = false;
        private bool useQueue = false;

        //lock objects
        private readonly object tradeWorkFlowNewOrderLockObject = new Object();
        private readonly object tradeWorkFlowExitTradeLockObject = new Object();
        private readonly object tradeWorkFlowTradeEntryOCOLockObject = new Object();


        private readonly object lockObjectClose = new object();
        private bool isLockPositionClose = false;
        private readonly object lockObjectPositionClose = new object();

        #region orders
        protected const string arrowUp = "↑";
        protected const string arrowDown = "↓";

        protected const string entry1NameLong = "↑EL";
        protected const string entry1NameShort = "↓ES";
        protected const string closeNameLong = "XL";
        protected const string closeNameShort = "XS";
        protected const string closeOrderName = "Close";

        protected const string target1Name = "PT1";
        protected const string target2Name = "PT2";
        protected const string target3Name = "PT3";
        protected const string target4Name = "PT4";
        protected const string stop1Name = "SL1";
        protected const string stop2Name = "SL2";
        protected const string stop3Name = "SL3";
        protected const string stop4Name = "SL4";

        protected const string orderEntryOCOLongName = "↑OEL";
        protected const string orderEntryOCOShortName = "↓OES";


        protected int entryCount = 1;

        protected Order orderEntry = null;
        protected Order orderEntryPrior = null;
        protected Order orderTarget1 = null;
        protected Order orderStop1 = null;
        protected Order orderTarget2 = null;
        protected Order orderStop2 = null;
        protected Order orderTarget3 = null;
        protected Order orderStop3 = null;
        protected Order orderTarget4 = null;
        protected Order orderStop4 = null;
        protected Order orderExit = null;
        protected Order orderClose = null;
        protected Order orderExitOnClose = null;
        protected Order orderEntryOCOLong = null;
        protected Order orderEntryOCOShort = null;

        private List<Order> ordersRT = new List<Order>(1000);
        private List<Order> ordersStopLoss = new List<Order>(4);
        private List<Order> ordersProfitTarget = new List<Order>(4);

        protected string oCOId = Guid.NewGuid().ToString();

        protected string orderEntryName = string.Empty;


        #endregion

        Queue<AlgoSignalActionMsq> q = new Queue<AlgoSignalActionMsq>(10000);
        private bool lockedQueue = false;
        private readonly object queueLockObject = new Object();

        protected bool onErrorShowAlertOnBarUpdate = false;
        protected bool onErrorDisableOnBarUpdate = false;
        protected bool onErrorShowAlertOnTerminate = false;
        protected bool onErrorDumpSettingsToFileOnBarUpdate = false;
        protected bool onErrorDumpSettingsToFileOnTerminate = false;

        protected string errorMsg = string.Empty;
        protected bool errorsOccured = false;
        protected string errorFileName = string.Empty;
        protected string path = string.Empty;

        //StreamWriter writer = null;
        protected string lUID = string.Empty;
        public event EventHandler<StrategyTradeWorkFlowUpdatedEventArgs> StrategyTradeWorkFlowUpdated;
        public event EventHandler<ATSAlgoSystemStateUpdatedEventArgs> ATSAlgoSystemStateUpdated;

        private bool tracing = false;
        private bool showOrderLabels = false;

        //Error Handling 
        private bool orderCancelStopsOnly = true;
        private bool orderCancelInspectEachOrDoBatchCancel = true;
        private bool raiseErrorOnAllOrderRejects = false;
        private readonly object lockObjectMarketData = new object();
        private Currency accountDenomination = Currency.UsDollar;

        #endregion
        #region events and overrides

        public ATSQuadroStrategyBase()
        {
            TradeSignalExpiryInterval = 3;
            IsFlattenOnTransition = false;
            IsTracingModeRealtimeOnly = true;

        }


        protected override void OnStateChange()
        {

            try
            {
                switch (State)
                {
                    case State.SetDefaults:
                        Description = @"ATS.NT8.QuadroStrategyBase";
                        Name = "ATS.NT8.QuadroStrategyBase";
                        Calculate = Calculate.OnBarClose;
                        EntriesPerDirection = 1;
                        EntryHandling = EntryHandling.AllEntries;
                        IsExitOnSessionCloseStrategy = false;
                        ExitOnSessionCloseSeconds = 30;
                        IsFillLimitOnTouch = false;
                        OrderFillResolution = OrderFillResolution.Standard;
                        Slippage = 0;
                        StartBehavior = StartBehavior.WaitUntilFlat;
                        TimeInForce = TimeInForce.Gtc;
                        TraceOrders = true;
                        RealtimeErrorHandling = RealtimeErrorHandling.IgnoreAllErrors;
                        StopTargetHandling = StopTargetHandling.PerEntryExecution;
                        BarsRequiredToTrade = 0;
                        BarsRequiredToPlot = 1;
                        // Disable this property for performance gains in Strategy Analyzer optimizations
                        // See the Help Guide for additional information
                        IsInstantiatedOnEachOptimizationIteration = true;
                        MaximumBarsLookBack = MaximumBarsLookBack.Infinite;
                        IgnoreOverfill = true;
                        IsUnmanaged = true;
                        this.DefaultQuantity = 4;


                        break;
                    case State.Configure:
                        ATSAlgoSystemState = AlgoSystemState.Starting;

                        break;
                    case State.Active:

                        break;
                    case State.DataLoaded:

                        if (tracing)
                            Print("OnStateChange >" + State.ToString());


                        if (Account.Name == Account.BackTestAccountName)
                            ATSAlgoSystemMode = AlgoSystemMode.Test;
                        else if (Account.Name == Account.PlaybackAccountName)
                        {
                            ATSAlgoSystemMode = AlgoSystemMode.Replay;

                        }
                        else
                        {
                            if (Account.Connection == null)
                            {
                                ATSAlgoSystemMode = AlgoSystemMode.Sim;
                            }
                            else
                            {
                                if (Account.Connection.Options.Mode == Mode.Simulation)
                                    ATSAlgoSystemMode = AlgoSystemMode.Sim;
                                else
                                    ATSAlgoSystemMode = AlgoSystemMode.Live;
                            }
                        }
                        ATSAlgoSystemState = AlgoSystemState.DataLoaded;
                        InstrumentFullName = this.Instrument.FullName;


                        break;
                    case State.Historical:
                        if (tracing)
                            Print("OnStateChange >" + State.ToString());

                        ATSAlgoSystemState = AlgoSystemState.Historical;
                        PositionInfo = string.Format("{0}", Position.MarketPosition.ToString());


                        break;
                    case State.Transition:

                        if (tracing)
                            Print("OnStateChange >" + State.ToString());

                        if (ATSAlgoSystemMode == AlgoSystemMode.UnKnown && Account.Connection != null)
                        {
                            if (Account.Connection.Options.Mode == Mode.Live && !string.IsNullOrEmpty(Account.Fcm))
                            {
                                ATSAlgoSystemMode = AlgoSystemMode.Live;
                            }
                            else
                                ATSAlgoSystemMode = AlgoSystemMode.Sim;
                        }

                        ATSAlgoSystemState = AlgoSystemState.Transition;

                        //validate test to the state to realtime with historical orders
                        //NT8 can be realtime state yet historical orders exist...
                        if (Position.MarketPosition != MarketPosition.Flat)
                        {
                            ATSAlgoSystemState = AlgoSystemState.HisTradeRT;
                        }

                        break;
                    case State.Realtime:

                        if (tracing)
                            Print("OnStateChange >" + State.ToString());

                        if (ATSAlgoSystemState == AlgoSystemState.Transition)
                            ATSAlgoSystemState = AlgoSystemState.Realtime;

                        // one time only, as we transition from historical to real-time - doesnt seem to work  for unmanaged mode
                        // the work around was to use order names and reference them OnOrderUpdate
                        //https://ninjatrader.com/support/helpGuides/nt8/?getrealtimeorder.htm



                        if (orderEntry != null)
                        {
                            Order order = GetRealtimeOrder(orderEntry);
                            if (order != null) orderEntry = order;
                        }
                        if (orderClose != null)
                        {
                            Order order = GetRealtimeOrder(orderClose);
                            if (order != null) orderClose = order;
                        }
                        if (orderExit != null)
                        {
                            Order order = GetRealtimeOrder(orderExit);
                            if (order != null) orderExit = order;
                        }
                        if (orderExitOnClose != null)
                        {
                            Order order = GetRealtimeOrder(orderExitOnClose);
                            if (order != null) orderExitOnClose = order;
                        }
                        if (orderStop1 != null)
                        {
                            Order order = GetRealtimeOrder(orderStop1);
                            if (order != null) orderStop1 = order;
                        }
                        if (orderStop2 != null)
                        {
                            Order order = GetRealtimeOrder(orderStop2);
                            if (order != null) orderStop2 = order;
                        }
                        if (orderStop3 != null)
                        {
                            Order order = GetRealtimeOrder(orderStop3);
                            if (order != null) orderStop3 = order;
                        }
                        if (orderStop4 != null)
                        {
                            Order order = GetRealtimeOrder(orderStop4);
                            if (order != null) orderStop4 = order;
                        }
                        if (orderTarget1 != null)
                        {
                            Order order = GetRealtimeOrder(orderTarget1);
                            if (order != null) orderTarget1 = order;
                        }
                        if (orderTarget2 != null)
                        {
                            Order order = GetRealtimeOrder(orderTarget2);
                            if (order != null) orderTarget2 = order;
                        }
                        if (orderTarget3 != null)
                        {
                            Order order = GetRealtimeOrder(orderTarget3);
                            if (order != null) orderTarget3 = order;
                        }
                        if (orderTarget4 != null)
                        {
                            Order order = GetRealtimeOrder(orderTarget4);
                            if (order != null) orderTarget4 = order;
                        }
                        if (orderEntryOCOLong != null)
                        {
                            Order order = GetRealtimeOrder(orderEntryOCOLong);
                            if (order != null) orderEntryOCOLong = order;
                        }
                        if (orderEntryOCOShort != null)
                        {
                            Order order = GetRealtimeOrder(orderEntryOCOShort);
                            if (order != null) orderEntryOCOShort = order;
                        }


                        if (ATSAlgoSystemState == AlgoSystemState.HisTradeRT && IsFlattenOnTransition)
                        {

                            if (tracing)
                                Print("OnStateChange > IsFlattenOnTransition" + State.ToString());

                            TradeWorkFlowTradeExitTransition();

                        }



                        AskPrice = GetCurrentAsk(0);
                        BidPrice = GetCurrentBid(0);

                        PositionInfo = string.Format("{0} {1} @ {2}", Position.MarketPosition.ToString().Substring(0, 1), Position.Quantity, Position.AveragePrice);

                        if (Position.MarketPosition == MarketPosition.Long)
                            PositionState = 1;
                        else
                            PositionState = -1;

                        UnRealizedPL = Position.GetUnrealizedProfitLoss(PerformanceUnit.Currency, lastPrice);

                        break;
                    case State.Terminated:

                        break;
                    case State.Finalized:
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("OnStateChange {0}", ex.ToString());
                Debug.Print(errorMsg);
                Print(errorMsg);
                Log(errorMsg, LogLevel.Error);
            }
        }


        protected override void OnConnectionStatusUpdate(ConnectionStatusEventArgs connectionStatusUpdate)
        {

            if (tracing)
                Print("OnConnectionStatusUpdate(" + connectionStatusUpdate.ToString() + ")");

            connectionStatusOrder = connectionStatusUpdate.Status;
            connectionStatusPrice = connectionStatusUpdate.PriceStatus;
            base.OnConnectionStatusUpdate(connectionStatusUpdate);

            if (ATSAlgoSystemState == AlgoSystemState.HisTradeRT && Position.MarketPosition == MarketPosition.Flat)
            {
                ATSAlgoSystemState = AlgoSystemState.Realtime;
            }


        }

        protected override void OnAccountItemUpdate(Account account, AccountItem accountItem, double value)
        {
            accountDenomination = account.Denomination;
        }


        public override void OnCalculateMinMax()
        {

            //trap erroneous bugs from base class
            try
            {
                base.OnCalculateMinMax();

            }
            catch (Exception ex)
            {
                Print("OnCalculateMinMax > " + ex.ToString());
            }

        }


        protected override void OnBarUpdate()
        {
            //historical playback not supported,  test for realtimeOnly trading
            if ((State == State.Historical && (IsRealtimeTradingOnly || IsPlayBack)) || CurrentBar < 1)
                return;

            try
            {
                lastPrice = Closes[0][0];

                if (IsFirstTickOfBar)
                {
                    if (Bars.IsFirstBarOfSessionByIndex(0))
                    {
                        if (IsExitOnSessionCloseStrategy)
                        {
                            OnExitOnCloseDetected();
                        }
                    }
                }


                if (AlgoSignalAction == AlgoSignalAction.None)
                    return;


                //if execution context reaches here process signal
                //historical mode
                if (State == State.Historical)
                {
                    //Assumes all is perfect and forces action regardless of workflow state
                    switch (AlgoSignalAction)
                    {
                        case AlgoSignalAction.GoLong:
                            TradeWorkFlowNewOrder(StrategyTradeWorkFlowState.GoLong);
                            break;
                        case AlgoSignalAction.GoShort:
                            TradeWorkFlowNewOrder(StrategyTradeWorkFlowState.GoShort);
                            break;
                        case AlgoSignalAction.ExitTrade:
                            TradeWorkFlowTradeExit();
                            break;
                        case AlgoSignalAction.ExitTradeLong:
                            TradeWorkFlowTradeExitLong();
                            break;
                        case AlgoSignalAction.ExitTradeShort:
                            TradeWorkFlowTradeExitShort();
                            break;
                        case AlgoSignalAction.GoOCOEntry:
                            TradeWorkFlowTradeEntryOCO();
                            break;


                    }
                    //Reset to avoid duplicate action
                    AlgoSignalAction = AlgoSignalAction.None;
                    return;
                }
                //Realtime process with no signal queue
                else if (!IsRealtimeTradingUseQueue)
                {
                    bool isActionProcessed = false;
                    //realtime mode assumes all is not perfect and will all the queue of any AlgoSignalAction if not validated
                    switch (AlgoSignalAction)
                    {
                        case AlgoSignalAction.GoLong:
                            if (this.IsTradeWorkFlowCanGoLong())
                            {
                                isActionProcessed = true;
                                TradeWorkFlowNewOrder(StrategyTradeWorkFlowState.GoLong);
                            }
                            break;
                        case AlgoSignalAction.GoShort:
                            if (this.IsTradeWorkFlowCanGoShort())
                            {
                                isActionProcessed = true;
                                TradeWorkFlowNewOrder(StrategyTradeWorkFlowState.GoShort);
                            }
                            break;
                        case AlgoSignalAction.ExitTrade:
                            if (this.IsTradeWorkFlowCanExit())
                            {
                                isActionProcessed = true;
                                TradeWorkFlowTradeExit();
                            }
                            break;
                        case AlgoSignalAction.ExitTradeLong:
                            if (this.IsTradeWorkFlowCanExit())
                            {
                                isActionProcessed = true;
                                TradeWorkFlowTradeExitLong();
                            }
                            break;
                        case AlgoSignalAction.ExitTradeShort:
                            if (this.IsTradeWorkFlowCanExit())
                            {
                                isActionProcessed = true;
                                TradeWorkFlowTradeExitShort();
                            }
                            break;
                        case AlgoSignalAction.GoOCOEntry:
                            if (this.IsTradeWorkFlowCanEntryOCO())
                            {
                                isActionProcessed = true;
                                TradeWorkFlowTradeEntryOCO();
                            }
                            break;
                    }
                    if (isActionProcessed)
                    {
                        //Reset to avoid duplicate action
                        AlgoSignalAction = AlgoSignalAction.None;
                        return;
                    }
                    //if here then no action take discard or store signal in q
                    if (IsUseSignalQFallbackForSignals)
                    {
                        lock (TEQ)
                        {
                            //if execution context reaches here use q IsRealtimeTradingUseQueue or !isActionProcessed, add signAction to q
                            TEQ.Enqueue(new AlgoSignalActionMsq(AlgoSignalAction, Account.Connection.Now, "Auto Signal " + AlgoSignalAction));
                        }
                        //Reset to avoid duplicate action
                        AlgoSignalAction = AlgoSignalAction.None;

                        TEQOnMarketDataEnable();
                        return;
                    }
                }
                lock (TEQ)
                {
                    //if execution context reaches here use q IsRealtimeTradingUseQueue or !isActionProcessed, add signAction to q
                    TEQ.Enqueue(new AlgoSignalActionMsq(AlgoSignalAction, Account.Connection.Now, "Auto Signal " + AlgoSignalAction));
                }

                //Reset to avoid duplicate action
                AlgoSignalAction = AlgoSignalAction.None;

                // try q straigh away
                ProcessTradeEventQueue();
            }
            catch (Exception ex)
            {
                Print("OnBarUpdate > " + ex.ToString());
            }
        }







        protected override void OnOrderUpdate(Order order, double limitPrice, double stopPrice, int quantity, int filled, double averageFillPrice, OrderState orderState, DateTime time, ErrorCode error, string comment)
        {
            if (order == null) return;

            if (tracing)
                Print("OnOrderUpdate(" + order.Name + " OrderId=" + order.OrderId + " State=" + order.OrderState.ToString() + ")");


            if (IsHistorical)
            {
                if (IsShowOrderLabels && order.OrderState == OrderState.Submitted)
                    Draw.Text(this, order.Name, order.Name, 0, order.AverageFillPrice != 0 ? order.AverageFillPrice : order.StopPrice != 0 ? order.StopPrice : order.LimitPrice != 0 ? order.LimitPrice : GetCurrentAsk());
                return;
            }

            try
            {

                //add this bit to take care of caveats and problems over submitorder returning slowly missing the order ref or the order ref changing due to realtime transition
                if (order.OrderState == OrderState.Submitted || order.OrderState == OrderState.Accepted || order.OrderState == OrderState.Working)
                {

                    if ((order.Name == orderEntryName) || order.Name.Contains(entry1NameLong) || order.Name.Contains(entry1NameShort))
                    {
                        orderEntry = order;

                    }
                    else if (order.Name.Contains(closeOrderName))
                    {
                        orderClose = order;
                    }
                    else if (order.OrderType == OrderType.StopMarket)
                    {
                        if (order.Name.Contains(stop1Name)) { orderStop1 = order; }
                        else if (order.Name.Contains(stop2Name)) { orderStop2 = order; }
                        else if (order.Name.Contains(stop3Name)) { orderStop3 = order; }
                        else if (order.Name.Contains(stop4Name)) { orderStop4 = order; }
                        else if (order.Name.Contains(orderEntryOCOLongName)) { orderEntryOCOLong = order; }
                        else if (order.Name.Contains(orderEntryOCOShortName)) { orderEntryOCOShort = order; }
                    }
                    else if (order.OrderType == OrderType.Limit)
                    {
                        if (order.Name.Contains(target1Name)) { orderTarget1 = order; }
                        else if (order.Name.Contains(target2Name)) { orderTarget2 = order; }
                        else if (order.Name.Contains(target3Name)) { orderTarget3 = order; }
                        else if (order.Name.Contains(target4Name)) { orderTarget4 = order; }
                    }
                }

                if (IsPlayBack) return;


                #region order tracking for entry, stops and targets

                #region OrdersRT

                //not interested in market orders
                if (IsRealtime && order.OrderType != OrderType.Market)
                {
                    lock (ordersActiveLockObject)
                    {
                        if (order.OrderState == OrderState.Submitted || order.OrderState == OrderState.Accepted)
                        {
                            if (!OrdersActive.Contains(order))
                                OrdersActive.Add(order);
                        }
                        else if (!IsOrderIsActive(order))
                        {
                            OrdersActive.Remove(order);
                        }
                    }  // End of Lock

                }

                #endregion

                #endregion

                #region order state process
                switch (order.OrderState)
                {
                    case OrderState.Accepted:
                        goto case OrderState.Working;
                    case OrderState.Cancelled:
                        if (order.HasOverfill) OnOrderOverFillDetected(order);
                        if(TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoShortSubmitOrderWorking)
                        {
                            if (IsOrdersAnyActiveExist()) return;
                            TradeWorkFlow = StrategyTradeWorkFlowState.Waiting;
                            return;
                        }
                        break;
                    case OrderState.Filled:
                        if (order.HasOverfill) OnOrderOverFillDetected(order);

                        break;
                    case OrderState.Initialized:
                        break;
                    case OrderState.PartFilled:
                        if (order.HasOverfill) OnOrderOverFillDetected(order);
                        break;
                    case OrderState.CancelPending:
                        if (order.HasOverfill) OnOrderOverFillDetected(order);
                        break;
                    case OrderState.ChangePending:
                        if (order.HasOverfill) OnOrderOverFillDetected(order);
                        break;
                    case OrderState.Submitted:


                        if (order == orderEntry)
                        {
                            if (TradeWorkFlow == StrategyTradeWorkFlowState.GoLongSubmitOrder)
                            {
                                TradeWorkFlow = StrategyTradeWorkFlowState.GoLongSubmitOrderPending;
                            }
                            else if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortSubmitOrder)
                            {
                                TradeWorkFlow = StrategyTradeWorkFlowState.GoShortSubmitOrderPending;
                            }

                        }
                        else if (order == orderClose)
                        {
                            if (tradeWorkFlow == StrategyTradeWorkFlowState.GoLongClosePositions) TradeWorkFlow = StrategyTradeWorkFlowState.GoLongClosedPositionsPending;
                            else if (tradeWorkFlow == StrategyTradeWorkFlowState.GoShortClosePositions) TradeWorkFlow = StrategyTradeWorkFlowState.GoShortClosedPositionsPending;
                            else if (tradeWorkFlow == StrategyTradeWorkFlowState.ExitTradeClosePositions) TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeClosePositionsPending;
                        }
                        else if (IsExitOnSessionCloseStrategy && order.Name.ToLower().Contains("exit on close"))
                        {
                            orderExitOnClose = order;
                            TradeWorkFlow = StrategyTradeWorkFlowState.ExitOnCloseOrderPending;
                        }

                        break;
                    case OrderState.Rejected:
                        if (tracing)
                        {
                            Print("\r\n");
                            Print("OnOrderUpdate > Rejected(" + order.ToString() + ")");
                            Print(order.ToString());
                        }

                        bool raiseAsError = false;
                        raiseAsError = IsRaiseErrorOnAllOrderRejects;

                        if (!raiseAsError)
                        {
                            if (order == orderEntry)
                            {
                                raiseAsError = true;
                            }
                            else
                                lock (OrdersStopLoss)
                                    if (OrdersStopLoss.Contains(order))
                                    {
                                        raiseAsError = true;
                                    }
                                    else
                                        lock (OrdersProfitTarget)
                                            if (OrdersProfitTarget.Contains(order))
                                            {
                                                raiseAsError = true;
                                            }
                        }
                        if (tracing) Print("OnOrderUpdate > Raise Error " + raiseAsError.ToString());

                        //set to error state and process in deffered execution to allow OnOrderUpdate to return etc
                        if (raiseAsError)
                            TradeWorkFlowErrorProcess(false);

                        break;
                    case OrderState.Unknown:
                        if (tracing)
                            Print("OnOrderUpdate > Unknown(" + order.ToString() + ")");
                        TradeWorkFlowErrorProcess(false);
                        break;
                    case OrderState.Working:

                        if (order == orderEntry)
                        {
                            if (order.OrderAction < OrderAction.Sell)
                                TradeWorkFlow = StrategyTradeWorkFlowState.GoLongSubmitOrderWorking;
                            else
                                TradeWorkFlow = StrategyTradeWorkFlowState.GoShortSubmitOrderWorking;
                        }
                        else if (order == orderEntryOCOLong || order == orderEntryOCOLong)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking;
                        }
                        else
                        {
                            // locking - the whole if() structure and call to ProcessWorkFlow() is too large to lock in whole, is deadlock risky so created a bool for the if(..)
                            bool bStopLossOrdersContainsOrder = false;
                            lock (OrdersStopLoss)
                                if (OrdersStopLoss.Contains(order)) { bStopLossOrdersContainsOrder = true; }

                            if (bStopLossOrdersContainsOrder)
                            {
                                if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortPlaceStopsPending || TradeWorkFlow == StrategyTradeWorkFlowState.GoLongPlaceStopsPending)
                                {
                                    //put a lock here as this region was causing errors
                                    // locking - the whole if() structure and call to ProcessWorkFlow() is too large to lock in whole, is deadlock risky so created a bool for the if(..)
                                    bool result = false;
                                    lock (OrdersStopLoss)
                                        result = IsOrdersAllActiveOrWorkingOrFilled(OrdersStopLoss) && OrdersStopLoss.Sum(o => o.Quantity) == orderEntry.Quantity;

                                    if (result)
                                    {
                                        if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortPlaceStopsPending) TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceStopsConfirmed;
                                        else if (TradeWorkFlow == StrategyTradeWorkFlowState.GoLongPlaceStopsPending) TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceStopsConfirmed;
                                        ProcessWorkFlow();
                                    }
                                }
                            }

                            else
                            {
                                // locking - the whole if() structure and call to ProcessWorkFlow() is too large to lock in whole, is deadlock risky so created a bool for the if(..)
                                bool bProfitTargetOrdersContainsOrder = false;
                                lock (OrdersProfitTarget)
                                    if (OrdersProfitTarget.Contains(order)) { bProfitTargetOrdersContainsOrder = true; }


                                if (bProfitTargetOrdersContainsOrder)
                                {
                                    if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsPending || TradeWorkFlow == StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsPending)
                                    {
                                        //put a lock here as this region was causing errors
                                        // locking the whole if() structure and call to ProcessWorkFlow() is too large to lock in whole, is deadlock risky so created a bool for the if(..)
                                        bool result = false;
                                        lock (OrdersProfitTarget)
                                            result = IsOrdersAllActiveOrWorkingOrFilled(OrdersProfitTarget) && OrdersProfitTarget.Sum(o => o.Quantity) == orderEntry.Quantity;

                                        if (result)
                                        {
                                            if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsPending) TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsConfirmed;
                                            else if (TradeWorkFlow == StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsPending) TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsConfirmed;
                                            ProcessWorkFlow();
                                        }
                                    }
                                }
                            }
                        }


                        break;
                }


                //to do replace with the onmarketdata way
                #region confirm orders are cancelled
                if (!IsOrderIsActive(order) && (TradeWorkFlow == StrategyTradeWorkFlowState.GoLongCancelWorkingOrdersPending
                    || TradeWorkFlow == StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersPending
                    || TradeWorkFlow == StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderPending
                    || TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersPending
                    ) && order != orderClose)
                {

                    //check all stops and targets and working orders have gone through workflow and are all cancelled or inactive
                    if (!IsOrdersAnyActiveExist())
                    {
                        if (TradeWorkFlow == StrategyTradeWorkFlowState.GoLongCancelWorkingOrdersPending)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoLongCancelWorkingOrdersConfirmed;
                        }
                        else if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersPending)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersConfirmed;
                        }
                        else if (TradeWorkFlow == StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderPending)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderConfirmed;
                        }
                        else if (TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersPending)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersConfirmed;
                        }
                        ProcessWorkFlow();
                    }
                }


                #endregion



                #endregion
                //}
            }
            catch (Exception ex)
            {
                Print("OnOrderUpdate >> Error: " + ex.ToString());
                Debug.Print("OnOrderUpdate >> Error: " + ex.ToString());
                Log("OnOrderUpdate >> Error: " + ex.ToString(), LogLevel.Error);

            }

        }

        protected override void OnExecutionUpdate(Execution execution, string executionId, double price, int quantity, MarketPosition marketPosition, string orderId, DateTime time)
        {
            if (tracing)
                Print("OnExecution(" + execution.ToString() + ")");
            try
            {
                if (execution.Order.HasOverfill)
                {
                    OnOrderOverFillDetected(execution.Order);
                    return;
                }


                if (ATSAlgoSystemState == AlgoSystemState.HisTradeRT)
                {
                    if (TradeWorkFlow == StrategyTradeWorkFlowState.ExitOnTransitionWaitingConfirmation)
                        ProcessWorkFlow();

                    if (!execution.Order.IsBacktestOrder || Position.MarketPosition == MarketPosition.Flat)
                        ATSAlgoSystemState = AlgoSystemState.Realtime;
                }


                if (execution.Order.OrderState != OrderState.Filled) return;

                if (execution.Order == orderEntryOCOLong || execution.Order == orderEntryOCOShort)
                {
                    orderEntry = execution.Order;
                }

                if (execution.Order == orderEntry)
                {

                    //filled
                    if (orderEntry.OrderAction < OrderAction.Sell)
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongSubmitOrderFilled;
                    else
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortSubmitOrderFilled;

                    ProcessWorkFlow();
                    return;
                }
                else if (execution.Order == orderClose)
                {

                    if (tradeWorkFlow == StrategyTradeWorkFlowState.GoLongClosedPositionsPending) TradeWorkFlow = StrategyTradeWorkFlowState.GoLongClosedPositionsConfirmed;
                    else if (tradeWorkFlow == StrategyTradeWorkFlowState.GoShortClosedPositionsPending) TradeWorkFlow = StrategyTradeWorkFlowState.GoShortClosedPositionsConfirmed;
                    else if (tradeWorkFlow == StrategyTradeWorkFlowState.ExitTradeClosePositionsPending) TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeClosePositionsConfirmed;
                    else if (tradeWorkFlow == StrategyTradeWorkFlowState.ErrorFlattenAllPending) TradeWorkFlow = StrategyTradeWorkFlowState.ErrorFlattenAllConfirmed;
                    ProcessWorkFlow(TradeWorkFlow);
                    return;
                }
                else if (execution.Order == orderExitOnClose || IsExitOnSessionCloseStrategy && execution.Order.Name.ToLower().Contains("exit on close"))
                {
                    orderExitOnClose = execution.Order;
                    TradeWorkFlow = StrategyTradeWorkFlowState.ExitOnCloseOrderFilled;
                    ProcessWorkFlow();
                    return;
                }
                else if (execution.Order == orderEntryPrior)
                {
                    //if priorEntry executed this means a cancel failed and was missed by the time the new order submitted - something went wrong....  best to go to error mode
                    if (tracing)
                        Print("OnExecutionUpdate > ERROR! Prior order Executed (" + execution.ToString() + ")");

                    //process error
                    TradeWorkFlowErrorProcess(false);

                }
            }
            catch (Exception ex)
            {
                Print("OnExecutionUpdate >> Error: " + ex.ToString());
                Debug.Print("OnExecutionUpdate >> Error: " + ex.ToString());
                Log("OnExecutionUpdate >> Error: " + ex.ToString(), LogLevel.Error);
            }

        }



        protected override void OnPositionUpdate(Position position, double averagePrice, int quantity, MarketPosition marketPosition)
        {
            if (tracing)
                Print("OnPositionUpdate > " + marketPosition.ToString());

            if (IsHistorical) return;


            if (ATSAlgoSystemState == AlgoSystemState.HisTradeRT && marketPosition == MarketPosition.Flat)
            {
                ATSAlgoSystemState = AlgoSystemState.Realtime;
            }

        }

        protected override void OnMarketData(MarketDataEventArgs marketDataUpdate)
        {

            if (marketDataUpdate.MarketDataType == MarketDataType.Bid || marketDataUpdate.MarketDataType == MarketDataType.Ask)
            {
                if (DateTime.Now < onMarketDataBidTimeNextAllowed) return;
                onMarketDataBidTimeNextAllowed = DateTime.Now.AddMilliseconds(250);
                AskPrice = marketDataUpdate.Ask;
                BidPrice = marketDataUpdate.Bid;
                return;
            }

            if (marketDataUpdate.MarketDataType != MarketDataType.Last) return;
            this.MarketDataUpdate = marketDataUpdate;
            if (this.LastPrice == marketDataUpdate.Price) return;
            LastPrice = marketDataUpdate.Price;


            if (DateTime.Now < onMarketDataTimeNextAllowed) return;
            onMarketDataTimeNextAllowed = DateTime.Now.AddSeconds(1);

            if (Position.MarketPosition == MarketPosition.Flat)
            {
                PositionInfo = string.Format("{0}", Position.MarketPosition.ToString());
                PositionState = 0;
                UnRealizedPL = 0;
            }
            else
            {
                PositionInfo = string.Format("{0} {1} @ {2}", Position.MarketPosition.ToString().Substring(0, 1), Position.Quantity, Position.AveragePrice);

                if (Position.MarketPosition == MarketPosition.Long)
                    PositionState = 1;
                else
                    PositionState = -1;

                UnRealizedPL = Position.GetUnrealizedProfitLoss(PerformanceUnit.Currency);
            }

            if (IsHistorical || TradeWorkFlow == StrategyTradeWorkFlowState.ExitOnTransitionWaitingConfirmation)
                return;

            //lock re-etnry here during processing
            if (this.inOnMarketData) return;
            lock (this.inOnMarketDataLock)
            {
                if (this.inOnMarketData) return;
                this.inOnMarketData = true;
            }


            try
            {



                //unjam StrategyTradeWorkFlowState in case its stuck
                if ((!IsTradeWorkFlowReady() || TradeWorkFlow == StrategyTradeWorkFlowState.Error) && TradeWorkFlowLastChanged < DateTime.Now.AddSeconds(-1 * TradeWorkFlowTimeOut))
                {
                    if (tracing)
                        Print("OnMarketData >> TWF ErrorTimeOut");

                    TradeWorkFlow = StrategyTradeWorkFlowState.ErrorTimeOut;
                    ProcessWorkFlow(StrategyTradeWorkFlowState.ErrorTimeOut);
                    this.inOnMarketData = false;
                    return;
                }


                //process the trade workflow state engine - move state on in case its holding up signals execution
                if (IsTradeWorkFlowOnMarketData && Now >= tradeWorkFlowNextTimeValid)
                {
                    tradeWorkFlowNextTimeValid = Now.AddMilliseconds(TradeWorkFlowTimerInterval);
                    if (tracing)
                        Print("OnMarketData >> TWF");
                    ProcessWorkFlow();
                }

                //process the signal q
                if (IsTEQOnMarketData && Now >= tEQNextTimeValid)
                {
                    tEQNextTimeValid = Now.AddSeconds(TEQTimerInterval);
                    if (tracing)
                        Print("OnMarketData >> TEQ");


                    bool isTEQPopulated = false;
                    lock (TEQ)
                        isTEQPopulated = TEQ.Count > 0;

                    if (isTEQPopulated)
                        ProcessTradeEventQueue();
                    else if (!IsRealtimeTradingUseQueue) TEQOnMarketDataDisable();



                }


            }
            catch (Exception ex)
            {
                Print("OnMarketData >> Error: " + ex.ToString());
                Debug.Print("OnMarketData >> Error: " + ex.ToString());
                Log("OnMarketData >> Error: " + ex.ToString(), LogLevel.Error);
            }
            this.inOnMarketData = false;


        }



        /// <summary>
        /// OnStrategyTradeWorkFlowUpdated
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnStrategyTradeWorkFlowUpdated(StrategyTradeWorkFlowUpdatedEventArgs e)
        {
            EventHandler<StrategyTradeWorkFlowUpdatedEventArgs> handler = StrategyTradeWorkFlowUpdated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public virtual void OnATSAlgoSystemStateUpdated(ATSAlgoSystemStateUpdatedEventArgs e)
        {

            if (tracing)
                Print("OnATSAlgoSystemStateUpdated > " + e.ATSAlgoSystemState.ToString());


            EventHandler<ATSAlgoSystemStateUpdatedEventArgs> handler = ATSAlgoSystemStateUpdated;
            if (handler != null)
            {
                handler(this, e);
            }


        }



        #endregion
        #region methods
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
        #region timers

        private void TradeWorkFlowOnMarketDataEnable(int mSeconds)
        {
            if (IsTradeWorkFlowOnMarketData) return;

            if (tracing)
                Print("TradeWorkFlowOnMarketDataEnable");

            tradeWorkFlowNextTimeValid = Now.AddMilliseconds(mSeconds);
            IsTradeWorkFlowOnMarketData = true;

        }


        private void TradeWorkFlowOnMarketDataEnable()
        {
            TradeWorkFlowOnMarketDataEnable(TradeWorkFlowTimerInterval);
        }

        private void TradeWorkFlowOnMarketDataDisable()
        {
            if (!IsTradeWorkFlowOnMarketData) return;

            IsTradeWorkFlowOnMarketData = false;
            if (tracing)
                Print("TradeWorkFlowOnMarketDataDisable");

        }


        private void TEQOnMarketDataEnable()
        {
            if (IsTEQOnMarketData) return;

            tEQNextTimeValid = Now.AddSeconds(TEQTimerInterval);
            IsTEQOnMarketData = true;
            if (tracing)
                Print("TEQOnMarketDataEnable");

        }

        private void TEQOnMarketDataDisable()
        {
            if (!IsTEQOnMarketData) return;

            IsTEQOnMarketData = false;
            if (tracing)
                Print("TEQOnMarketDataDisable");

        }

        public bool CurrentBarIsLastBar()
        {
            bool result = false;
            result = base.CurrentBar > (base.Bars.Count - 2);

#if DEBUG

            if (result) Print(base.CurrentBar);
#endif

            return result;
        }


        #endregion
        #region orderHelpers

        /// <summary>
        /// PreTradeValidateNoActiveOrdersExist for working or active orders and returns true if none are found - all clear is true - returns false if orders are found
        /// </summary>
        /// <returns></returns>
        public virtual bool PreTradeValidateNoActiveOrdersExist()
        {
            if (tracing)
                Print("PreTradeValidateNoActiveOrdersExist()");

            return !IsOrdersAnyActiveExist();
        }

        /// <summary>
        /// PreTradeValidatePositionIsFlat checks and returns true if Position.Quantity==0 - returns false if not
        /// </summary>
        /// <returns></returns>
        public virtual bool PreTradeValidatePositionIsFlat()
        {
            if (tracing)
                Print("PreTradeValidatePositionIsFlat()");

            return Position.Quantity == 0;
        }



        /// <summary>
        /// preTradeValidateCanEnterTrade - returns true if PreTradeValidateNoActiveOrdersExist and PreTradeValidatePositionIsFlat are true - returns false if not.
        /// Can be overidden in derived child class so that a user/developer defined set of conditions can be assessed to allow or prevent a trade entry.
        /// </summary>
        /// <param name="isLong"></param>
        /// <returns></returns>
        private bool PreTradeValidateCanEnterTrade(bool isLong)
        {
            if (tracing)
                Print("preTradeValidateCanEnterTrade()");

            return PreTradeValidateNoActiveOrdersExist() && (PreTradeValidatePositionIsFlat() || IsOrderInFlightOrActive(orderClose)) && OnPreTradeEntryValidate(isLong);
        }


        private bool PreTradeValidateCanEnterTradeOCO()
        {
            if (tracing)
                Print("PreTradeValidateCanEnterTradeOCO()");

            return PreTradeValidateNoActiveOrdersExist() && (PreTradeValidatePositionIsFlat() || IsOrderInFlightOrActive(orderClose));
        }


        public virtual bool OnPreTradeEntryValidate(bool isLong)
        {
            if (tracing)
                Print("OnpreTradeValidateCanEnterTrade()");
            return true;

        }




        /// <summary>
        /// OnExitOnCloseDetected() called when exit on close order has been detected from NT
        /// </summary>
        public virtual void OnExitOnCloseDetected()
        {
            if (tracing)
                Print("OnExitOnCloseDetected()");

            if (TradeWorkFlow != StrategyTradeWorkFlowState.Waiting)
            {
                ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTrade);
            }
        }


        public virtual void OnOrderOverFillDetected(Order order)
        {
            if (tracing)
                Print("OnOrderOverFillDetected(" + order.ToString() + ")");

            TradeWorkFlowErrorProcess(false);

        }


        public void CancelAllOrders(bool oCOLetCancel = false)
        {

            if (tracing)
                Print("CancelAllOrders()");

            try
            {
                if (IsHistoricalTradeOrPlayBack)
                {
                    if (IsOrderCancelInspectEachOrDoBatchCancel)
                    {

                        if (IsOrderActiveCanCancel(orderStop1)) CancelOrder(orderStop1);
                        if (IsOrderActiveCanCancel(orderStop2)) CancelOrder(orderStop2);
                        if (IsOrderActiveCanCancel(orderStop3)) CancelOrder(orderStop3);
                        if (IsOrderActiveCanCancel(orderStop4)) CancelOrder(orderStop4);

                        //these dont need to cancel if order stop cancel called - but just in case we have the option to put in a mode to do so
                        if (oCOLetCancel)
                        {
                            if (IsOrderActiveCanCancel(orderTarget1)) CancelOrder(orderTarget1);
                            if (IsOrderActiveCanCancel(orderTarget2)) CancelOrder(orderTarget2);
                            if (IsOrderActiveCanCancel(orderTarget3)) CancelOrder(orderTarget3);
                            if (IsOrderActiveCanCancel(orderTarget4)) CancelOrder(orderTarget4);
                        }

                        if (IsOrderActiveCanCancel(orderEntry)) CancelOrder(orderEntry);
                        if (IsOrderActiveCanCancel(orderEntryOCOLong)) CancelOrder(orderEntryOCOLong);
                        if (IsOrderActiveCanCancel(orderEntryOCOShort)) CancelOrder(orderEntryOCOShort);


                    }
                    else  //batch
                    {
                        lock (ordersActiveLockObject)
                        {
                            if (IsOrdersActiveExist(OrdersActive))
                            {
                                Account.Cancel(OrdersActive);
                                OrdersActive.Clear();
                            }
                        }
                    }
                }
                else //isRealtime
                {
                    Account.CancelAllOrders(this.Instrument);
                }

            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("CancelAllOrders {0}", ex.ToString());
                Print(errorMsg);
                Debug.Print(errorMsg);
                Log(errorMsg, LogLevel.Error);
            }
        }


        private bool IsOrdersAllActiveOrWorking(List<Order> orders)
        {
            //belt and braces code as errors logged were related to this method
            bool result = false;
            try
            {
                if (orders == null || orders.Count() == 0) return false;
                result = orders.Count(o => o != null && o.OrderState == OrderState.Accepted || o.OrderState == OrderState.Working) == orders.Count(o => o != null);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        private bool IsOrdersAllActiveOrWorkingOrFilled(List<Order> orders)
        {
            //belt and braces code as errors logged were related to this method
            bool result = false;
            try
            {
                if (orders == null || orders.Count() == 0) return false;
                return orders.Count(o => o != null && o.OrderState == OrderState.Accepted || o.OrderState == OrderState.Working || o.OrderState == OrderState.PartFilled || o.OrderState == OrderState.Filled) == orders.Count(o => o != null);
            }
            catch
            {
                result = false;
            }
            return result;
        }


        public bool IsOrdersCheckInactiveAndPurge()
        {
            lock (ordersActiveLockObject)
            {
                if (tracing)
                    Print("OrdersCheckInactiveAndPurge() > OrdersRT : " + OrdersActive.Count.ToString());

                foreach (Order o in OrdersActive.ToArray())
                {
                    if (!IsOrderIsActive(o))
                    {
                        OrdersActive.Remove(o);

                    }
                }
                return false;
            } // End of Lock
        }


        public bool IsOrdersActiveExist(List<Order> ordersActive)
        {
            bool bOrdersActiveExist = ordersActive.ToArray().Count(o => IsOrderIsActive(o)) > 0;
            if (tracing)
                Print("OrdersActiveExist() > OrdersRT : " + bOrdersActiveExist.ToString());

            return bOrdersActiveExist;

        }

        public bool IsOrdersProfitTargetActiveExist()
        {
            lock (OrdersStopLoss)
                return IsOrdersActiveExist(this.OrdersProfitTarget);
        }


        public bool IsOrdersStopLossActiveExist()
        {
            lock (OrdersStopLoss)
                return IsOrdersActiveExist(this.OrdersStopLoss);
        }


        public bool IsOrdersAnyActiveExist()
        {
            lock (OrdersActive)
                return IsOrdersActiveExist(this.OrdersActive);
        }

        public bool IsOrderAcceptedOrWorking(Order o)
        {
            return (o != null ? (o.OrderState == OrderState.Accepted || o.OrderState == OrderState.Working) : false);
        }

        public bool IsOrderIsActive(Order o)
        {
            return (o != null && !IsOrderTerminated(o));
        }

        public bool IsOrderInFlight(Order o)
        {
            return o.OrderState == OrderState.PartFilled;
        }

        private bool IsOrderInFlightOrActive(Order o)
        {
            return IsOrderIsActive(o) || IsOrderInFlight(o);
        }


        public bool IsOrderActiveCanChangeOrCancel(Order o)
        {
            return IsOrderIsActive(o) && o.OrderState != OrderState.CancelPending && o.OrderState != OrderState.ChangePending;
        }


        public bool IsOrderActiveCanCancel(Order o)
        {
            return IsOrderIsActive(o) && o.OrderState != OrderState.CancelPending;
        }


        public bool IsOrderTerminated(Order o)
        {
            return o != null && Order.IsTerminalState(o.OrderState) || ((State == State.Realtime ? o.OrderState == OrderState.Submitted && o.Time.AddSeconds(10) < DateTime.Now : false));
        }

        #endregion
        #region SignalActions
        public void SubmitSignalAction(AlgoSignalAction AlgoSignalActions, string signalLabel)
        {
            if (tracing) Print("SubmitSignalAction " + AlgoSignalActions.ToString() + " WF=" + this.TradeWorkFlow.ToString());

            TEQ.Enqueue(new AlgoSignalActionMsq(AlgoSignalActions, IsHistoricalTradeOrPlayBack ? Time[0] : DateTime.Now, signalLabel));
            #region Signal Execution
            if (IsHistoricalTradeOrPlayBack || !IsRealtimeTradingUseQueue)
            {
                if (TEQ.Count > 0) ProcessTradeEventQueue(this);
            }
            else
            {
                if (State == State.Realtime)
                {
                    TEQOnMarketDataEnable();
                }
            }
            #endregion
        }
        #endregion
        #region process queue

        public void ProcessTradeEventQueue()
        {
            ProcessTradeEventQueue(this.TEQ);
        }

        public void ProcessTradeEventQueue(object state)
        {
            ProcessTradeEventQueue();
        }

        //in the case of many signals coming in e.g. each tick
        public void ProcessTradeEventQueue(Queue<AlgoSignalActionMsq> q)
        {

            #region queueLock
            //we dont want to process again if execution context is already within here
            if (lockedQueue) return;
            lock (queueLockObject)
            {
                if (lockedQueue) return;
                lockedQueue = true;
            }
            #endregion
            //external locking required in case this is being set onBarUpdate or vias User
            lock (q)
            {
                if (q.Count == 0)
                {
                    lockedQueue = false;
                    return;
                }
                else if (!IsTradeWorkFlowReady())
                {
                    if (tracing)
                        Print("ProcessTradeEventQueue(Queue count " + q.Count.ToString() + " TradeWF " + tradeWorkFlow.ToString() + "  ) ");

                    if (State == State.Realtime)
                    {
                        TEQOnMarketDataEnable();
                    }
                    lockedQueue = false;
                    return;
                }
            }

            #region process Queue
            try
            {
                AlgoSignalActionMsq a = null;
                lock (q)
                {
                    while (q.Count > 1)
                    {
                        a = (AlgoSignalActionMsq)q.Dequeue();
                        if (tracing)
                            Print("ProcessTradeEventQueue > Dequeue " + a.ToString());
                    }
                    //Try process action
                    a = (AlgoSignalActionMsq)q.Peek();
                    if (tracing)
                        Print("ProcessTradeEventQueue> Peek> AlgoSignalActions " + a.ToString());

                    //expire signal if needed
                    if (DateTime.Now > a.ActionDateTime.AddSeconds(TradeSignalExpiryInterval))
                    {
                        if (tracing)
                            Print("ProcessTradeEventQueue> Trade Signal TimeOut > AlgoSignalActions " + a.ToString());

                        q.Dequeue();
                        a = null;
                        lockedQueue = false;
                        TEQOnMarketDataDisable();
                        return;
                    }

                    //if cannot process now do not dQ - unlock and return
                    switch (a.Action)
                    {
                        case AlgoSignalAction.GoLong:
                            if (!this.IsTradeWorkFlowCanGoLong())
                                goto default;
                            break;
                        case AlgoSignalAction.GoShort:
                            if (!this.IsTradeWorkFlowCanGoShort())
                                goto default;
                            break;
                        case AlgoSignalAction.ExitTrade:
                            if (!this.IsTradeWorkFlowCanExit())
                                goto default;
                            break;
                        case AlgoSignalAction.ExitTradeLong:
                            if (!this.IsTradeWorkFlowCanExit())
                                goto default;
                            break;
                        case AlgoSignalAction.ExitTradeShort:
                            if (!this.IsTradeWorkFlowCanExit())
                                goto default;
                            break;
                        default:
                            //if here then go around again
                            Print("ProcessTradeEventQueue> Retry Later> AlgoSignalActions " + a.ToString());
                            lockedQueue = false;
                            TEQOnMarketDataDisable();
                            return;
                    }
                    //if still here then dQ process to do
                    q.Dequeue();
                }

                Print("ProcessTradeEventQueue> Processed> AlgoSignalActions " + a.ToString());

                switch (a.Action)
                {
                    case AlgoSignalAction.GoLong:
                        TradeWorkFlowNewOrder(StrategyTradeWorkFlowState.GoLong);
                        break;
                    case AlgoSignalAction.GoShort:
                        TradeWorkFlowNewOrder(StrategyTradeWorkFlowState.GoShort);
                        break;
                    case AlgoSignalAction.ExitTrade:
                        TradeWorkFlowTradeExit();
                        break;
                    case AlgoSignalAction.ExitTradeLong:
                        TradeWorkFlowTradeExitLong();
                        break;
                    case AlgoSignalAction.ExitTradeShort:
                        TradeWorkFlowTradeExitShort();
                        break;
                }

            }
            catch (Exception ex)
            {
                Print(ex.ToString());
                Debug.Print(ex.ToString());
                Log(ex.ToString(), LogLevel.Error);
            }
            #endregion
            lockedQueue = false;
            TEQOnMarketDataDisable();
        }

        #endregion
        #region process workflow

        public void ProcessWorkFlow()
        {
            TradeWorkFlow = ProcessWorkFlow(TradeWorkFlow);
        }

        public void ProcessWorkFlow(object state)
        {
            ProcessWorkFlow(this.tradeWorkFlow);
        }

        public virtual StrategyTradeWorkFlowState ProcessWorkFlow(StrategyTradeWorkFlowState tradeWorkFlow)
        {
            if (tracing)
                Print("ProcessWorkFlow(" + tradeWorkFlow.ToString() + ")");
            //to do post fill check is correct prior to stoploss/targets


            TradeWorkFlow = tradeWorkFlow;

            switch (tradeWorkFlow)
            {
                case StrategyTradeWorkFlowState.GoLong:
                    //trade per direction test
                    if (Position.MarketPosition == MarketPosition.Long) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongValidationRejected);

                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongCancelWorkingOrders;
                        goto case StrategyTradeWorkFlowState.GoLongCancelWorkingOrders;
                    }
                    //orders to cancel test
                    if (IsOrdersAnyActiveExist()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongCancelWorkingOrders);

                    //position to close test
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongClosePositions);

                    //if midflight working orders wait ie exist in iOrders- have been terminated/ filled or part filled but onOrderUpdate has not yet been raised and processed on this thread retry on timer if realtime come back later and retry
                    //inflight order test   -but what about stuck pending orders
                    //prioWF= working && the priorEntryOrder==filled && still in OrdersRT - ie onOrderUpdate,exec, positionupdate not fired for it yet?

                    // created bool for the if(comparison) because the if() structure and calls are too big to lock in whole 
                    bool bOrdersActiveContainsOrderEntry = false;
                    lock (ordersActiveLockObject)
                        if (OrdersActive.Contains(orderEntry)) bOrdersActiveContainsOrderEntry = true;

                    if (State == State.Realtime && bOrdersActiveContainsOrderEntry && (tradeWorkFlowPrior == StrategyTradeWorkFlowState.GoShortSubmitOrderWorking || tradeWorkFlowPrior == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking) && (orderEntry.OrderState == OrderState.Filled))
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    //goto case StrategyTradeWorkFlowState.GoLongSubmitOrder;
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongSubmitOrder);
                case StrategyTradeWorkFlowState.GoLongCancelWorkingOrders:

                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        CancelAllOrders();
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongClosePositions;
                        goto case StrategyTradeWorkFlowState.GoLongClosePositions;

                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoLongCancelWorkingOrdersPending;

                            CancelAllOrders();
                            //OnOrderUpdate Event or WorkFlow Timer will re-enter Workflow
                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);

                    }

                    break;
                case StrategyTradeWorkFlowState.GoLongCancelWorkingOrdersPending:

                    if (IsOrdersAnyActiveExist())
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongCancelWorkingOrdersConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoLongCancelWorkingOrdersConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongClosePositions);
                    else return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongSubmitOrder);
                case StrategyTradeWorkFlowState.GoLongClosePositions:
                    if (Position.Quantity != 0)
                    {
                        if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoLongClosedPositionsPending;
                            PositionCloseInternal();
                            break;
                        }
                        else
                        {
                            if (connectionStatusOrder == ConnectionStatus.Connected)
                            {
                                //avoid a double exit multithreaded race condition
                                if (TradeWorkFlow == StrategyTradeWorkFlowState.GoLongClosedPositionsPending) break;
                                lock (lockObjectClose)
                                {
                                    if (TradeWorkFlow == StrategyTradeWorkFlowState.GoLongClosedPositionsPending) break;
                                    TradeWorkFlow = StrategyTradeWorkFlowState.GoLongClosedPositionsPending;
                                }
                                PositionCloseInternal();
                                break;
                            }
                            //if still here then connection issues must come back and try or raise alarm
                            TradeWorkFlowOnMarketDataEnable();
                            //will continue to loop back here forever unless we have a timeout
                            tradeWorkFlowRetryCount++;
                            if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                                return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);

                        }
                    }
                    else
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongSubmitOrder;
                        goto case StrategyTradeWorkFlowState.GoLongSubmitOrder;
                    }


                    break;
                case StrategyTradeWorkFlowState.GoLongClosedPositionsPending:
                    //execution event or realtime event or timer moves event on
                    if (Position.Quantity != 0)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongClosedPositionsConfirmed);
                    }

                    break;
                case StrategyTradeWorkFlowState.GoLongClosedPositionsConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongSubmitOrder);
                case StrategyTradeWorkFlowState.GoLongSubmitOrder:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        if (PreTradeValidateCanEnterTrade(true))
                        {
                            orderEntryPrior = orderEntry;
                            orderEntry = SubmitLongTrade();
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongSubmitOrderPending);
                        }
                        else
                        {
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongValidationRejected);
                        }
                        break;
                    }

                    if (connectionStatusOrder == ConnectionStatus.Connected)
                    {
                        if (PreTradeValidateCanEnterTrade(true))
                        {
                            orderEntryPrior = orderEntry;
                            orderEntry = SubmitLongTrade();
                        }
                        else
                        {
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongValidationRejected);
                        }
                    }
                    else
                    {
                        //flatten all and cancel the trade by the time it reconnects it might be too late.
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }

                    break;
                case StrategyTradeWorkFlowState.GoLongValidationRejected:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode) return ProcessWorkFlow(StrategyTradeWorkFlowState.Waiting);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                case StrategyTradeWorkFlowState.GoLongSubmitOrderPending:
                    if (orderEntry.OrderState == OrderState.Filled)
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongSubmitOrderFilled);
                    }
                    else if (orderEntry.OrderState == OrderState.Accepted || orderEntry.OrderState == OrderState.Working)
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongSubmitOrderWorking);
                    }

                    TradeWorkFlowOnMarketDataEnable();
                    //will continue to loop back here forever unless we have a timeout
                    tradeWorkFlowRetryCount++;
                    if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    break;
                case StrategyTradeWorkFlowState.GoLongSubmitOrderWorking:
                    TradeWorkFlowOnMarketDataDisable();
                    break;
                case StrategyTradeWorkFlowState.GoLongSubmitOrderFilled:
                    TradeWorkFlowOnMarketDataDisable();
                    if (SubmitStopLossWillOccur()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongPlaceStops);
                    if (SubmitProfitTargetWillOccur()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongPlaceProfitTargets);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.GoLongPlaceStops:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceStopsPending;
                        SubmitStopLossInternal();
                        goto case StrategyTradeWorkFlowState.GoLongPlaceStopsPending;
                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceStopsPending;
                            SubmitStopLossInternal();
                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);

                    }
                    break;
                case StrategyTradeWorkFlowState.GoLongPlaceStopsPending:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceStopsConfirmed;
                        goto case StrategyTradeWorkFlowState.GoLongPlaceStopsConfirmed;
                    }

                    // lock (StopLossOrders)  // Commened out 2020-11-30 by JC to allow for refactor to a lock with smaller scopeso less Deadlock risk and lower performance hit
                    //  {

                    // just seperated the creation of the bool from assignment for use so could lock ProfitTargetOrders before access attempt
                    bool allConfirmedGoLongPlaceStops = false;
                    lock (OrdersStopLoss)
                        allConfirmedGoLongPlaceStops = IsOrdersAllActiveOrWorkingOrFilled(OrdersStopLoss);

                    if (!allConfirmedGoLongPlaceStops)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongPlaceStopsConfirmed);
                    }
                    //                    }  // Commened out 2020-11-30 by JC to allow for refactor to a lock with smaller scope 
                    break;
                case StrategyTradeWorkFlowState.GoLongPlaceStopsConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    if (SubmitProfitTargetWillOccur()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongPlaceProfitTargets);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.GoLongPlaceProfitTargets:
                    if (IsHistoricalTradeOrPlayBack || !IsSubmitTargetsAndConfirm)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsPending;
                        SubmitProfitTargetInternal();
                        goto case StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsPending;
                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsPending;
                            SubmitProfitTargetInternal();
                            if (!IsSubmitTargetsAndConfirm)
                                goto case StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsPending;

                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsPending:
                    if (IsHistoricalTradeOrPlayBack || !IsSubmitTargetsAndConfirm)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsConfirmed;
                        goto case StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsConfirmed;
                    }

                    // just seperated the creation of the bool from assignment for use so could lock ProfitTargetOrders before access attempt
                    bool allConfirmedGoLongPlaceProfitTargets = false;
                    lock (OrdersProfitTarget)
                        allConfirmedGoLongPlaceProfitTargets = (IsOrdersAllActiveOrWorkingOrFilled(OrdersProfitTarget) || OrdersProfitTarget.Sum(o => o.Quantity) == orderEntry.Quantity);

                    if (!allConfirmedGoLongPlaceProfitTargets)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoLongPlaceProfitTargetsConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.GoShort:
                    //trade per direction test
                    if (Position.MarketPosition == MarketPosition.Short) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortValidationRejected);

                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortCancelWorkingOrders;
                        goto case StrategyTradeWorkFlowState.GoShortCancelWorkingOrders;
                    }


                    //orders to cancel test
                    //if (Historical || OrdersActiveExist() || Account.Name.ToLower() == Connection.ReplayAccountName) 
                    if (IsOrdersAnyActiveExist()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortCancelWorkingOrders);

                    //position to close test
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortClosePositions);

                    // created bool for the if(comparison) because the if() structure and calls are too big to lock in whole 
                    bOrdersActiveContainsOrderEntry = false;
                    lock (ordersActiveLockObject)
                        if (OrdersActive.Contains(orderEntry)) bOrdersActiveContainsOrderEntry = true;

                    //inflight entry order test
                    if (State == State.Realtime && bOrdersActiveContainsOrderEntry && (tradeWorkFlowPrior == StrategyTradeWorkFlowState.GoShortSubmitOrderWorking || tradeWorkFlowPrior == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking) && (orderEntry.OrderState == OrderState.Filled))
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortSubmitOrder);

                case StrategyTradeWorkFlowState.GoShortCancelWorkingOrders:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        CancelAllOrders();
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersConfirmed;
                        goto case StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersConfirmed;

                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersPending;

                            CancelAllOrders();
                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersPending:
                    if (IsOrdersAnyActiveExist())
                    {

                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortCancelWorkingOrdersConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortClosePositions);
                    else return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortSubmitOrder);
                case StrategyTradeWorkFlowState.GoShortClosePositions:
                    if (Position.Quantity != 0)
                    {
                        if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoShortClosedPositionsPending;
                            PositionCloseInternal();
                            break;
                        }
                        else
                        {
                            if (connectionStatusOrder == ConnectionStatus.Connected)
                            {
                                //avoid a double exit multithreaded race condition
                                if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortClosedPositionsPending) break;
                                lock (lockObjectClose)
                                {
                                    if (TradeWorkFlow == StrategyTradeWorkFlowState.GoShortClosedPositionsPending) break;
                                    TradeWorkFlow = StrategyTradeWorkFlowState.GoShortClosedPositionsPending;
                                }
                                PositionCloseInternal();

                                break;
                            }
                            //if still here then connection issues must come back and try or raise alarm
                            TradeWorkFlowOnMarketDataEnable();
                            //wait for event or timer confirmation
                            tradeWorkFlowRetryCount++;
                            if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                                return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                        }
                    }
                    else
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortSubmitOrder;
                        goto case StrategyTradeWorkFlowState.GoShortSubmitOrder;
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortClosedPositionsPending:
                    //execution event or realtime event or timer moves event on

                    if (Position.Quantity != 0)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortClosedPositionsConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortClosedPositionsConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortSubmitOrder);
                case StrategyTradeWorkFlowState.GoShortSubmitOrder:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        if (PreTradeValidateCanEnterTrade(false))
                        {
                            orderEntryPrior = orderEntry;
                            orderEntry = SubmitShortTrade();
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortSubmitOrderPending);
                        }
                        else
                        {
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortValidationRejected);
                        }
                        break;
                    }


                    if (connectionStatusOrder == ConnectionStatus.Connected)
                    {
                        if (PreTradeValidateCanEnterTrade(false))
                        {
                            orderEntryPrior = orderEntry;
                            orderEntry = SubmitShortTrade();
                        }
                        else
                        {
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortValidationRejected);
                        }
                    }
                    else
                    {
                        //flatten all and cancel the trade by the time it reconnects it might be too late.
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }

                    //TradeWorkFlow = StrategyTradeWorkFlowState.GoShortSubmitOrderPending;

                    break;
                case StrategyTradeWorkFlowState.GoShortValidationRejected:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode) return ProcessWorkFlow(StrategyTradeWorkFlowState.Waiting);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                case StrategyTradeWorkFlowState.GoShortSubmitOrderPending:

                    if (orderEntry.OrderState == OrderState.Filled)
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortSubmitOrderFilled);
                    }
                    else if (orderEntry.OrderState == OrderState.Accepted || orderEntry.OrderState == OrderState.Working)
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortSubmitOrderWorking);
                    }

                    TradeWorkFlowOnMarketDataEnable();
                    //will continue to loop back here forever unless we have a timeout
                    tradeWorkFlowRetryCount++;
                    if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);

                    break;
                case StrategyTradeWorkFlowState.GoShortSubmitOrderWorking:
                    break;
                case StrategyTradeWorkFlowState.GoShortSubmitOrderFilled:
                    TradeWorkFlowOnMarketDataDisable();
                    if (SubmitStopLossWillOccur()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortPlaceStops);
                    if (SubmitProfitTargetWillOccur()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortPlaceProfitTargets);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.GoShortPlaceStops:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceStopsPending;
                        SubmitStopLossInternal();
                        goto case StrategyTradeWorkFlowState.GoShortPlaceStopsPending;

                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceStopsPending;
                            SubmitStopLossInternal();
                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortPlaceStopsPending:

                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceStopsConfirmed;
                        goto case StrategyTradeWorkFlowState.GoShortPlaceStopsConfirmed;
                    }

                    // just seperated the creation of the bool from assignment for use so could lock StopLossOrders before access attempt
                    bool allConfirmedGoShortPlaceStops = false;
                    lock (OrdersStopLoss)
                        allConfirmedGoShortPlaceStops = IsOrdersAllActiveOrWorkingOrFilled(OrdersStopLoss);


                    if (!allConfirmedGoShortPlaceStops)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortPlaceStopsConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortPlaceStopsConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    if (SubmitProfitTargetWillOccur()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortPlaceProfitTargets);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.GoShortPlaceProfitTargets:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsPending;
                        SubmitProfitTargetInternal();
                        goto case StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsPending;

                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsPending;
                            SubmitProfitTargetInternal();
                            if (!IsSubmitTargetsAndConfirm)
                                goto case StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsPending;

                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsPending:
                    if (IsHistoricalTradeOrPlayBack || !IsSubmitTargetsAndConfirm)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsConfirmed;
                        goto case StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsConfirmed;
                    }

                    // just seperated the creation of the bool from assignment for use so could lock ProfitTargetOrders before access attempt
                    bool allConfirmedGoShortPlaceProfitTargets = false;
                    lock (OrdersProfitTarget)
                        allConfirmedGoShortPlaceProfitTargets = (IsOrdersAllActiveOrWorkingOrFilled(OrdersProfitTarget) || OrdersProfitTarget.Sum(o => o.Quantity) == orderEntry.Quantity);

                    if (!allConfirmedGoShortPlaceProfitTargets)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoShortPlaceProfitTargetsConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                //Begin GoOCO
                case StrategyTradeWorkFlowState.GoOCOLongShort:
                    //trade per direction test
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrders;
                        goto case StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrders;
                    }

                    //orders to cancel test
                    if (IsOrdersAnyActiveExist()) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrders);

                    //position to close test
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortClosePositions);

                    // created bool for the if(comparison) because the if() structure and calls are too big to lock in whole 
                    bOrdersActiveContainsOrderEntry = false;
                    lock (ordersActiveLockObject)
                        if (OrdersActive.Contains(orderEntry)) bOrdersActiveContainsOrderEntry = true;

                    //inflight entry order test
                    if (State == State.Realtime && bOrdersActiveContainsOrderEntry && (tradeWorkFlowPrior == StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking || tradeWorkFlowPrior == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking) && (orderEntry.OrderState == OrderState.Filled))
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrder);

                case StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrders:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        CancelAllOrders();
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersConfirmed;
                        goto case StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersConfirmed;

                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersPending;

                            CancelAllOrders();
                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersPending:
                    if (IsOrdersAnyActiveExist())
                    {

                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoOCOLongShortCancelWorkingOrdersConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortClosePositions);
                    else return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrder);
                case StrategyTradeWorkFlowState.GoOCOLongShortClosePositions:
                    if (Position.Quantity != 0)
                    {
                        if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortClosedPositionsPending;
                            PositionCloseInternal();
                            break;
                        }
                        else
                        {
                            if (connectionStatusOrder == ConnectionStatus.Connected)
                            {
                                //avoid a double exit multithreaded race condition
                                if (TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortClosedPositionsPending) break;
                                lock (lockObjectClose)
                                {
                                    if (TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortClosedPositionsPending) break;
                                    TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortClosedPositionsPending;
                                }
                                PositionCloseInternal();

                                break;
                            }
                            //if still here then connection issues must come back and try or raise alarm
                            TradeWorkFlowOnMarketDataEnable();
                            //wait for event or timer confirmation
                            tradeWorkFlowRetryCount++;
                            if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                                return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                        }
                    }
                    else
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrder;
                        goto case StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrder;
                    }
                    break;
                case StrategyTradeWorkFlowState.GoOCOLongShortClosedPositionsPending:
                    //execution event or realtime event or timer moves event on

                    if (Position.Quantity != 0)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortClosedPositionsConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.GoOCOLongShortClosedPositionsConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrder);
                case StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrder:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        if (PreTradeValidateCanEnterTradeOCO())
                        {
                            orderEntryPrior = orderEntry;
                            //orderEntry = SubmitShortTrade();
                            orderEntry = null;
                            SubmitOCOBreakoutInternal();
                        }
                        else
                        {
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortValidationRejected);
                        }
                        break;
                    }


                    if (connectionStatusOrder == ConnectionStatus.Connected)
                    {
                        if (PreTradeValidateCanEnterTradeOCO())
                        {
                            orderEntryPrior = orderEntry;
                            orderEntry = null;
                            SubmitOCOBreakoutInternal();
                        }
                        else
                        {
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortValidationRejected);
                        }
                    }
                    else
                    {
                        //flatten all and cancel the trade by the time it reconnects it might be too late.
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }



                    break;
                case StrategyTradeWorkFlowState.GoOCOLongShortValidationRejected:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode) return ProcessWorkFlow(StrategyTradeWorkFlowState.Waiting);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                case StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderPending:


                    if ((orderEntryOCOLong.OrderState == OrderState.Accepted || orderEntryOCOLong.OrderState == OrderState.Working)
                        && (orderEntryOCOShort.OrderState == OrderState.Accepted || orderEntryOCOShort.OrderState == OrderState.Working))
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking);
                    }

                    TradeWorkFlowOnMarketDataEnable();
                    //will continue to loop back here forever unless we have a timeout
                    tradeWorkFlowRetryCount++;
                    if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);

                    break;
                case StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking:

                    break;
                //End GoOCO
                case StrategyTradeWorkFlowState.ExitTradeLong:
                    if (Position.MarketPosition != MarketPosition.Long) return ProcessWorkFlow(StrategyTradeWorkFlowState.Waiting);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTrade);
                case StrategyTradeWorkFlowState.ExitTradeShort:
                    if (Position.MarketPosition != MarketPosition.Short) return ProcessWorkFlow(StrategyTradeWorkFlowState.Waiting);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTrade);
                case StrategyTradeWorkFlowState.ExitTrade:
                    //orders to cancel test

                    if (IsHistoricalTradeOrPlayBack || IsOrdersAnyActiveExist()) return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrders);
                    //position to close test
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeClosePositions);
                    //nothing to do
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrders:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {

                        CancelAllOrders();
                        //Assume all cancelled and move workflow onwards
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderConfirmed);
                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderPending;
                            //CancelAllTrackedOrders();
                            CancelAllOrders();

                        }
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    break;
                case StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderPending:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderConfirmed);

                    if (IsOrdersAnyActiveExist())
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.ExitTradeCancelWorkingOrderConfirmed:
                    TradeWorkFlowOnMarketDataDisable();
                    if (Position.Quantity != 0) return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeClosePositions);
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.ExitTradeClosePositions:
                    if (Position.Quantity != 0)
                    {
                        if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeClosePositionsPending;
                            PositionCloseInternal();
                            //event will return
                        }
                        else
                        {
                            if (connectionStatusOrder == ConnectionStatus.Connected)
                            {
                                TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeClosePositionsPending;
                                PositionCloseInternal();
                            }
                            TradeWorkFlowOnMarketDataEnable();
                            //wait for event or timer confirmation
                            tradeWorkFlowRetryCount++;
                            if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                                return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                        }
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeClosePositionsConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.ExitTradeClosePositionsPending:
                    //execution event or realtime event or timer moves event on
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode) break;
                    if (Position.Quantity != 0)
                    {
                        TradeWorkFlowOnMarketDataEnable();
                        //will continue to loop back here forever unless we have a timeout
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitTradeClosePositionsConfirmed);
                    }
                    break;
                case StrategyTradeWorkFlowState.ExitTradeClosePositionsConfirmed:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);

                    TradeWorkFlowOnMarketDataDisable();
                    if (Position.MarketPosition != MarketPosition.Flat || IsOrdersAnyActiveExist())
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);

                case StrategyTradeWorkFlowState.ExitOnTransition:
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitOnTransitionCancelWorkingOrders);
                case StrategyTradeWorkFlowState.ExitOnTransitionCancelWorkingOrders:
                    CancelAllOrders();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitOnTransitionClosePositions);
                case StrategyTradeWorkFlowState.ExitOnTransitionClosePositions:
                    PositionCloseInternal();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitOnTransitionWaitingConfirmation);
                case StrategyTradeWorkFlowState.ExitOnTransitionWaitingConfirmation:
                    //when bar closes the orders to exit will execute
                    if (Position.MarketPosition == MarketPosition.Flat)
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitOnTransitionComplete);
                    break;
                case StrategyTradeWorkFlowState.ExitOnTransitionComplete:
                    ATSAlgoSystemState = AlgoSystemState.Realtime;

                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.ErrorTimeOut:
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                case StrategyTradeWorkFlowState.Error:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        CancelAllOrders();
                        //Assume all cancelled and move workflow onwards
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ErrorFlattenAll);
                    }
                    else
                    {
                        if (tracing && IsTracingOpenFileOnError)
                            DebugTraceHelper.OpenTraceFile();

                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            CancelAllOrders();
                            TradeWorkFlow = StrategyTradeWorkFlowState.ErrorFlattenAll;

                            lock (ordersActiveLockObject)
                                OrdersActive.Clear();

                            lock (ordersStopLoss)
                                ordersStopLoss.Clear();

                            lock (ordersProfitTarget)
                                ordersProfitTarget.Clear();

                        }
                        TradeWorkFlowOnMarketDataEnable();
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                        {
                            Log("Unable to resolove error - unable to check and cancel orders - reseting alarm to try again", LogLevel.Error);
                            Log("Unable to resolve error - unable to check and cancel orders - reseting alarm to try again", LogLevel.Alert);
                            tradeWorkFlowRetryCount = 0;
                            //                            Position.Close();
                            //                            this.Disable();
                        }
                        //wait for event or timer
                    }

                    break;
                case StrategyTradeWorkFlowState.ErrorFlattenAll:
                    if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
                    {
                        TradeWorkFlow = StrategyTradeWorkFlowState.ErrorFlattenAllPending;
                        PositionCloseInternal();
                        //event will return
                    }
                    else
                    {
                        if (connectionStatusOrder == ConnectionStatus.Connected)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.ErrorFlattenAllPending;
                            PositionCloseInternal();
                            TradeWorkFlowOnMarketDataEnable();
                            break;
                        }

                        TradeWorkFlowOnMarketDataEnable();
                        //wait for event or timer confirmation
                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                        {
                            Log("Unable to check and close position - reseting alarm to try again", LogLevel.Error);
                            Log("Unable to check and close position - reseting alarm to try again", LogLevel.Alert);
                            tradeWorkFlowRetryCount = 0;
                        }

                    }
                    break;
                case StrategyTradeWorkFlowState.ErrorFlattenAllPending:
                    if (connectionStatusOrder == ConnectionStatus.Connected)
                    {
                        if (Position.MarketPosition == MarketPosition.Flat)
                        {
                            TradeWorkFlow = StrategyTradeWorkFlowState.ErrorFlattenAllConfirmed;
                            TradeWorkFlowOnMarketDataEnable();
                            break;
                        }

                    }
                    TradeWorkFlowOnMarketDataEnable();
                    //wait for event or timer confirmation
                    tradeWorkFlowRetryCount++;
                    if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                    {
                        Log("Unable verify ErrorFlattenAllPending - reseting alarm to try again", LogLevel.Error);
                        Log("Unable verify ErrorFlattenAllPending - reseting alarm to try again", LogLevel.Alert);
                        tradeWorkFlowRetryCount = 0;
                        Account.Flatten(new[] { this.Instrument });
                    }

                    break;
                case StrategyTradeWorkFlowState.ErrorFlattenAllConfirmed:
                    if (State == State.Realtime)
                    {
                        TradeWorkFlowOnMarketDataDisable();
                        if (IsOrdersAnyActiveExist() || Position.MarketPosition != MarketPosition.Flat)
                        {
                            Log("Unable to verify ErrorFlattenAllConfirmed", LogLevel.Error);
                            Log("Unable verify ErrorFlattenAllConfirmed - reseting state to error to try again", LogLevel.Alert);
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                        }
                    }
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                case StrategyTradeWorkFlowState.ExitOnCloseOrderPending:
                    if (State == State.Realtime) break;

                    TradeWorkFlow = StrategyTradeWorkFlowState.ExitOnCloseWaitingConfirmation;
                    TradeWorkFlowOnMarketDataEnable();

                    break;
                case StrategyTradeWorkFlowState.ExitOnCloseWaitingConfirmation:
                    if (State == State.Realtime) break;

                    if (IsOrdersAnyActiveExist() || Position.MarketPosition != MarketPosition.Flat)
                    {
                        TradeWorkFlowOnMarketDataEnable();

                        tradeWorkFlowRetryCount++;
                        if (tradeWorkFlowRetryCount > tradeWorkFlowRetryAlarm)
                            return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitOnCloseConfirmed);
                    break;
                case StrategyTradeWorkFlowState.ExitOnCloseOrderFilled:
                    TradeWorkFlowOnMarketDataDisable();
                    return ProcessWorkFlow(StrategyTradeWorkFlowState.ExitOnCloseConfirmed);
                case StrategyTradeWorkFlowState.ExitOnCloseConfirmed:
                    if (IsOrdersAnyActiveExist() || Position.MarketPosition != MarketPosition.Flat)
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.Error);
                    }
                    else
                    {
                        return ProcessWorkFlow(StrategyTradeWorkFlowState.CycleComplete);
                    }

                case StrategyTradeWorkFlowState.CycleComplete:
                    TradeWorkFlowOnMarketDataDisable();
                    TradeWorkFlow = StrategyTradeWorkFlowState.Waiting;
                    break;
                default:
                    TradeWorkFlowOnMarketDataDisable();
                    break;
            }

            return TradeWorkFlow;

        }

        public bool IsTradeWorkFlowReady()
        {
            return (TradeWorkFlow == StrategyTradeWorkFlowState.Waiting || TradeWorkFlow == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoShortSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking);
        }

        public bool IsTradeWorkFlowCanGoLong()
        {
            return (TradeWorkFlow == StrategyTradeWorkFlowState.Waiting || TradeWorkFlow == StrategyTradeWorkFlowState.GoShortSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking);
        }

        public bool IsTradeWorkFlowCanGoShort()
        {
            return (TradeWorkFlow == StrategyTradeWorkFlowState.Waiting || TradeWorkFlow == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking);
        }

        public bool IsTradeWorkFlowCanExit()
        {
            return (TradeWorkFlow == StrategyTradeWorkFlowState.Waiting || TradeWorkFlow == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoShortSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoOCOLongShortSubmitOrderWorking);
        }

        public bool IsTradeWorkFlowCanEntryOCO()
        {
            return (TradeWorkFlow == StrategyTradeWorkFlowState.Waiting || TradeWorkFlow == StrategyTradeWorkFlowState.GoLongSubmitOrderWorking || TradeWorkFlow == StrategyTradeWorkFlowState.GoShortSubmitOrderWorking);
        }


        /// <summary>
        /// TradeWorkFlowErrorProcess Set and start error workflow for immediate or deffered process /pseudo Async - allows caller to return
        /// </summary>
        /// <param name="forceImmediate"></param>
        public virtual void TradeWorkFlowErrorProcess(bool forceImmediate = false)
        {
            TradeWorkFlow = StrategyTradeWorkFlowState.Error;

            if (tracing)
                Print("TradeWorkFlowErrorProcess() > forceImmediate " + forceImmediate.ToString());

            if (IsHistoricalTradeOrPlayBack || forceImmediate)
                ProcessWorkFlow(TradeWorkFlow);

            //deferred execution
            if (ATSAlgoSystemState == AlgoSystemState.Realtime)
                TradeWorkFlowOnMarketDataEnable();

        }


        public virtual void TradeWorkFlowTradeEntryOCO()
        {
            if (tracing)
                Print("TradeWorkFlowTradeEntryOCO()");

            lock (tradeWorkFlowTradeEntryOCOLockObject)
            {
                if (IsTradeWorkFlowCanEntryOCO())
                {
                    TradeWorkFlow = StrategyTradeWorkFlowState.GoOCOLongShort;
                    ProcessWorkFlow();
                }
            }

        }


        public virtual void TradeWorkFlowTradeExitLong()
        {
            if (tracing)
                Print("TradeWorkFlowTradeExitLong()");

            lock (tradeWorkFlowExitTradeLockObject)
            {
                if (IsTradeWorkFlowCanExit())
                {
                    TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeLong;
                    ProcessWorkFlow();
                }
            }

        }

        public virtual void TradeWorkFlowTradeExitShort()
        {
            if (tracing)
                Print("TradeWorkFlowTradeExitShort()");

            lock (tradeWorkFlowExitTradeLockObject)
            {
                if (IsTradeWorkFlowCanExit())
                {
                    TradeWorkFlow = StrategyTradeWorkFlowState.ExitTradeShort;
                    ProcessWorkFlow();
                }
            }

        }

        public void TradeWorkFlowTradeExit(object state)
        {
            this.TradeWorkFlowTradeExit();
        }

        public virtual void TradeWorkFlowTradeExit()
        {
            if (tracing)
                Print("TradeWorkFlowTradeExit()");


            lock (tradeWorkFlowExitTradeLockObject)
            {
                if (IsTradeWorkFlowCanExit())
                {
                    TradeWorkFlow = StrategyTradeWorkFlowState.ExitTrade;
                    ProcessWorkFlow();
                }
            }

        }


        public virtual void TradeWorkFlowTradeExitTransition()
        {
            if (tracing)
                Print("TradeWorkFlowTradeExitTransition()");
            TradeWorkFlow = StrategyTradeWorkFlowState.ExitOnTransition;
            ProcessWorkFlow();
        }


        public virtual void TradeWorkFlowNewOrderCustom(object tradeDirection)
        {
            this.TradeWorkFlowNewOrder((StrategyTradeWorkFlowState)tradeDirection);

        }

        public virtual void TradeWorkFlowNewOrder(StrategyTradeWorkFlowState tradeDirection)
        {
            if (tracing)
                Print("TradeWorkFlowNewOrder tradeDirection " + tradeDirection.ToString());

            //Scenarios to consider
            //Enqueue GoLong WF=GoShortSubmitOrderWorking
            //Enqueue GoShort WF=Waiting - go ahead
            //if   ATSAlgoSystemState = AlgoSystemState.HisTradeRT;  then make sure the old orders are cancelled etc... as it cant see them


            lock (tradeWorkFlowNewOrderLockObject)
            {
                if (tradeDirection == StrategyTradeWorkFlowState.GoLong && !IsTradeWorkFlowCanGoLong())
                {
                    if (tracing)
                        Print("TradeWorkFlowNewOrder rejected " + tradeDirection.ToString());

                    return;
                }
                else if (tradeDirection == StrategyTradeWorkFlowState.GoShort && !IsTradeWorkFlowCanGoShort())
                {
                    if (tracing)
                        Print("TradeWorkFlowNewOrder rejected " + tradeDirection.ToString());

                    return;
                }
                tradeWorkFlowPrior = TradeWorkFlow;
                TradeWorkFlow = tradeDirection;
                ProcessWorkFlow(TradeWorkFlow);
            }
        }

        #endregion
        #region Submit Orders

        private void SubmitOCOBreakoutInternal()
        {
            SubmitOCOBreakout(false);
        }


        public void SubmitOCOBreakout(bool isUser = false)
        {
            entryCount++;
            oCOId = "OCO-" + Guid.NewGuid().ToString();
            orderEntryPrior = orderEntry;
            orderEntry = null;
            string signalLongName = orderEntryOCOLongName + "S#" + entryCount.ToString() + (IsHistorical ? ".H" : isUser ? ".U" : string.Empty);
            string signalShortName = orderEntryOCOShortName + "S#" + entryCount.ToString() + (IsHistorical ? ".H" : isUser ? ".U" : string.Empty);
            SubmitOCOBreakout(oCOId, signalLongName, signalShortName);
        }

        public virtual void SubmitOCOBreakout(string oCOId)
        {
            SubmitOCOBreakout(oCOId, signalLongName: "OCO-L", signalShortName: "OCO-S");
        }

        public virtual void SubmitOCOBreakout(string oCOId, string signalLongName, string signalShortName)
        {

            double rAVG = TechRAVG();

            double longPrice = Math.Max(GetCurrentAsk(0), Highs[0][0]) + rAVG;
            double shortPrice = Math.Min(GetCurrentBid(0), Lows[0][0]) - rAVG;

            orderEntryOCOLong = SubmitOrderUnmanaged(0, OrderAction.Buy, OrderType.StopMarket, this.DefaultQuantity, 0, longPrice, oCOId, signalLongName);
            orderEntryOCOShort = SubmitOrderUnmanaged(0, OrderAction.SellShort, OrderType.StopMarket, this.DefaultQuantity, 0, shortPrice, oCOId, signalShortName);
        }





        public virtual bool SubmitStopLossWillOccur()
        {
            return true;
        }

        public virtual bool SubmitProfitTargetWillOccur()
        {
            return true;
        }

        private void SubmitStopLossInternal()
        {
            if (tracing)
                Print("submitStopLossInternal(" + orderEntry.ToString() + ")");

            if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
            {
                SubmitStopLoss(this.orderEntry);
                return;
            }



            //stopLossOrderConfirmations.Clear();
            lock (OrdersStopLoss)
                OrdersStopLoss.Clear();
            orderStop1 = null;
            orderStop2 = null;
            orderStop3 = null;
            orderStop4 = null;

            if (State == State.Realtime && connectionStatusOrder != ConnectionStatus.Connected)
            {
                throw new Exception("submitStopLossInternal Error - no order connection - unable to submit stoploss");
            }

            SubmitStopLoss(this.orderEntry);
            //add stops to list set graphics

            if (Account.Connection == Connection.PlaybackConnection) return;

            lock (OrdersStopLoss)
            {
                if (orderStop1 != null) OrdersStopLoss.Add(orderStop1);
                if (orderStop2 != null) OrdersStopLoss.Add(orderStop2);
                if (orderStop3 != null) OrdersStopLoss.Add(orderStop3);
                if (orderStop4 != null) OrdersStopLoss.Add(orderStop4);
            }

        }

        private void SubmitProfitTargetInternal()
        {
            if (tracing)
                Print("submitProfitTargetInternal(" + orderEntry.ToString() + ")");


            if (IsHistoricalTradeOrPlayBack || IsStrategyUnSafeMode)
            {
                SubmitProfitTarget(this.orderEntry, this.oCOId);
                return;
            }

            OrdersProfitTarget.Clear();
            orderTarget1 = null;
            orderTarget2 = null;
            orderTarget3 = null;
            orderTarget4 = null;

            if (State == State.Realtime && connectionStatusOrder != ConnectionStatus.Connected)
            {
                throw new Exception("submitProfitTargetInternal Error - no order connection - unable to submit profit target");
            }

            SubmitProfitTarget(this.orderEntry, this.oCOId);

            lock (OrdersProfitTarget)
            {
                if (orderTarget1 != null) OrdersProfitTarget.Add(orderTarget1);
                if (orderTarget2 != null) OrdersProfitTarget.Add(orderTarget2);
                if (orderTarget3 != null) OrdersProfitTarget.Add(orderTarget3);
                if (orderTarget4 != null) OrdersProfitTarget.Add(orderTarget4);
            }

        }

        private void PositionCloseInternal()
        {
            if (tracing)
                Print("PositionCloseInternal()");

            if (State == State.Realtime && connectionStatusOrder != ConnectionStatus.Connected)
            {
                throw new Exception("positionCloseInternal Error - no order connection - unable to cancel working orders and close position");
            }
            if (isLockPositionClose) return;
            lock (lockObjectPositionClose)
            {
                if (isLockPositionClose) return;
                isLockPositionClose = true;
            }
            PositionClose();
            isLockPositionClose = false;

        }

        public void Flatten(object state)
        {
            this.Flatten();
        }

        public void Flatten()
        {
            CancelAllOrders();
            PositionClose();
            TradeWorkFlow = StrategyTradeWorkFlowState.Waiting;
        }


        public virtual void PositionClose()
        {


            if (Position.MarketPosition == MarketPosition.Long)
            {
                if (tracing)
                    Print("PositionClose() > MarketPosition.Long");

                string orderEntryName = orderEntry != null ? orderEntry.Name.Replace(arrowUp, string.Empty) : "Long";
                orderEntryName = orderEntryName.Substring(3);
                orderClose = null;
                orderClose = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Market, Position.Quantity, 0, 0, string.Empty, arrowDown + closeOrderName + orderEntryName);
            }
            else if (Position.MarketPosition == MarketPosition.Short)
            {
                if (tracing)
                    Print("PositionClose() > MarketPosition.Short");

                string orderEntryName = orderEntry != null ? orderEntry.Name.Replace(arrowDown, string.Empty) : "Short";
                orderEntryName = orderEntryName.Substring(3);
                orderClose = null;
                orderClose = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Market, Position.Quantity, 0, 0, string.Empty, arrowUp + closeOrderName + orderEntryName);
            }
            else if (Position.Quantity != 0)
            {
                if (tracing)
                    Print("PositionClose() > Position.Quantity != 0");

                Position.Close();
            }
        }

        public Order SubmitShortTrade(bool isUser = false)
        {


            if (State == State.Realtime && connectionStatusOrder != ConnectionStatus.Connected)
            {
                throw new Exception("submitShortInternal Error - no order connection - unable to submit short order");
            }

            //check for short states working or open
            if (Position.MarketPosition == MarketPosition.Short || (IsOrderIsActive(orderEntry) && orderEntry.OrderAction == OrderAction.SellShort)) return null;
            string signal = entry1NameShort + "M#" + entryCount.ToString() + (IsHistorical ? ".H" : isUser ? ".U" : string.Empty);

            //set the new trade operation state
            entryCount++;
            oCOId = Guid.NewGuid().ToString();

            orderEntryPrior = orderEntry;
            orderEntry = null;

            orderEntryName = signal;

            orderEntry = SubmitShort(signal);

            if (tracing)
                Print("SubmitShortTrade() >> " + orderEntry.ToString());


            return orderEntry;
        }

        public Order SubmitLongTrade(bool isUser = false)
        {



            if (State == State.Realtime && connectionStatusOrder != ConnectionStatus.Connected)
            {
                throw new Exception("submitLongInternal Error - no order connection - unable to submit short order");
            }


            //check for long states working or open
            if (Position.MarketPosition == MarketPosition.Long || (IsOrderIsActive(orderEntry) && orderEntry.OrderAction == OrderAction.Buy)) return null;
            string signal = entry1NameLong + "M#" + entryCount.ToString() + (IsHistorical ? ".H" : isUser ? ".U" : string.Empty);

            orderEntryName = signal;
            //set the new trade operation state
            entryCount++;
            oCOId = Guid.NewGuid().ToString();

            orderEntryPrior = orderEntry;
            orderEntry = null;
            orderEntry = SubmitLong(signal);

            if (tracing)
                Print("SubmitLongTrade() >> " + orderEntry.ToString());

            return orderEntry;
        }

        public virtual void SubmitProfitTarget(Order orderEntry, string oCOId)
        {
            if (tracing)
                Print(Name + "public virtual void SubmitProfitTarget(Order orderEntry, string oCOId) - was Called ");

            if (orderEntry.OrderAction == OrderAction.Buy)
            {
                string orderEntryName = orderEntry != null ? orderEntry.Name.Replace(arrowUp, string.Empty) : "Long";
                orderEntryName = orderEntryName.Substring(3);
                orderTarget1 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, orderEntry.AverageFillPrice + 6 * TickSize, 0, orderEntryName + ".OCO1." + oCOId, arrowDown + target1Name + orderEntryName);
                orderTarget2 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, orderEntry.AverageFillPrice + 10 * TickSize, 0, orderEntryName + ".OCO2." + oCOId, arrowDown + target2Name + orderEntryName);
                orderTarget3 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, orderEntry.AverageFillPrice + 16 * TickSize, 0, orderEntryName + ".OCO3." + oCOId, arrowDown + target3Name + orderEntryName);
                orderTarget4 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.Limit, 1, orderEntry.AverageFillPrice + 24 * TickSize, 0, orderEntryName + ".OCO4." + oCOId, arrowDown + target4Name + orderEntryName);
            }
            else if (orderEntry.OrderAction == OrderAction.SellShort)
            {
                string orderEntryName = orderEntry != null ? orderEntry.Name.Replace(arrowDown, string.Empty) : "Short";
                orderEntryName = orderEntryName.Substring(3);
                orderTarget1 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, orderEntry.AverageFillPrice - 6 * TickSize, 0, orderEntryName + ".OCO1." + oCOId, arrowUp + target1Name + orderEntryName);
                orderTarget2 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, orderEntry.AverageFillPrice - 10 * TickSize, 0, orderEntryName + ".OCO2." + oCOId, arrowUp + target2Name + orderEntryName);
                orderTarget3 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, orderEntry.AverageFillPrice - 16 * TickSize, 0, orderEntryName + ".OCO3." + oCOId, arrowUp + target3Name + orderEntryName);
                orderTarget4 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.Limit, 1, orderEntry.AverageFillPrice - 24 * TickSize, 0, orderEntryName + ".OCO4." + oCOId, arrowUp + target4Name + orderEntryName);
            }
        }

        public virtual void SubmitStopLoss(Order orderEntry)
        {
            if (tracing)
                Print(Name + "public virtual void SubmitStopLoss(Order orderEntry) - was Called ");

            if (orderEntry.OrderAction == OrderAction.Buy)
            {
                string orderEntryName = orderEntry != null ? orderEntry.Name.Replace(arrowUp, string.Empty) : "Long";
                orderEntryName = orderEntryName.Substring(3);

                orderStop1 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, orderEntry.AverageFillPrice - 16 * TickSize, orderEntry.AverageFillPrice - 16 * TickSize, orderEntryName + ".OCO1." + oCOId, arrowDown + stop1Name + orderEntryName);
                orderStop2 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, orderEntry.AverageFillPrice - 18 * TickSize, orderEntry.AverageFillPrice - 16 * TickSize, orderEntryName + ".OCO2." + oCOId, arrowDown + stop2Name + orderEntryName);
                orderStop3 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, orderEntry.AverageFillPrice - 20 * TickSize, orderEntry.AverageFillPrice - 16 * TickSize, orderEntryName + ".OCO3." + oCOId, arrowDown + stop3Name + orderEntryName);
                orderStop4 = SubmitOrderUnmanaged(0, OrderAction.Sell, OrderType.StopMarket, 1, orderEntry.AverageFillPrice - 22 * TickSize, orderEntry.AverageFillPrice - 16 * TickSize, orderEntryName + ".OCO4." + oCOId, arrowDown + stop4Name + orderEntryName);
            }
            else if (orderEntry.OrderAction == OrderAction.SellShort)
            {
                string orderEntryName = orderEntry != null ? orderEntry.Name.Replace(arrowDown, string.Empty) : "Short";
                orderEntryName = orderEntryName.Substring(3);

                orderStop1 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, orderEntry.AverageFillPrice + 16 * TickSize, orderEntry.AverageFillPrice + 16 * TickSize, orderEntryName + ".OCO1." + oCOId, arrowUp + stop1Name + orderEntryName);
                orderStop2 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, orderEntry.AverageFillPrice + 18 * TickSize, orderEntry.AverageFillPrice + 16 * TickSize, orderEntryName + ".OCO2." + oCOId, arrowUp + stop2Name + orderEntryName);
                orderStop3 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, orderEntry.AverageFillPrice + 20 * TickSize, orderEntry.AverageFillPrice + 16 * TickSize, orderEntryName + ".OCO3." + oCOId, arrowUp + stop3Name + orderEntryName);
                orderStop4 = SubmitOrderUnmanaged(0, OrderAction.BuyToCover, OrderType.StopMarket, 1, orderEntry.AverageFillPrice + 22 * TickSize, orderEntry.AverageFillPrice + 16 * TickSize, orderEntryName + ".OCO4." + oCOId, arrowUp + stop4Name + orderEntryName);
            }
        }

        public virtual Order SubmitShort(string signal)
        {
            if (tracing)
                Print(Name + "Order SubmitShort(string signal) - was Called ");

            orderEntry = SubmitOrderUnmanaged(0, OrderAction.SellShort, OrderType.Market, this.DefaultQuantity, 0, 0, String.Empty, signal);
            return orderEntry;
        }

        public virtual Order SubmitLong(string signal)
        {
            if (tracing)
                Print(Name + "SubmitLong(string signal) - was Called ");


            orderEntry = SubmitOrderUnmanaged(0, OrderAction.Buy, OrderType.Market, this.DefaultQuantity, 0, 0, String.Empty, signal);
            return orderEntry;
        }


        #endregion


        #region TechnicalHelpers

        /// <summary>
        /// returns the points average range in points of the last n bars
        /// </summary>
        /// <param name="period"></param>
        /// <param name="barsArrayIndex"></param>
        /// <returns></returns>
        public double TechRAVG(int period = 5, int barsArrayIndex = 0)
        {
            double result = (Highs[barsArrayIndex][0] - Lows[barsArrayIndex][0]);

            if (period > 1)
            {
                List<double> aTRList = new List<double>(period);
                for (int i = 0; i < Math.Min(CurrentBars[barsArrayIndex], period); i++)
                {
                    aTRList.Add(Highs[barsArrayIndex][i] - Lows[barsArrayIndex][i]);
                }
                result = aTRList.Average();
            }

            return Instrument.MasterInstrument.RoundToTickSize(result);
        }
        #endregion


        #region  Logging Tracing
        public void Print(string msg)
        {
            try
            {
                string txt = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:fff") + " Strategy " + this.Name + "/" + base.Id.ToString();

                PrintTo = PrintTo.OutputTab1;

                //account bars and other items not available here in this state

                txt = string.Format("{0} {1} {2} {3} {4}:> {5}", txt, State.ToString(), PositionStateString, ATSAlgoSystemState.ToString(), TradeWorkFlow.ToString(), msg);
                base.Print(txt);

                if (IsSimplePrintMode) return;


                if (State < State.Active || State >= State.Terminated)
                    return;


                //if context here then we are loaded or running and can provide the full gamet of info
                PrintTo = PrintTo.OutputTab2;

                txt += "/" + base.Account.Name;


                if (State >= State.DataLoaded)
                    txt += " " + (Bars != null ? Bars.ToChartString() : "Bars.?");


                txt += " " + (string.IsNullOrEmpty(Thread.CurrentThread.Name) ? "CThread.?" : Thread.CurrentThread.Name);


                if (State >= State.Historical)
                {
                    if (this.Bars != null && Bars.Count > 0 && CurrentBar > -1)
                    {
                        txt += "|BT=" + Time[0].ToString("yyyy.MM.dd HH:mm:ss:fff")
                        + "|HR=" + (IsHistorical ? "H" : "R")
                        + "|CB=" + CurrentBar.ToString()
                        + "|LC=" + Close[0].ToString();
                    }

                    // creating an int var so scope of locking can remain very small and short lived
                    int ordersActiveCount = 0;
                    // locking more for the protection of the rest of the app rather than Logging Tracing which would survive with just the try-catch
                    lock (ordersActiveLockObject)
                        ordersActiveCount = OrdersActive.Count;

                    txt += "|RX=" + Executions.Count.ToString()
                    + "|RO=" + Orders.Count.ToString()
                    + "|MP=" + Position.MarketPosition.ToString()
                    + "|PQ=" + Position.Quantity.ToString()
                    + "|AO=" + ordersActiveCount.ToString()
                    + "|SQ=" + TEQ.Count().ToString()
                    ;
                }

                txt += "|WF=" + this.tradeWorkFlow.ToString()
                + "|S=" + ATSAlgoSystemState.ToString()
                + "|: "
                + msg;




                base.Print(txt);

                if (tracing)
                    TraceToFile(txt);
            }
            catch (Exception ex)
            {
                //careful to avoid print here or use base.Print to avoid StackOverFlow
                string error = "Print() >> Error: " + ex.ToString();
                TraceToFile(error);
                Debug.Print(error);
                Log(error, LogLevel.Error);
            }
        }

        public new void Log(string msg, LogLevel logLevel)
        {
            NinjaScript.Log(msg, logLevel);
            if (tracing)
                TraceToFile(msg);

        }


        public void TraceToFile(string msg)
        {
            if (IsTracingModeRealtimeOnly && State < State.Realtime) return;

            DebugTraceHelper.WriteLine(msg);

#if DEBUG
            Debug.Print(msg);
#endif
        }






        #endregion
        #region DateTime

        private DateTime tEQNextTimeValid = DateTime.MinValue;
        private DateTime tradeWorkFlowNextTimeValid = DateTime.MinValue;
        private DateTime now = Core.Globals.Now;


        [Browsable(false)]
        public DateTime Now
        {
            get
            {
                now = (Cbi.Connection.PlaybackConnection != null ? Cbi.Connection.PlaybackConnection.Now : Core.Globals.Now);

                if (now.Millisecond > 0)
                    now = Core.Globals.MinDate.AddSeconds((long)Math.Floor(now.Subtract(Core.Globals.MinDate).TotalSeconds));

                return now;
            }
        }
        #endregion

        #endregion
        #region properties





        private bool stratCanTrade = true;

        [XmlIgnore, Browsable(false)]
        public virtual bool StratCanTrade
        {
            get
            {
                return this.stratCanTrade;
            }
            set
            {
                if (value != this.stratCanTrade)
                {
                    this.stratCanTrade = value;
                    this.NotifyPropertyChanged("StratCanTrade");
                }
            }
        }

        private bool stratCanTradeLong = true;

        [XmlIgnore, Browsable(false)]
        public virtual bool StratCanTradeLong
        {
            get
            {
                return this.stratCanTradeLong;
            }
            set
            {
                if (value != this.stratCanTradeLong)
                {
                    this.stratCanTradeLong = value;
                    this.NotifyPropertyChanged("StratCanTradeLong");
                }
            }
        }

        private bool stratCanTradeShort = true;

        [XmlIgnore, Browsable(false)]
        public virtual bool StratCanTradeShort
        {
            get
            {
                return this.stratCanTradeShort;
            }
            set
            {
                if (value != this.stratCanTradeShort)
                {
                    this.stratCanTradeShort = value;
                    this.NotifyPropertyChanged("StratCanTradeShort");
                }
            }
        }



        private AlgoSystemState aTSAlgoSystemState = AlgoSystemState.None;

        [XmlIgnore, Browsable(false)]
        public AlgoSystemState ATSAlgoSystemState
        {
            get
            {
                return this.aTSAlgoSystemState;
            }
            set
            {
                if (value != this.aTSAlgoSystemState)
                {
                    this.aTSAlgoSystemState = value;
                    if (aTSAlgoSystemState == AlgoSystemState.HisTradeRT)
                        SystemState = "Hist.RT!";
                    else
                        SystemState = this.aTSAlgoSystemState.ToString();

                    OnATSAlgoSystemStateUpdated(new ATSAlgoSystemStateUpdatedEventArgs(this.aTSAlgoSystemState));
                }
            }

        }

        private AlgoSystemMode aTSAlgoSystemMode = AlgoSystemMode.UnKnown;
        [XmlIgnore, Browsable(false)]
        public AlgoSystemMode ATSAlgoSystemMode
        {
            get
            {
                return this.aTSAlgoSystemMode;
            }
            set
            {
                if (value != this.aTSAlgoSystemMode)
                {
                    this.aTSAlgoSystemMode = value;
                    SystemMode = aTSAlgoSystemMode.ToString();
                }
            }
        }




        private string systemMode = AlgoSystemMode.UnKnown.ToString();
        [XmlIgnore, Browsable(false)]
        public string SystemMode
        {
            get
            {
                return this.systemMode;
            }
            set
            {
                if (value != this.systemMode)
                {
                    this.systemMode = value;
                    this.NotifyPropertyChanged("SystemMode");
                }
            }
        }

        private string systemState = AlgoSystemState.None.ToString();
        [XmlIgnore, Browsable(false)]
        public string SystemState
        {
            get
            {
                return this.systemState;
            }
            set
            {
                if (value != this.systemState)
                {
                    this.systemState = value;
                    this.NotifyPropertyChanged("SystemState");
                }
            }

        }


        private string instrumentFullName = string.Empty;

        [XmlIgnore, Browsable(false)]
        public string InstrumentFullName
        {
            get
            {
                return this.instrumentFullName;
            }
            set
            {
                if (value != this.instrumentFullName)
                {
                    this.instrumentFullName = value;
                    this.NotifyPropertyChanged("InstrumentFullName");
                }
            }
        }



        private string positionInfo = "Algo Trade Manager";
        [XmlIgnore, Browsable(false)]
        public string PositionInfo
        {
            get
            {
                return this.positionInfo;
            }
            set
            {
                if (value != this.positionInfo)
                {
                    this.positionInfo = value;
                    this.NotifyPropertyChanged("PositionInfo");
                }
            }
        }


        private int positionState = 0;

        [XmlIgnore, Browsable(false)]
        public int PositionState
        {
            get { return positionState; }
            set
            {
                if (value != this.positionState)
                {
                    this.positionState = value;
                    this.NotifyPropertyChanged("PositionState");
                }
            }

        }

        [XmlIgnore, Browsable(false)]
        public string PositionStateString
        {
            get
            {
                if (PositionState == 0) return "Flat";
                else if (PositionState > 0) return "Long";
                else return "Short";
            }
        }




        private double unrealizedPL = 0;

        [XmlIgnore, Browsable(false)]
        public double UnRealizedPL
        {
            get
            {
                return this.unrealizedPL;
            }
            set
            {
                if (value != this.unrealizedPL)
                {
                    this.unrealizedPL = value;
                    //this.NotifyPropertyChanged("PositionState");
                    this.NotifyPropertyChanged("UnRealizedPL");
                    this.NotifyPropertyChanged("UnRealizedPLString");
                }
            }
        }

        [XmlIgnore, Browsable(false)]
        public string UnRealizedPLString
        {
            get
            {

                if (PositionState != 0)
                    return Core.Globals.FormatCurrency(UnRealizedPL, accountDenomination);
                else return string.Empty;
            }

        }

        [XmlIgnore, Browsable(false)]
        public string AskPriceString
        {
            get { return FormatPriceMarker(AskPrice); }
        }

        [XmlIgnore, Browsable(false)]
        public string BidPriceString
        {
            get { return FormatPriceMarker(BidPrice); }
        }


        [XmlIgnore, Browsable(false)]
        public double LastPrice
        {
            get
            {
                return this.lastPrice;
            }
            set
            {
                if (value != this.lastPrice)
                {
                    this.lastPrice = value;
                    this.NotifyPropertyChanged("LastPriceString");
                }
            }
        }

        [XmlIgnore, Browsable(false)]
        public string LastPriceString
        {
            get { return FormatPriceMarker(LastPrice); }
        }



        private double askPrice = 0;
        [XmlIgnore, Browsable(false)]
        public double AskPrice
        {
            get
            {
                return this.askPrice;
            }
            set
            {
                if (value != this.askPrice)
                {
                    this.askPrice = value;
                    this.NotifyPropertyChanged("AskPriceString");
                }
            }
        }

        private double bidPrice = 0;

        [XmlIgnore, Browsable(false)]
        public double BidPrice
        {
            get
            {
                return this.bidPrice;
            }
            set
            {
                if (value != this.bidPrice)
                {
                    this.bidPrice = value;
                    this.NotifyPropertyChanged("BidPriceString");

                }
            }
        }





        [Display(GroupName = "Zystem Params", Order = 0, Name = "Strategy Error Handling - OrderCancelStopsOnly", Description = "MT Errors - Error Handling Order Cancel Stops Only -  when using \"Error Handling Single or Batch Order Cancel\"=true - the trade engine will cancel only stop loss exits so that OCOC's are handled broker side for the cancellation of the profit target - avoiding some superflous messaages and rejects for some brokers")]
        public bool IsOrderCancelStopsOnly
        {
            get { return orderCancelStopsOnly; }
            set { orderCancelStopsOnly = value; }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Strategy Error Handling - Single or Batch Order Cancel", Description = "MT Errors - Error Handling Single or Batch Order Cancel -  when set to true when reversing or cancelling - the trade engine will inspect the state of eaach order that is working before cancelling - also only cancelling OCO brackets one side the stops - this is slower but will help deal with order rejection notices from some brokers  - where by the order is not cancellable, part filled, filled or rejected - it is faster to use batch mode - but less resourceful and can cause unneccessary order messages- false - errors flagged by the trade platform in that mode can be handled by setting parameter \"Error Raise On All Order Rejects\"=false and specifying the error code to trap and ignore in the list in the parameter \"Error Native Errors To Ignore\"=error1|error2|etc")]
        public bool IsOrderCancelInspectEachOrDoBatchCancel
        {
            get { return orderCancelInspectEachOrDoBatchCancel; }
            set { orderCancelInspectEachOrDoBatchCancel = value; }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Strategy Error Handling - RaiseErrorOnAllOrderRejects", Description = "MT Errors - Raise Error On All Order Rejects or just when a pending order was rejected but ignoring if the error scenario was in the list of exceptions in \"Error Native Errors To Ignore\"")]
        public bool IsRaiseErrorOnAllOrderRejects
        {
            get { return raiseErrorOnAllOrderRejects; }
            set { raiseErrorOnAllOrderRejects = value; }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Strategy Error Handling - RealtimeErrorHandling", Description = "RealtimeErrorHandling")]
        public new RealtimeErrorHandling RealtimeErrorHandling
        {
            get { return base.RealtimeErrorHandling; }
            set { base.RealtimeErrorHandling = value; }
        }



        [Display(GroupName = "Zystem Params", Order = 0, Name = "DEBUG - IsTracingMode", Description = "Tracing mode true or false - DEBUG developer usage only")]
        public bool IsTracingMode
        {
            get { return tracing; }
            set { tracing = value; }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "DEBUG - IsTracingModeRealtimeOnly", Description = "IsTracingModeRealtimeOnly - DEBUG developer usage only")]
        public bool IsTracingModeRealtimeOnly
        {
            get; set;
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "IsTracingOpenFileOnError", Description = "IsTracingOpenFileOnError")]
        public bool IsTracingOpenFileOnError { get; set; }


        [Display(GroupName = "Zystem Params", Order = 0, Name = "IsSimplePrintMode", Description = "IsSimplePrintMode")]
        public bool IsSimplePrintMode { get; set; }



        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - Realtime Trading Signal Queue", Description = "Realtime Trading Use Queued Signals: True/False - When using small timeseries or every tick this can smooth out performance as signals are queued and the last in is executed others are purged")]
        public bool IsRealtimeTradingUseQueue
        {
            get { return useQueue; }
            set { useQueue = value; }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - IsUseSignalQFallbackForSignals", Description = "IsUseSignalQFallbackForSignals use signal q to catch any missed signals due to busy trade workflow state")]
        public bool IsUseSignalQFallbackForSignals { get; set; }



        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - Strategy Trade Signal Expiry Period", Description = "For any buffered signals due to a series of fast reversals or pending order actions - invalidate and Ignore trade signals of age longer than 3 seconds")]
        public int TradeSignalExpiryInterval
        {
            get; set;

        }


        private int tradeWorkFlowTimeOut = 10;
        //private bool entryOrderInFlightCollision;

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - S TradeWorkFlowTimeOut", Description = "Trade Work Flow Time Out Seconds 10 - if  Workflow is stuck for time out seconds set state to error 1 to 30 seconds.")]
        public int TradeWorkFlowTimeOut
        {
            get { return tradeWorkFlowTimeOut; }
            set { tradeWorkFlowTimeOut = Math.Max(1, Math.Min(30, value)); }
        }



        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - MS TradeWorkFlowTimerInterval", Description = "Trade Work Flow Timer Interval Seconds 1 to 10")]
        public int TradeWorkFlowTimerInterval
        {
            get { return tradeWorkFlowTimerInterval; }
            set { tradeWorkFlowTimerInterval = Math.Max(100, Math.Min(3000, value)); }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - MS TradeWorkFlowTimerInterval Reset", Description = "Trade Work Flow Timer Cycle Reset to allow new trade workflow action - Interval Seconds 1 to 10")]
        public int TradeWorkFlowTimerIntervalReset
        {
            get { return tradeWorkFlowTimerIntervalReset; }
            set { tradeWorkFlowTimerIntervalReset = Math.Max(10, Math.Min(1000, value)); }
        }



        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - TradeWorkFlowRetryAlarm", Description = "Trade Work Flow Retry Alarm 1 to 5 - the number of times to wait for confirmation or retry an action before going to Error state - cancel all and flatten")]
        public int TradeWorkFlowRetryAlarm
        {
            get { return tradeWorkFlowRetryAlarm; }
            set { tradeWorkFlowRetryAlarm = Math.Max(1, Math.Min(5, value)); }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - Realtime Trading Signal Queue Interval", Description = "Trade Event Timer Interval Seconds 1 to 5")]
        public int TEQTimerInterval
        {
            get { return tEQTimerInterval; }
            set { tEQTimerInterval = Math.Max(1, Math.Min(5, value)); }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - IsStrategyUnSafeMode", Description = "WARNING do not use this!!! IsStrategyUnSafeMode allows faster operation for scalping and less latency on entry - however this mode is only for very competent experienced traders as this can result in oder fills and unexpected position fills and order balances... Always keep this off unless you fully understand the risks are yours, and you are an experienced trader in attendance and plan to interact and control any order issues, positions which might result in unsafe mode due to fast market reversal and other anomalies")]
        public bool IsStrategyUnSafeMode { get; set; }




        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - IsFlattenOnTransition", Description = "Realtime Trading Flatten all historical positions and cancel orders - to prevent caveats caused by historical trades becoming realtime and to prevent the need to wait for a historical trade postion to close in realtime prior to realtime trading")]
        public bool IsFlattenOnTransition { get; set; }




        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - RealtimeTradingOnly", Description = "Realtime Trading Only True or False")]
        public bool IsRealtimeTradingOnly
        {
            get { return realtimeTradingOnly; }
            set { realtimeTradingOnly = value; }
        }

        [Display(GroupName = "Zystem Params", Order = 0, Name = "Trade Engine - IsSubmitTargetsAndConfirm", Description = "Confirm Target Placement or skip")]
        public bool IsSubmitTargetsAndConfirm { get; set; }



        [Display(GroupName = "Zystem Params", Order = 0, Name = "Visuals - ShowOrderLabels", Description = "Show Entry Order Labels on chart")]
        public bool IsShowOrderLabels
        {
            get { return showOrderLabels; }
            set { showOrderLabels = value; }
        }





        #region Non browsable


        [Browsable(false)]
        [XmlIgnore()]
        protected Queue<AlgoSignalActionMsq> TEQ
        {
            get
            {
                return this.q;
            }
        }



        [Browsable(false)]
        [XmlIgnore()]
        public DateTime TradeWorkFlowLastChanged { get; private set; }

        [Browsable(false)]
        [XmlIgnore()]
        public int TradeWorkFlowLastChangedBar { get; private set; }


        [Browsable(false)]
        [XmlIgnore()]
        public StrategyTradeWorkFlowState TradeWorkFlow
        {
            get { return tradeWorkFlow; }
            set
            {
                if (tradeWorkFlow != value)
                {
                    TradeWorkFlowLastChanged = DateTime.Now;
                    tradeWorkFlowRetryCount = 0;
                    TradeWorkFlowLastChangedBar = CurrentBars[0];

                    if (tracing)
                        Print("StrategyTradeWorkFlowStates=" + value.ToString());

                    tradeWorkFlow = value;
                    OnStrategyTradeWorkFlowUpdated(new StrategyTradeWorkFlowUpdatedEventArgs(value));
                }

            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public List<Order> OrdersActive
        {
            get
            {
                lock (ordersRT)
                    return ordersRT;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public List<Order> OrdersStopLoss
        {
            get
            {
                lock (ordersStopLoss)
                    return ordersStopLoss;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public List<Order> OrdersProfitTarget
        {
            get
            {
                lock (ordersProfitTarget)
                    return ordersProfitTarget;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public bool IsHistorical { get { return State == State.Historical || ATSAlgoSystemState == AlgoSystemState.HisTradeRT; } }

        [Browsable(false)]
        [XmlIgnore()]
        public bool IsRealtime { get { return ATSAlgoSystemState == AlgoSystemState.Realtime; } }

        [Browsable(false)]
        [XmlIgnore()]
        public bool IsHistoricalTradeOrPlayBack { get { return State == State.Historical || ATSAlgoSystemState == AlgoSystemState.HisTradeRT || IsPlayBack; } }


        [Browsable(false)]
        [XmlIgnore()]
        public bool IsPlayBack { get { return ATSAlgoSystemMode == AlgoSystemMode.Replay; } }

        [Browsable(false)]
        [XmlIgnore()]
        public bool IsBackTest { get { return ATSAlgoSystemMode == AlgoSystemMode.Test; } }

        [Browsable(false)]
        [XmlIgnore()]
        public bool IsSimMode { get { return ATSAlgoSystemMode == AlgoSystemMode.Sim; } }

        [Browsable(false)]
        [XmlIgnore()]
        public bool IsLiveMode { get { return ATSAlgoSystemMode == AlgoSystemMode.Live; } }


        [Browsable(false)]
        [XmlIgnore()]
        public MarketDataEventArgs MarketDataUpdate { get; private set; }






        #endregion

        #endregion
    }

}
