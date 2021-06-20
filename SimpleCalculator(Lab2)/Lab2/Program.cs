using System;
using System.Collections.Generic;
using CalculatorLib;

namespace Lab2
{
    class Program
    {
        static void Main( string[] args )
        {
            if ( args.Length < 1 )
            {
                Console.WriteLine( "Please specify calculation string" );
                return;
            }
            List<IOperation> operations = new List<IOperation>
            {
                new AdditionOperation(),
                new SubstractionOperation(),
                new MultiplicationOperation()
            };
            ICalculator calculator = new SimpleCalculator( operations );
            int result = calculator.Calculate( args[ 0 ] );

            Console.WriteLine( $"Result: {result}" );
        }
    }
}
