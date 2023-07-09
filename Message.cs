using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace message_server
{
    public class Message
    {
        public string senderNumber {  get; set; }
        public string targetNumber {  get; set; }
        public string data { get; set; }
    }
}
