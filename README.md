# NinjaTrader8
NinjaTrader8 Components Strategies and Trading tools

ATSQuadroStrategyBase
Algorithmic Trading Framework NinjaTrader 8 UnManaged Trade Engine for Automated Trading Systems.
Developer: Tom Leeson of MicroTrends LTd www.microtrends.pro

About: ATSQuadroStrategyBase is a NinjaTrader 8 Strategy unmanaged mode trade engine base foundation for futures, comprising of 4 Bracket capacity, all In scale out non position compounding,  prevents overfills and builds on functionality provided by the Managed approach for NinjaTrader Strategies. 

GitHub Main Branch:
https://github.com/MicroTrendsLtd/NinjaTrader8/tree/main/ATSQuadroStrategyBase

The main file is:
\ATSQuadroStrategyBase\NinjaTrader 8\bin\Custom\Strategies\AlgoSystemBase.cs

NinjaTrader Links and Info
https://ninjatrader.com/support/forum/forum/ninjascript-file-sharing/ninjascript-file-sharing-discussion/1126945-atsquadrostrategybase-nt8-strategy-foundation-with-samplemacrossover

NinjaTrader APP Share
Simply download and import to NinjaTrader 8 from here
https://ninjatraderecosystem.com/user-app-share-download/atsquadrostrategybase-nt8-strategy-foundation-with-samplemacrossover/


Testing and Reported Issues
Notes we are soak testing with unreasonably fast data series which a retail platform will have no hope at all of providing fills to makle a profit at market.
So the fast market open and 6Tick ES ,10 tick NQ at the NYSE is where we see a lot of caveats thrown up -so we test on super fast crazy conditions and look for stability and fault auto correction and tolerance etc. So for the main part any errors we encounter at 10ticks on NQ will never if very rarely occur on a 150 tick range chart for example and very very unlikely on a 500 and so on.  So our testing is very hard and destructive so normal trading application stresses are easily accomodated.

Trading scope
The system is not designed to take many super fast scalp reversal trades
It might be to do 1 trade per second, however it all depends on market conditions and slippage and what events occur with orders and datafeed etc.
The system is really a generic engine to allow most day trading applications to be used.  Perhpas you can compare it to a vehcicle a SUV - not a racing car or a specialist off road but a good generic vehicle for most trading needs for retail trading terrrain.
