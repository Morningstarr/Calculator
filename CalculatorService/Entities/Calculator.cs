using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService
{
    public class Calculator
    {
        public double Sum(double first, double second)
        {
            return first + second;
        }

        public double Multiplication(double first, double second)
        {
            return first * second;
        }

        public double Difference(double first, double second)
        {
            return first - second;
        }

        public double Division(double first, double second)
        {
            return first / second;
        }
    }
}
