using System;

namespace Calculator
{
    public class CalcEngine
    {
        public enum Operator
        {
            eUnknown,
            eAdd,
            eSubtract,
            eMultiply,
            eDivide,
            ePower,
            eSqrt,
            eReciprocal,
            eSquare,
            eFactorial,
            eCubeRoot
        }

        private double _firstNumber;
        private double _secondNumber;
        private string _currentInput = "0";
        private Operator _currentOperation = Operator.eUnknown;
        private bool _isNewNumber = true;
        private bool _hasDecimal = false;
        private bool _isSecondNumber = false;

        public string CurrentDisplay => _currentInput;

        public void ProcessNumber(string number)
        {
            if (_isNewNumber)
            {
                _currentInput = number;
                _isNewNumber = false;
            }
            else
            {
                _currentInput += number;
            }
        }

        public void ProcessDecimal()
        {
            if (!_hasDecimal)
            {
                if (_isNewNumber)
                {
                    _currentInput = "0,";
                    _isNewNumber = false;
                }
                else
                {
                    _currentInput += ",";
                }
                _hasDecimal = true;
            }
        }

        public void ProcessSign()
        {
            if (_currentInput != "0" && _currentInput != "-0")
            {
                if (_currentInput.StartsWith("-"))
                    _currentInput = _currentInput.Substring(1);
                else
                    _currentInput = "-" + _currentInput;
            }
        }

        public void SetOperation(Operator op)
        {
            if (_currentOperation != Operator.eUnknown && !_isNewNumber)
            {
                Calculate();
            }

            _firstNumber = Convert.ToDouble(_currentInput);
            _currentOperation = op;
            _isNewNumber = true;
            _hasDecimal = false;
            _isSecondNumber = false;

           
            if (IsUnaryOperation(op))
            {
                Calculate();
            }
        }

        private bool IsUnaryOperation(Operator op)
        {
            return op == Operator.eSqrt ||
                   op == Operator.eReciprocal ||
                   op == Operator.eSquare ||
                   op == Operator.eFactorial ||
                   op == Operator.eCubeRoot;
        }

        public void Calculate()
        {
            try
            {
                double result = 0;

                if (!_isNewNumber && !_isSecondNumber)
                {
                    _secondNumber = Convert.ToDouble(_currentInput);
                    _isSecondNumber = true;
                }

                switch (_currentOperation)
                {
                    case Operator.eAdd:
                        result = _firstNumber + _secondNumber;
                        break;
                    case Operator.eSubtract:
                        result = _firstNumber - _secondNumber;
                        break;
                    case Operator.eMultiply:
                        result = _firstNumber * _secondNumber;
                        break;
                    case Operator.eDivide:
                        if (_secondNumber == 0)
                            throw new DivideByZeroException("Division by zero impossible");
                        result = _firstNumber / _secondNumber;
                        break;
                    case Operator.ePower:
                        result = Math.Pow(_firstNumber, _secondNumber);
                        break;
                    case Operator.eSqrt:
                        if (_firstNumber < 0)
                            throw new ArgumentException("You can't take the root of a negative number");
                        result = Math.Sqrt(_firstNumber);
                        break;
                    case Operator.eReciprocal:
                        if (_firstNumber == 0)
                            throw new DivideByZeroException("Division by zero impossible");
                        result = 1 / _firstNumber;
                        break;
                    case Operator.eSquare:
                        result = Math.Pow(_firstNumber, 2);
                        break;
                    case Operator.eFactorial:
                        if (_firstNumber < 0 || _firstNumber != Math.Truncate(_firstNumber))
                            throw new ArgumentException("Factorial for integers only >= 0");
                        result = Factorial((long)_firstNumber);
                        break;
                    case Operator.eCubeRoot:
                        result = Math.Pow(_firstNumber, 1.0 / 3.0);
                        break;
                    case Operator.eUnknown:
                        return;
                }

                _currentInput = result.ToString();
                _firstNumber = result;
                _currentOperation = Operator.eUnknown;
                _isNewNumber = true;
                _hasDecimal = _currentInput.Contains(",");
            }
            catch (Exception ex)
            {
                _currentInput = "Error: " + ex.Message;
            }
        }

        private long Factorial(long n)
        {
            long result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        public void Clear()
        {
            _currentInput = "0";
            _firstNumber = 0;
            _secondNumber = 0;
            _currentOperation = Operator.eUnknown;
            _isNewNumber = true;
            _hasDecimal = false;
            _isSecondNumber = false;
        }

        public void ClearEntry()
        {
            _currentInput = "0";
            _isNewNumber = true;
            _hasDecimal = false;
        }

        public void Backspace()
        {
            if (_currentInput.Length > 1 && _currentInput != "0" && _currentInput != "-0")
            {
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            }
            else
            {
                _currentInput = "0";
                _isNewNumber = true;
                _hasDecimal = false;
            }
        }

        public string SolveQuadratic(double a, double b, double c)
        {
            try
            {
                if (Math.Abs(a) < 1e-10)
                    return "Error: coef a can't be equal 0";

                double discriminant = b * b - 4 * a * c;

                if (discriminant < 0)
                {
                    return "There are no real roots";
                }
                else if (Math.Abs(discriminant) < 1e-10)
                {
                    double x = -b / (2 * a);
                    return $"One root: x = {x:F4}";
                }
                else
                {
                    double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                    return $"Two roots: x₁ = {x1:F4}, x₂ = {x2:F4}";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}