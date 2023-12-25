using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laba_4_
{
    internal class Program
    {
        static void Main()
        {
            Console.Write("Введите выражение в обычной записи:");
            string expression = Console.ReadLine();
            var result = EvaluateExpression(expression);
            var input = ConvertToRpn(expression);
            Console.WriteLine(string.Join("", input));
            Console.WriteLine("Результат: " + result);
        }



        public static List<object> ConvertToRpn(string str)
        {
            Dictionary<object, int> prioretyDictionary = new Dictionary<object, int>
            {
                {'+', 1},
                {'-', 1},
                {'*', 2},
                {'/', 2},
                {'(', 0},
                {')', 5},
            };
            List<object> prn = new List<object>();
            Stack<object> stack = new Stack<object>();
            string num = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (prioretyDictionary.ContainsKey(str[i]))
                {
                    if (num != string.Empty)
                    {
                        prn.Add(num);
                        num = string.Empty;
                    }

                    if (str[i] == ')')
                    {
                        while ((Char)stack.Peek() != '(')
                        {
                            prn.Add(stack.Pop());
                        }
                        stack.Pop();
                    }
                    else if (stack.Count == 0
                        || str[i] == '('
                        || prioretyDictionary[str[i]] > prioretyDictionary[stack.Peek()])
                    {
                        stack.Push(str[i]);
                    }
                    else if (prioretyDictionary[str[i]] <= prioretyDictionary[stack.Peek()])
                    {
                        while (stack.Count > 0 && (Char)stack.Peek() != '(')
                        {
                            prn.Add(stack.Pop());
                        }
                        stack.Push(str[i]);
                    }
                }
                else
                {
                    num += str[i];
                }
            }
            prn.Add(num);
            while (stack.Count > 0)
            {
                prn.Add(stack.Pop());
            }
            return prn;
        }


        static double EvaluateExpression(string expression)
        {
            Stack<double> numbers = new Stack<double>();
            Stack<char> operations = new Stack<char>();

            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                if (Char.IsDigit(ch))
                {
                    string number = "";
                    while (i < expression.Length && (Char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i];
                        i++;
                    }
                    i--;

                    double num = Double.Parse(number);
                    numbers.Push(num);
                }
                else if (ch == '(')
                {
                    operations.Push(ch);
                }
                else if (ch == ')')
                {
                    while (operations.Peek() != '(')
                    {
                        numbers.Push(PerformOperation(operations.Pop(), numbers.Pop(), numbers.Pop()));
                    }
                    operations.Pop();
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/')
                {
                    while (operations.Count > 0 && Priority(ch, operations.Peek()))
                    {
                        numbers.Push(PerformOperation(operations.Pop(), numbers.Pop(), numbers.Pop()));
                    }
                    operations.Push(ch);
                }
            }

            while (operations.Count > 0)
            {
                numbers.Push(PerformOperation(operations.Pop(), numbers.Pop(), numbers.Pop()));
            }

            return numbers.Pop();
        }

        static bool Priority(char op1, char op2)
        {
            if (op2 == '(' || op2 == ')')
            {
                return false;
            }
            if ((op1 == '*' || op1 == '/') && (op2 == '+' || op2 == '-'))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static double PerformOperation(char operation, double num1, double num2)
        {
            switch (operation)
            {
                case '+':
                    return num2 + num1;
                case '-':
                    return num2 - num1;
                case '*':
                    return num2 * num1;
                case '/':
                    if (num1 != 0)
                    {
                        return num2 / num1;
                    }
                    else
                    {
                        throw new DivideByZeroException("На ноль делить нельзя");
                    }
                default:
                    return 0;
            }
        }
    }
}
