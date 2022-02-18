//using BWTNT8Interfaces;
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

namespace ATSQuadroStrategyBase.GUI
{
    /// <summary>
    /// Interaction logic for UserControlNinjaBuddy.xaml
    /// </summary>
    public partial class UserControlNinjaBuddy : UserControl
    {

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

        public UserControlNinjaBuddy()
        {
            InitializeComponent();

            try
            {
                shortTextColor = Brushes.Red;
                longTextColor = Brushes.LimeGreen;

                shortBGColor = (Brush)new BrushConverter().ConvertFrom("#FFB70000");
                longBGColor = (Brush)new BrushConverter().ConvertFrom("#FF00D409");
                shortTextColor = (Brush)new BrushConverter().ConvertFrom("#FFFF0000");
                longTextColor = (Brush)new BrushConverter().ConvertFrom("#FF32CD32");


            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
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


    }
}