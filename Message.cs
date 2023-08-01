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
        public string username { get; set; }
        public string data { get; set; }
        public int type { get; set; }
        public int offset { get; set; }
        public int part { get; set; }
        public string fileName { get; set; }
        public string fileExtension { get; set; }
    }
}
