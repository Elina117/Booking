using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking
{
    internal class Book
    {
        public int id;
        public string check_in;
        public string check_out;
        public string beds;
        public string breakfast;
        public string price;
        public string extra;
        public string feedback;
        public string score;
        public void Clear()
        {
            id = 0;
            check_in = string.Empty;
            check_out = string.Empty;
            beds = string.Empty;
            breakfast = string.Empty;
            price = string.Empty;
            extra = string.Empty;
            feedback = string.Empty;
            score = string.Empty;
        }
    }
}
