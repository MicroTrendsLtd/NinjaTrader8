
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Automation;
using System.Windows.Controls.Primitives;
using System.Windows.Ink;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Shell;

namespace ATSQuadroStrategyBase.GUI
{
    /// <summary>
    /// Interaction logic and AutoGen xamnl load and bind  for UserControlNinjaBuddy.xaml
    /// </summary>
    public class UserControlNinjaBuddy : System.Windows.Controls.UserControl
    {
        //NOTE! to prevent anomalies with autogetn using .txt not XAML - causing duplicate object errors and so on
        //E.G: System.Uri resourceLocater = usercontrolninjabuddy.xaml.txt etc
        //The process is to create this from a usercontrol library as open source for loading at runtime etc


        public event RoutedEventHandler OnAutoClick;
        public event RoutedEventHandler OnAutoLongClick;
        public event RoutedEventHandler OnAutoShortClick;

        public event RoutedEventHandler OnBuyClick;
        public event RoutedEventHandler OnSellClick;
        public event RoutedEventHandler OnOCOBreakoutClick;
        public event RoutedEventHandler OnBoxUpperClick;
        public event RoutedEventHandler OnBoxLowerClick;
        public event RoutedEventHandler OnBoxClearClick;

        public event RoutedEventHandler OnCloseClick;
        public event RoutedEventHandler OnTrail50Click;
        public event RoutedEventHandler OnTrailHiLoClick;
        public event RoutedEventHandler OnBreakEvenClick;
        public event RoutedEventHandler OnTrailTriggerClick;
        public event RoutedEventHandler OnTrail1Click;
        public event RoutedEventHandler OnTrail2Click;
        public event RoutedEventHandler OnTrail3Click;
        public event RoutedEventHandler OnTrail4Click;

        private Brush shortBGColor, longBGColor, shortTextColor, longTextColor;


