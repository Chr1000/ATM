using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Interfaces
{
    public class WriteLineEventArgs : EventArgs
    {
        public string Output { set; get; }
        public WriteLineEventArgs(string output)
        {
            Output = output;
        }
    }
    public interface IRender
    {
        event EventHandler<WriteLineEventArgs> WriteLine;
        event EventHandler<EventArgs> Clear;
    }
}
