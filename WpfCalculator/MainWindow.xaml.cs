using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MOC;

namespace WpfCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Brush Color { get; set; } = Brushes.CadetBlue;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        #region Fields

        SCalculator calculator = new();

        bool reset = false;

        private string view = " ";

        public string View
        {
            get { return view; }
            set { view = value; OnPropertyChanged(); }
        }


        private string content = "0";

        public string Display
        {
            get { return content; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    content = value;
                else
                    content = "0";
                OnPropertyChanged();
            }
        }

        #endregion

        #region Binding

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Funcs

        bool IsNumPart(char ch) => (char.IsDigit(ch) || ch == '.');

        bool IsNumPart(string ch) => ch.Length == 1 ? (char.IsDigit(ch.First()) || ch == ",") : false;

        void AssignNumber(string num)
        {
            if (Display == "0" || reset)
                Display = num;
            else
                Display += num;
            reset = false;
        }

        void BackSpace()
        {
            int index = Display.Length - 1;
            string s = Display.Remove(index);
            Display = s;
        }

        void CE() => Display = "0";

        void ChangePM()
        {
            if (Display.First().IsNumber())
                Display = $"-{Display}";
            else if (Display.First() == '-')
                Display = Display.TrimStart('-');
        }

        void Fill()
        {
            Display = calculator.Result.ToString();
            View = calculator.EquationForView;
            reset = true;
        }

        char CheckView()
        {
            if (string.IsNullOrWhiteSpace(View))
                return ' ';
            char last = View.Last();
            if (!(last == '+' || last == '-' || last == '/' || last == '*'))
            {
                return '*';
            }
            return ' ';
        }

        void Calculate(string block)
        {
            try
            {
                calculator.Add(block);
                Fill();
            }
            catch (ArgumentException ex)
            {
                CE();
                calculator.C();
                Fill();
                Display = ex.Message;
                reset = true;
            }
        }

        bool equalsMode = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (equalsMode)
                {
                    CE();
                    calculator.C();
                    equalsMode = false;
                }

                string btnC = btn.Content.ToString();
                if (IsNumPart(btnC))
                    AssignNumber(btnC);
                else if (btn.Content.ToString() == "🢤")
                    BackSpace();
                else if (btnC == "CE")
                    CE();
                else if (btnC == "C")
                {
                    calculator.C();
                    Fill();
                }
                else if (btnC == "+/-")
                    ChangePM();
                else if (btnC == "π")
                    Display = MOC.General.Pi.ToString();
                else if (btnC == "e")
                    Display = General.E.ToString();
                else if (btnC == "2^x")
                {
                    Display = calculator.Cal.Power(2, Convert.ToDouble(Display)).ToString();
                }
                else if (btnC == "10^x")
                {
                    Display = calculator.Cal.Power(10, Convert.ToDouble(Display)).ToString();
                }
                else if (btnC == "n!")
                {
                    Calculate($"{CheckView()}!{Display}");
                }
                else if (btnC == "|x|")
                    Calculate($"{CheckView()}|{calculator.Cal.Pip(Convert.ToDouble(Display))}");
                else if (btnC == "=")
                {
                    Calculate(Display);
                    reset = true;
                    equalsMode = true;
                }
                else
                    Calculate($"{Display}{btnC}");

            }
        }

        #endregion
    }
}