        public DependencyObject InitFromXAML()
        {
            //use txt to stop autogen due to xaml class.
            System.IO.StreamReader mysr = new System.IO.StreamReader(System.IO.Path.Combine(NinjaTrader.Core.Globals.UserDataDir, "bin", "Custom", "Strategies", "ATSQuadroStrategyBase.GUI", "UserControlNinjaBuddy.xaml.txt"));
            UserControl userControl = XamlReader.Load(mysr.BaseStream) as UserControl;

            if (userControl == null)
                return null;

            this.Content = userControl.Content;

            try
            {


                btnBoxClear = LogicalTreeHelper.FindLogicalNode(userControl, "btnBoxClear") as Button;
                if (btnBoxClear != null)
                    btnBoxClear.Click += btnBoxClear_Click;

                btnBreakEven = LogicalTreeHelper.FindLogicalNode(userControl, "btnBreakEven") as Button;
                if (btnBreakEven != null)
                    btnBreakEven.Click += btnBreakEven_Click;

                btnBuy = LogicalTreeHelper.FindLogicalNode(userControl, "btnBuy") as Button;
                if (btnBuy != null)
                    btnBuy.Click += btnBuy_Click;

                btnClosePositions = LogicalTreeHelper.FindLogicalNode(userControl, "btnClosePositions") as Button;
                if (btnClosePositions != null)
                    btnClosePositions.Click += btnClosePositions_Click;

                btnOCOBreakout = LogicalTreeHelper.FindLogicalNode(userControl, "btnOCOBreakout") as Button;
                if (btnOCOBreakout != null)
                    btnOCOBreakout.Click += btnOCOBreakout_Click;

                btnSell = LogicalTreeHelper.FindLogicalNode(userControl, "btnSell") as Button;
                if (btnSell != null)
                    btnSell.Click += btnSell_Click;

                btnSetTrail = LogicalTreeHelper.FindLogicalNode(userControl, "btnSetTrail") as Button;
                if (btnSetTrail != null)
                    btnSetTrail.Click += btnSetTrail_Click;

                btnTrail1 = LogicalTreeHelper.FindLogicalNode(userControl, "btnTrail1") as Button;
                if (btnTrail1 != null)
                    btnTrail1.Click += btnTrail1_Click;

                btnTrail2 = LogicalTreeHelper.FindLogicalNode(userControl, "btnTrail2") as Button;
                if (btnTrail2 != null)
                    btnTrail2.Click += btnTrail2_Click;

                btnTrail3 = LogicalTreeHelper.FindLogicalNode(userControl, "btnTrail3") as Button;
                if (btnTrail3 != null)
                    btnTrail3.Click += btnTrail3_Click;

                btnTrail4 = LogicalTreeHelper.FindLogicalNode(userControl, "btnTrail4") as Button;
                if (btnTrail4 != null)
                    btnTrail4.Click += btnTrail4_Click;

                btnTrail50 = LogicalTreeHelper.FindLogicalNode(userControl, "btnTrail50") as Button;
                if (btnTrail50 != null)
                    btnTrail50.Click += btnTrail50_Click;

                btnTrailHiLo = LogicalTreeHelper.FindLogicalNode(userControl, "btnTrailHiLo") as Button;
                if (btnTrailHiLo != null)
                    btnTrailHiLo.Click += btnTrailHiLo_Click;

                cbAutoLong = LogicalTreeHelper.FindLogicalNode(userControl, "cbAutoLong") as CheckBox;
                if (cbAutoLong != null)
                {
                    cbAutoLong.Checked += cbAutoLong_Checked;
                    cbAutoLong.Unchecked += cbAutoLong_Unchecked;
                }

                cbAutomatedTrading = LogicalTreeHelper.FindLogicalNode(userControl, "cbAutomatedTrading") as CheckBox;
                if (cbAutomatedTrading != null)
                {
                    cbAutomatedTrading.Checked += cbAutomatedTrading_Checked;
                    cbAutomatedTrading.Unchecked += cbAutomatedTrading_Unchecked;
                }

                cbAutoShort = LogicalTreeHelper.FindLogicalNode(userControl, "cbAutoShort") as CheckBox;
                if (cbAutoShort != null)
                {
                    cbAutoShort.Checked += cbAutoShort_Checked;
                    cbAutoShort.Unchecked += cbAutoShort_Unchecked;
                }

                cbLower = LogicalTreeHelper.FindLogicalNode(userControl, "cbLower") as CheckBox;
                if (cbLower != null)
                {
                    cbLower.Checked += cbLower_Checked;
                    //cbLower.Unchecked += cbLower_Unchecked;
                }

                cbUpper = LogicalTreeHelper.FindLogicalNode(userControl, "cbUpper") as CheckBox;
                if (cbUpper != null)
                {
                    cbUpper.Checked += cbUpper_Checked;
                    //cbUpper.Unchecked += cbUpper_Unchecked;
                }


                textPositionState = LogicalTreeHelper.FindLogicalNode(userControl, "textPositionState") as TextBox;
                if (textPositionState != null)
                {
                    textPositionState.TextChanged += textPositionState_TextChanged;
                }


                textUnrealizedPL = LogicalTreeHelper.FindLogicalNode(userControl, "textUnrealizedPL") as TextBox;
                if (textUnrealizedPL != null)
                {
                    textUnrealizedPL.TextChanged += textUnrealizedPL_TextChanged;
                }

                textBoxInstrumentFullName = LogicalTreeHelper.FindLogicalNode(userControl, "textBoxInstrumentFullName") as System.Windows.Controls.TextBlock;
                textBoxHeader = LogicalTreeHelper.FindLogicalNode(userControl, "textBoxHeader") as System.Windows.Controls.TextBlock;
                textBoxHeaderPL = LogicalTreeHelper.FindLogicalNode(userControl, "textBoxHeaderPL") as System.Windows.Controls.TextBlock;
                textBoxSystemMode = LogicalTreeHelper.FindLogicalNode(userControl, "textBoxSystemMode") as System.Windows.Controls.TextBlock;
                textBoxSystemStatus = LogicalTreeHelper.FindLogicalNode(userControl, "textBoxSystemStatus") as System.Windows.Controls.TextBlock;
                textBoxSystemMode = LogicalTreeHelper.FindLogicalNode(userControl, "textBoxSystemMode") as System.Windows.Controls.TextBlock;

                




            }
            catch (Exception ex)
            {
                Debug.Print("InitFromXAML > Error:" + ex.ToString());
            }


            return this.Content as DependencyObject;
        }



