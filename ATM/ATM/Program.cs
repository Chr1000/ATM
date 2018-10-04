using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            var myDataReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            myDataReceiver.TransponderDataReady += TransponderDataHandler;
            Console.ReadKey();
        }
        private static void TransponderDataHandler(object sender, RawTransponderDataEventArgs e)
        {
            Console.Clear();
            Console.WriteLine("Transponder Data Stream:");
            foreach (var rawData in e.TransponderData)
            {
                Console.WriteLine(rawData);
            }
        }
    }
}
