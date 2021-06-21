using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorLib
{
    public class ExponeniationOperation : IOperation
    {
        public string OperatorCode => "^";
        public int Apply(int operand1, int operand2)
        {
            int result = 1;

            for (int i = 1; i <= operand2; i++)
            {
                result = result * operand1;
            }

            return result;
        }
    }
}
