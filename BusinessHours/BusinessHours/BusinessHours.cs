using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BusinessHoursService
{
    public class BusinessHours:IBusinessHours<ReserveTime>
    {
        private ReserveTime _reserveTime;
        private string _LogString = "";

        public BusinessHours()
        {
            getJson();
        }
        /* 防止外部直接更改，new新物件 */
        public ReserveTime GetAll() => new()
        {
            EveryDayTime = _reserveTime.EveryDayTime,
            Days = _reserveTime.Days,
            StartTime = _reserveTime.StartTime,
            EndTime = _reserveTime.EndTime,
            Holiday = _reserveTime.Holiday,
            SHoliday = _reserveTime.SHoliday
        };
        public List<string> getReserveTime() => _reserveTime.EveryDayTime;
        public int getStartTime() => _reserveTime.StartTime;
        public int getEndTime() => _reserveTime.EndTime;
        public List<int> getHoliday() => _reserveTime.Holiday;

        public void ChangeEveryDayTime(IFormCollection edt)
        {
            _reserveTime.EveryDayTime = getTime(edt);
            setLogString("EveryDayTime");
            saveJson();
        }
        public void ChangeDayTime(IFormCollection data)
        {
            DayReserveTime day = new()
            {
                Day = Convert.ToInt32(data["day"]),
                EveryDayTime = getTime(data)
            };
            if (_reserveTime.Days.Select(x => x.Day).Contains(day.Day))
                _reserveTime.Days.Remove(_reserveTime.Days.Find(x => x.Day == day.Day));
            if (day.EveryDayTime.Count != 0)
                _reserveTime.Days.Add(day);
            setLogString("DayTime " + day.Day.ToString());
            saveJson();
        }
        public void ChangeWorkTime(IFormCollection data)
        {
            foreach (var item in data)
                if (item.Key == "StartTime")
                    _reserveTime.StartTime = Convert.ToInt32(item.Value);
                else
                    _reserveTime.EndTime = Convert.ToInt32(item.Value);
            setLogString("WorkTime");
            //更改EveryDayTime值
            saveJson();
        }
        public void ChangeHoliday(IFormCollection data)
        {
            List<int> newHoliday = new();
            for (int i = 0; i < 7; i++)
                if (data[key: $"{i}"].ToString() == "on")
                    newHoliday.Add(i);
            _reserveTime.Holiday = newHoliday;
            setLogString("Holiday");
            saveJson();
        }
        public void ChangeSHoliday(IFormCollection data)
        {
            if (!data.ContainsKey("Cancel"))
            {
                if (_reserveTime.SHoliday == null)
                    _reserveTime.SHoliday = new();
                if (!_reserveTime.SHoliday.Contains(data["SHoliday"]))
                    _reserveTime.SHoliday.Add(data["SHoliday"]);
            }
            else
                _reserveTime.SHoliday.Remove(data["Cancel"]);
            setLogString("SHoliday");
            saveJson();
            //Call GoogleCalendar Api to insert Event
        }
        private void saveJson()
        {
            string jsonData = JsonConvert.SerializeObject(_reserveTime);
            using (StreamWriter stream = new("wwwroot/Time.json"))
            {
                stream.Write(jsonData);
            }
            InsertLog();
        }
        private void getJson()
        {
            string jsonString = "";
            using (StreamReader stream = new("wwwroot/Time.json"))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                    jsonString += line;
            }
            _reserveTime = JsonConvert.DeserializeObject<ReserveTime>(jsonString);
            for (int i = 0; i < _reserveTime.SHoliday.Count; i++)
                if (Convert.ToDateTime(_reserveTime.SHoliday[i]) < DateTime.Today)
                    _reserveTime.SHoliday.Remove(_reserveTime.SHoliday[i]);
        }
        private List<string> getTime(IFormCollection data)
        {
            List<string> newedt = new();
            for (int i = _reserveTime.StartTime - 1; i < _reserveTime.EndTime; i++)
                if (data[key: $"{i}"].ToString() == "on")
                    newedt.Add($"{i:00}:00");
            return newedt;
        }
        private void InsertLog()
        {
            List<dataLog> logs = getDataLog();
            logs.Add(new(_reserveTime, _LogString.Substring(0, _LogString.Length - 1)));
            string jsonData = JsonConvert.SerializeObject(logs);
            using StreamWriter stream = new("wwwroot/Log.json");
            stream.Write(jsonData);
        }
        private List<dataLog> getDataLog()
        {
            string jsonString = "";
            using (StreamReader stream = new("wwwroot/Log.json"))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                    jsonString += line;
            }
            return JsonConvert.DeserializeObject<List<dataLog>>(jsonString) ?? (new());
        }
        private void setLogString(string Type) => _LogString += Type + ",";
    }
}