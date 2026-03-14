using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        private CalcEngine _calcEngine;
        private double _memoryValue = 0;
        private bool _isEngineeringMode = false; 

        public MainWindow()
        {
            InitializeComponent();
            _calcEngine = new CalcEngine();

            
            SetStandardMode();
        }

        private void SetStandardMode()
        {
            _isEngineeringMode = false;

            
            EngineeringRow1.Visibility = Visibility.Collapsed;
            EngineeringRow2.Visibility = Visibility.Collapsed;

            
            this.Height = 480;
            this.Title = "Стандартный";

            
            StandardModeMenuItem.IsChecked = true;
            EngineeringModeMenuItem.IsChecked = false;
        }

        private void SetEngineeringMode()
        {
            _isEngineeringMode = true;

            
            EngineeringRow1.Visibility = Visibility.Visible;
            EngineeringRow2.Visibility = Visibility.Visible;

           
            this.Height = 580;
            this.Title = "Инженерный";

            
            StandardModeMenuItem.IsChecked = false;
            EngineeringModeMenuItem.IsChecked = true;
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _calcEngine.ProcessNumber(button.Content.ToString());
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string tag = button.Tag.ToString();

            CalcEngine.Operator op = tag switch
            {
                "Add" => CalcEngine.Operator.eAdd,
                "Subtract" => CalcEngine.Operator.eSubtract,
                "Multiply" => CalcEngine.Operator.eMultiply,
                "Divide" => CalcEngine.Operator.eDivide,
                "Power" => CalcEngine.Operator.ePower,
                "Sqrt" => CalcEngine.Operator.eSqrt,
                "Reciprocal" => CalcEngine.Operator.eReciprocal,
                "Square" => CalcEngine.Operator.eSquare,
                "Factorial" => CalcEngine.Operator.eFactorial,
                "CubeRoot" => CalcEngine.Operator.eCubeRoot,
                _ => CalcEngine.Operator.eUnknown
            };

            _calcEngine.SetOperation(op);
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            _calcEngine.Calculate();
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _calcEngine.Clear();
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            _calcEngine.ClearEntry();
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            _calcEngine.Backspace();
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            _calcEngine.ProcessSign();
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            _calcEngine.ProcessDecimal();
            Display.Text = _calcEngine.CurrentDisplay;
        }

        private void Memory_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string tag = button.Tag.ToString();

            if (double.TryParse(Display.Text, out double currentValue))
            {
                switch (tag)
                {
                    case "MC":
                        _memoryValue = 0;
                        break;
                    case "MR":
                        Display.Text = _memoryValue.ToString();
                        break;
                    case "MS":
                        _memoryValue = currentValue;
                        break;
                    case "MPlus":
                        _memoryValue += currentValue;
                        break;
                }
            }
        }

        private void SolveQuadratic_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new QuadraticDialog();
            if (dialog.ShowDialog() == true)
            {
                string result = _calcEngine.SolveQuadratic(dialog.A, dialog.B, dialog.C);
                Display.Text = result;
            }
        }

        private void StandardMode_Click(object sender, RoutedEventArgs e)
        {
            SetStandardMode();
        }

        private void EngineeringMode_Click(object sender, RoutedEventArgs e)
        {
            SetEngineeringMode();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Display.Text);
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();
                _calcEngine.Clear();

                foreach (char c in text)
                {
                    if (char.IsDigit(c))
                    {
                        _calcEngine.ProcessNumber(c.ToString());
                    }
                    else if (c == ',' || c == '.')
                    {
                        _calcEngine.ProcessDecimal();
                    }
                    else if (c == '-')
                    {
                        _calcEngine.ProcessSign();
                    }
                }
                Display.Text = _calcEngine.CurrentDisplay;
            }
        }

        private void Topmost_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            this.Topmost = item.IsChecked;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Использование калькулятора:\n\n" +
                "• Цифры - ввод чисел\n" +
                "• Операторы: +, -, *, /, xʸ - бинарные операции\n" +
                "• Функции: √, ∛, x², 1/x, n! - унарные операции\n" +
                "• = - вычисление результата\n" +
                "• C - полная очистка\n" +
                "• CE - очистка текущего ввода\n" +
                "• ⌫ - удаление последнего символа\n" +
                "• ± - смена знака\n" +
                "• , - десятичный разделитель\n" +
                "• Кв.ур. - решение квадратного уравнения\n" +
                "• MC, MR, MS, M+ - работа с памятью",
                "Помощь",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Калькулятор\n" +
                "Функциональность:\n" + 
                "Базовые операции (+, -, *, /)\n" +
                "Возведение в степень (xʸ)\n" +
                "Квадратный корень (√)\n" +
                "Кубический корень (∛)\n" +
                "Квадрат числа (x²)\n" +
                "Обратное значение (1/x)\n" +
                "Факториал (n!)\n" +
                "Решение квадратных уравнений\n" +
                "Память (MC, MR, MS, M+)\n" +
                "Инженерный/стандартный режимы",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

           
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                int num = (int)e.Key - (int)Key.D0;
                _calcEngine.ProcessNumber(num.ToString());
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                int num = (int)e.Key - (int)Key.NumPad0;
                _calcEngine.ProcessNumber(num.ToString());
            }
            
            else if (e.Key == Key.Add)
                _calcEngine.SetOperation(CalcEngine.Operator.eAdd);
            else if (e.Key == Key.Subtract)
                _calcEngine.SetOperation(CalcEngine.Operator.eSubtract);
            else if (e.Key == Key.Multiply)
                _calcEngine.SetOperation(CalcEngine.Operator.eMultiply);
            else if (e.Key == Key.Divide)
                _calcEngine.SetOperation(CalcEngine.Operator.eDivide);
            else if (e.Key == Key.Enter || e.Key == Key.Return)
                _calcEngine.Calculate();
            else if (e.Key == Key.OemComma || e.Key == Key.Decimal)
                _calcEngine.ProcessDecimal();
            else if (e.Key == Key.Back)
                _calcEngine.Backspace();
            else if (e.Key == Key.Escape)
                _calcEngine.Clear();

            Display.Text = _calcEngine.CurrentDisplay;
        }
    }
}