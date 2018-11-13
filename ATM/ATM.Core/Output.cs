using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM.Interfaces;

namespace ATM
{
    public class Output : IOutput
    {
        public Output(IRender render)
        {
            render.WriteLine += WriteLine;
            render.Clear += Clear;
        }

        private void WriteLine(object o, WriteLineEventArgs args)
        {
            Console.WriteLine(args.Output);
        }

        private void Clear(object o, EventArgs args)
        {
            Console.Clear();
        }
    }
}
