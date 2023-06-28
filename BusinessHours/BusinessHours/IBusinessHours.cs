using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessHoursService
{
    public interface IBusinessHours<T>
    {
        /* UPDATE Method */
        void ChangeEveryDayTime(IFormCollection edt);
        void ChangeDayTime(IFormCollection data);
        void ChangeWorkTime(IFormCollection data);
        void ChangeHoliday(IFormCollection data);
        void ChangeSHoliday(IFormCollection data);
        /* GET Method */
        T GetAll();
        List<string> getReserveTime();
        List<int> getHoliday();
        int getStartTime();
        int getEndTime();
    }
}
