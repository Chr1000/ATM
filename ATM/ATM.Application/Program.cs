using ATM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ATM.Core;
using TransponderReceiver;

namespace ATM.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var transponderReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            var transponderdataReader = new Parser(transponderReceiver);
            var airspace = new Airspace();
            var filter = new Filter(airspace, transponderdataReader);
            var calculator = new Calculator();
            var updater = new Updater(filter, calculator);
            var output = new Output();
            var sepChecker = new SeperationChecker();
            var render = new Render(updater, output, sepChecker);

            Console.ReadLine();
        }
    }
}