        public UserControlNinjaBuddy()
        {
            try
            {
                InitFromXAML();

                shortTextColor = Brushes.Red;
                longTextColor = Brushes.LimeGreen;

                shortBGColor = (Brush)new BrushConverter().ConvertFrom("#FFB70000");
                longBGColor = (Brush)new BrushConverter().ConvertFrom("#FF00D409");
                shortTextColor = (Brush)new BrushConverter().ConvertFrom("#FFFF0000");
                longTextColor = (Brush)new BrushConverter().ConvertFrom("#FF32CD32");


            }
            catch (Exception ex)
            {
                Debug.Print("UserControlNinjaBuddy > Error:" + ex.ToString());
            }

        }

        private void textPositionState_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textPositionState.Text == "1")
            {
                this.textBoxInstrumentFullName.Background = longBGColor;
                this.textBoxInstrumentFullName.Foreground = Brushes.Black;
                this.textBoxHeader.Foreground = longTextColor;
            }
            else if (textPositionState.Text == "-1")
            {
                this.textBoxInstrumentFullName.Background = shortBGColor;
                this.textBoxInstrumentFullName.Foreground = Brushes.White;
                this.textBoxHeader.Foreground = shortTextColor;
            }
            else
            {
                this.textBoxInstrumentFullName.Background = Brushes.Black;
                this.textBoxInstrumentFullName.Foreground = Brushes.White;
                this.textBoxHeader.Foreground = Brushes.White;
            }

        }

        private void textUnrealizedPL_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (textPositionState.Text == "0")
            {
                this.textBoxHeaderPL.Foreground = Brushes.Black;
            }
            else if (textUnrealizedPL.Text.Contains("-"))
            {
                this.textBoxHeaderPL.Foreground = shortTextColor;
            }
            else
            {
                this.textBoxHeaderPL.Foreground = longTextColor;
            }

        }


        private void cbAutomatedTrading_Checked(object sender, RoutedEventArgs e)
        {

            if (OnAutoClick != null) OnAutoClick.Invoke(this, e);
        }

        private void cbAutomatedTrading_Unchecked(object sender, RoutedEventArgs e)
        {
            if (OnAutoClick != null) OnAutoClick.Invoke(this, e);
        }

        private void cbAutoLong_Checked(object sender, RoutedEventArgs e)
        {
            if (OnAutoLongClick != null) OnAutoLongClick.Invoke(this, e);
        }

        private void cbAutoLong_Unchecked(object sender, RoutedEventArgs e)
        {
            if (OnAutoLongClick != null) OnAutoLongClick.Invoke(this, e);
        }

        private void cbAutoShort_Unchecked(object sender, RoutedEventArgs e)
        {
            if (OnAutoShortClick != null) OnAutoShortClick.Invoke(this, e);
        }

        private void cbAutoShort_Checked(object sender, RoutedEventArgs e)
        {
            if (OnAutoShortClick != null) OnAutoShortClick.Invoke(this, e);
        }

        private void btnBoxClear_Click(object sender, RoutedEventArgs e)
        {
            if (OnBoxClearClick != null) OnBoxClearClick.Invoke(this, e);
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            if (OnBuyClick != null) OnBuyClick.Invoke(this, e);
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            if (OnSellClick != null) OnSellClick.Invoke(this, e);
        }

        private void btnOCOBreakout_Click(object sender, RoutedEventArgs e)
        {
            if (OnOCOBreakoutClick != null) OnOCOBreakoutClick.Invoke(this, e);
        }

        private void cbUpper_Checked(object sender, RoutedEventArgs e)
        {
            if (OnBoxUpperClick != null) OnBoxUpperClick.Invoke(this, e);
        }

        private void cbLower_Checked(object sender, RoutedEventArgs e)
        {
            if (OnBoxLowerClick != null) OnBoxLowerClick.Invoke(this, e);
        }

        private void btnClosePositions_Click(object sender, RoutedEventArgs e)
        {
            if (OnCloseClick != null) OnCloseClick.Invoke(this, e);

        }

        private void btnTrail50_Click(object sender, RoutedEventArgs e)
        {
            if (OnTrail50Click != null) OnTrail50Click.Invoke(this, e);
        }

        private void btnTrailHiLo_Click(object sender, RoutedEventArgs e)
        {
            if (OnTrailHiLoClick != null) OnTrailHiLoClick.Invoke(this, e);
        }

        private void btnBreakEven_Click(object sender, RoutedEventArgs e)
        {
            if (OnBreakEvenClick != null) OnBreakEvenClick.Invoke(this, e);
        }

        private void btnSetTrail_Click(object sender, RoutedEventArgs e)
        {
            if (OnTrailTriggerClick != null) OnTrailTriggerClick.Invoke(this, e);
        }

        private void btnTrail1_Click(object sender, RoutedEventArgs e)
        {
            if (OnTrail1Click != null) OnTrail1Click.Invoke(this, e);
        }

        private void btnTrail2_Click(object sender, RoutedEventArgs e)
        {
            if (OnTrail2Click != null) OnTrail2Click.Invoke(this, e);
        }

        private void btnTrail3_Click(object sender, RoutedEventArgs e)
        {
            if (OnTrail3Click != null) OnTrail3Click.Invoke(this, e);
        }

        private void btnTrail4_Click(object sender, RoutedEventArgs e)
        {
            if (OnTrail4Click != null) OnTrail4Click.Invoke(this, e);
        }

        private void HeaderSite_Checked(object sender, RoutedEventArgs e)
        {

        }

        #region AutoGen Code  REM interfaces and init  - to save time just get the locals
