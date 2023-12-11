using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking
{
    internal class User
    {
        public int id;
        public string surname;
        public string name;
        public string patronymic;
        public string email;
        public string password;

        public void Clear()
        {
            id = 0;
            surname = string.Empty;
            name = string.Empty;
            patronymic = string.Empty;
            email = string.Empty;
            password = string.Empty;

        }
    }
}
