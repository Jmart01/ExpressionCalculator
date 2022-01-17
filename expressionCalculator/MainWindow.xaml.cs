using System;
using System.Collections.Generic;
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

namespace expressionCalculator
{
    public class Token
    {
        public Token(char character)
        {
            _char = character;
        }

        public bool isOperator()
        {
            return operators.Contains(_char);
        }

        public float Operate(float lhs, float rhs)
        {
            switch(_char)
            {
                case '+':
                    return lhs + rhs;
                    break;
                case '-':
                    return lhs - rhs;
                    break;
                case '*':
                    return lhs * rhs;
                    break;
                case '/':
                    return lhs / rhs;
                default:
                    return 0;
            }
            return 0;
        }

        public float ToFloat()
        {
            return (float)Char.GetNumericValue(_char);
        }
        char[] operators = { '+', '-', '/', '*' };

        char _char;
        public char Char
        {
            get { return _char; }
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static string _Expression;

        private static Token PullNextToken()
        {
            Token nextToken = new Token(_Expression[0]);

            _Expression = _Expression.Remove(0, 1);
            return nextToken;
        }

        private static float GetNextElement()
        {
            if (GetNextExpressionOperator(out Token token))
            {
                if (token.Char == '(')
                {
                    return GetNextExpression();
                }
                else
                {
                    MessageBox.Show("Invalid Expression");
                    return 0;
                }

            }
            else
            {
                return GetNextNumber();
            }
        }

        private static float GetNextNumber()
        {
            string numberString = "";
            bool isNeg = false;

            if (GetNextAdditiveOperator(out Token additiveToken))
            {
                if (additiveToken.Char == '-')
                {
                    isNeg = true;
                }
            }

            while (_Expression.Length > 0)
            {
                if (GetNextNumberComponent(out Token nextToken))
                {
                    numberString += nextToken.Char;
                }
                else
                {
                    break;
                }
            }


            return isNeg ? -float.Parse(numberString) : float.Parse(numberString);
        }

        private static bool GetNextPrimitiveOperator(out Token token)
        {
            char nextChar = _Expression[0];
            if (nextChar == '*' || nextChar == '/')
            {
                token = PullNextToken();
                return true;
            }
            token = new Token(' ');
            return false;
        }
        private static float GetNextExpression()
        {
            float value = GetNextPrimitive();
            while (_Expression.Length > 0)
            {
                if (_Expression.Length > 0 && GetNextExpressionOperator(out Token expressionToken))
                {
                    if (expressionToken.Char == ')')
                    {
                        break;
                    }
                }
                if (!GetNextAdditiveOperator(out Token operatorToken))
                {
                    MessageBox.Show("Invalid thing");
                }
                float nextNumber = GetNextPrimitive();
                value = operatorToken.Operate(value, nextNumber);
                
            }
            return value;
        }

        private static bool GetNextExpressionOperator(out Token token)
        {
            char nextChar = _Expression[0];
            if (nextChar == '(' || nextChar == ')')
            {
                token = PullNextToken();
                return true;
            }
            token = new Token(' ');
            return false;
        }

        private static bool GetNextAdditiveOperator(out Token token)
        {
            char nextChar = _Expression[0];
            if (nextChar == '+' || nextChar == '-')
            {
                token = PullNextToken();
                return true;
            }
            token = new Token(' ');
            return false;
        }

        private static float GetNextPrimitive()
        {
            float value = GetNextElement();
            while (_Expression.Length > 0)
            {
                if (GetNextPrimitiveOperator(out Token operatorToken))
                {
                    float nextElement = GetNextElement();
                    value = operatorToken.Operate(value, nextElement);
                }
                else
                {
                    break;
                }
            }

            return value;
        }

        private static bool GetNextNumberComponent(out Token token)
        {
            char nextChar = _Expression[0];
            if (isCharacterNumberComponent(nextChar))
            {
                token = PullNextToken();
                return true;
            }
            token = new Token(' ');
            return false;
        }

        private static bool isCharacterNumberComponent(char character)
        {
            return char.IsDigit(character) || character == '.';
        }

        private void BTN_Calculate_Click(object sender, RoutedEventArgs e)
        {
            _Expression = TB_Expression.Text;
            float result = GetNextExpression();
            TB_Result.Text = result.ToString();
        }
    }
}
