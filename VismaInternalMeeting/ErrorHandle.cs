using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaInternalMeeting
{
    static class ErrorHandle
    {
        public static void Handle(Action func)
        {
            try
            {
                func();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
