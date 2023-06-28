using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessHoursService
{
    public class ReserveTime
    {
        public List<string>? EveryDayTime;//通用時間，未設定取值
        public List<DayReserveTime> Days;//個別時間
        public int StartTime;//開始時間
        public int EndTime;//結束時間
        public List<int>? Holiday;//公休(星期)
        public List<string>? SHoliday;//公休日
    }
}
