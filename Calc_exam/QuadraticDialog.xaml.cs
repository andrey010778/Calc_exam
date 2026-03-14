using System;
using System.Windows;

namespace Calculator
{
    public partial class QuadraticDialog : Window
    {
        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }

        public QuadraticDialog()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            
            if (!double.TryParse(TextBoxA.Text, out double a))
            {
                MessageBox.Show("Введите корректное число для коэффициента a", "Ошибка ввода");
                TextBoxA.Focus();
                return;
            }

            if (!double.TryParse(TextBoxB.Text, out double b))
            {
                MessageBox.Show("Введите корректное число для коэффициента b", "Ошибка ввода");
                TextBoxB.Focus();
                return;
            }

            if (!double.TryParse(TextBoxC.Text, out double c))
            {
                MessageBox.Show("Введите корректное число для коэффициента c", "Ошибка ввода");
                TextBoxC.Focus();
                return;
            }

            
            if (Math.Abs(a) < 1e-10)
            {
                MessageBox.Show("Коэффициент a не может быть равен нулю", "Ошибка");
                TextBoxA.Focus();
                return;
            }

            A = a;
            B = b;
            C = c;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}