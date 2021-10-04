using CalculatorClient;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CalctorClient = CalculatorClient.Calctor.CalctorClient;



namespace ClientApp.Processors
{
    public class Parser
    {
        private char[] _allowedSymbols = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '+', '/', '*', '(', ')' };

        private CalctorClient _client;

        private string _numbers = "[-]?\\d+\\.?\\d*";

        private string _secondPrio = "[\\+\\-]";

        private string _firstPrio = "[\\*\\/]";

        private string _brackets = "\\(([1234567890\\.\\+\\-\\*\\/]*)\\)";

        public Parser(CalctorClient client)
        {
            _client = client;
        }

        public double Parse(string str)
        {
            double? result = 0;

            //brackets
            BracketsParse(str, out result);
            if (result != null)
            {
                return Result(result);
            }

            //operations
            OperationsParse(str, out result);
            if (result != null)
            {
                return Result(result);
            }

            return Result(double.Parse(str, CultureInfo.InvariantCulture));
        }

        public double Result(double? r)
        {
            try
            {
                return (double)r;
            }
            catch (FormatException)
            {
                throw new FormatException(string.Format("Ошибка во введенном выражении"));
            }
        }

        public double? BracketsParse(string str, out double? res)
        {
            var brackets = Regex.Match(str, _brackets);

            if (brackets.Groups.Count > 1)
            {
                string innerPart = brackets.Groups[0].Value.Substring(1, brackets.Groups[0].Value.Trim().Length - 2);

                string leftPart = str.Substring(0, brackets.Index);
                string rightPart = str.Substring(brackets.Index + brackets.Length);

                return res = Parse(leftPart + Parse(innerPart).ToString(CultureInfo.InvariantCulture) + rightPart);
            }
            return res = null;
        }

        public double? OperationsParse(string str, out double? res)
        {
            var matchFristPrio = Regex.Match(str, string.Format("({0})\\s?({1})\\s?({2})\\s?", _numbers, _firstPrio, _numbers));

            var matchSecondPrio = Regex.Match(str, string.Format("({0})\\s?({1})\\s?({2})\\s?", _numbers, _secondPrio, _numbers));

            var match = matchFristPrio.Groups.Count > 1 ? matchFristPrio : matchSecondPrio.Groups.Count > 1 ? matchSecondPrio : null;
            if (match != null)
            {
                string left = str.Substring(0, match.Index);
                string right = str.Substring(match.Index + match.Length);

                return res = Parse(left + CompleteOperation(match).ToString(CultureInfo.InvariantCulture) + right);
            }
            return res = null;
        }

        private double CompleteOperation(Match match)
        {
            double firstArg = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

            double secondArg = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            switch (match.Groups[2].Value)
            {
                case "+":
                    {
                        return _client.Summa(new Arguments() { First = firstArg, Second = secondArg }).Res;
                    }
                case "-":
                    {
                        return _client.Difference(new Arguments() { First = firstArg, Second = secondArg }).Res;
                    }
                case "*":
                    {
                        return _client.Mult(new Arguments() { First = firstArg, Second = secondArg }).Res;
                    }
                case "/":
                    {
                        return _client.Division(new Arguments() { First = firstArg, Second = secondArg }).Res;
                    }
                default: throw new FormatException($"Неверная входная строка '{match.Value}'");
            }
        }

        public bool SymbolsCheck(string expression)
        {
            foreach (char c in expression)
            {
                if (!_allowedSymbols.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
        public bool BracketsCheck(string expression)
        {
            int openningBrackets = expression.Count(x => x == '(');
            int closingBrackets = expression.Count(x => x == ')');

            if (openningBrackets == closingBrackets)
            {
                return true;
            }
            return false;
        }
    }
}
