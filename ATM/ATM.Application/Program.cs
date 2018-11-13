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
            var parser = new Parser(transponderReceiver);
            var filter = new Filter(parser);
            var airspace = new Airspace(filter);
            var updater = new Updater(filter);
            var calculator = new Calculator(updater);
            var sepChecker = new SeperationChecker(updater);
            var render = new Render(updater);
            var output = new Output(render);
            var logger = new Logger(updater, sepChecker);

            Console.ReadLine();
        }
    }
}
