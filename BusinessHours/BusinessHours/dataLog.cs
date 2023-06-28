using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessHoursService
{
    public class dataLog
    {
        public ReserveTime Data;
        public string LogString;
        public DateTime UpdateTime;
        public dataLog(ReserveTime _data, string _LogString)
        {
            Data = _data;
            LogString = _LogString;
            UpdateTime = DateTime.Now;
        }
    }
}
