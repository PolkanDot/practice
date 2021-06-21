using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorLib
{
    public class RemainderOperation : IOperation
    {
        public string OperatorCode => "%";

        public int Apply(int operand1, int operand2)
        {
            return operand1 % operand2;
        }
    }
}
