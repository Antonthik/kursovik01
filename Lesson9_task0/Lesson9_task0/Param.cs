using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson9_task0
{
    class Param
    {
        public string CurrentPuth { get; set; }
        public int Page { get; set; }

        public Param(string currentPuth,int page)
        {
            CurrentPuth = currentPuth;
            Page = page;

        }
    }
}
