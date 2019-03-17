using System;
using TA4N.Examples.analysis;
using TA4N.Examples.Loaders;
using TA4N.Examples.Strategies;

namespace TA4N.Examples
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Run the various examples in turn...
            var demo = new Quickstart();
            demo.QuickStart();
            PressAnyKey();

            CsvTicksLoader.Main();
            PressAnyKey();

            CsvTradesLoader.Main();
            PressAnyKey();

            CCICorrectionStrategy.Main();
            PressAnyKey();

            GlobalExtremaStrategy.Main();
            PressAnyKey();

            MovingMomentumStrategy.Main();
            PressAnyKey();

            RSI2Strategy.Main();
            PressAnyKey();

            var s = new StrategyAnalysis();
            s.RunCciCorrection();
            PressAnyKey();

            s.RunGlobalExtrema();
            PressAnyKey();

            s.RunMovingMomentum();
            PressAnyKey();

            s.RunRSI2();
            PressAnyKey();
        }

        private static void PressAnyKey()
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