#line 10 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ATSQuadroStrategyBase.GUI.UserControlNinjaBuddy UCNinjaBuddy;

#line default
#line hidden


#line 595 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdMain;

#line default
#line hidden


#line 597 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowHeader;

#line default
#line hidden


#line 598 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowAskBid;

#line default
#line hidden


#line 599 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowAlgoSetup;

#line default
#line hidden


#line 600 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowButtonsEntry;

#line default
#line hidden


#line 601 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowBoxBreakout;

#line default
#line hidden


#line 602 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowPostitionManagment;

#line default
#line hidden


#line 603 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowFooter;

#line default
#line hidden


#line 605 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdHeader;

#line default
#line hidden


#line 612 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBoxInstrumentFullName;

#line default
#line hidden


#line 613 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBoxHeader;

#line default
#line hidden


#line 614 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBoxHeaderPL;

#line default
#line hidden


#line 623 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBoxSystemMode;

#line default
#line hidden


#line 624 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBoxSystemStatus;

#line default
#line hidden


#line 628 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdBidAsk;

#line default
#line hidden


#line 643 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdAlgoSetup;

#line default
#line hidden


#line 653 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbAutomatedTrading;

#line default
#line hidden


#line 654 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbAutoLong;

#line default
#line hidden


#line 655 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbAutoShort;

#line default
#line hidden


#line 657 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdButtonsEntry;

#line default
#line hidden


#line 667 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBuy;

#line default
#line hidden


#line 668 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSell;

#line default
#line hidden


#line 669 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOCOBreakout;

#line default
#line hidden


#line 682 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbUpper;

#line default
#line hidden


#line 683 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbLower;

#line default
#line hidden


#line 686 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBoxClear;

#line default
#line hidden


#line 702 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClosePositions;

#line default
#line hidden


#line 703 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTrail50;

#line default
#line hidden


#line 704 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTrailHiLo;

#line default
#line hidden


#line 705 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBreakEven;

#line default
#line hidden


#line 706 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetTrail;

#line default
#line hidden


#line 707 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTrail1;

#line default
#line hidden


#line 708 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTrail2;

#line default
#line hidden


#line 709 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTrail3;

#line default
#line hidden


#line 710 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTrail4;

#line default
#line hidden


#line 717 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textPositionState;

#line default
#line hidden


#line 718 "..\..\UserControlNinjaBuddy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textUnrealizedPL;

        

#line default
#line hidden

        #endregion


    }
}