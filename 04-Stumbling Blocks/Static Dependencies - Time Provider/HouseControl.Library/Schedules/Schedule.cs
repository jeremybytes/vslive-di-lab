using System;
using System.Collections.Generic;
using System.IO;

namespace HouseControl.Library
{
    public class Schedule : List<ScheduleItem>
    {
        public Schedule(string filename)
        {
            LoadScheduleFromCSV(filename);
        }

        private void LoadScheduleFromCSV(string fileName)
        {
            if (File.Exists(fileName))
            {
                var sr = new StreamReader(fileName);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var fields = line.Split(',');
                    var scheduleItem = new ScheduleItem()
                    {
                        ScheduleSet = fields[0],
                        Info = new ScheduleInfo()
                        {
                            EventTime = ScheduleHelper.Today() +
                                        DateTime.Parse(fields[1]).TimeOfDay,
                            TimeType = (ScheduleTimeType)Enum.Parse(typeof(ScheduleTimeType), fields[2], true),
                            RelativeOffset = TimeSpan.Parse(fields[3]),
                            Type = (ScheduleType)Enum.Parse(typeof(ScheduleType), fields[4], true),
                        },
                        Device = Int32.Parse(fields[5]),
                        Command = (DeviceCommands)Enum.Parse(typeof(DeviceCommands), fields[6]),
                        IsEnabled = bool.Parse(fields[7]),
                    };
                    this.Add(scheduleItem);
                }
                RollSchedule();
            }
        }

        public void RollSchedule()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                var currentItem = this[i];
                if (currentItem.Info.EventTime.IsInPast())
                {
                    switch (currentItem.Info.Type)
                    {
                        case ScheduleType.Daily:
                            currentItem.Info.EventTime =
                                ScheduleHelper.RollForwardToNextDay(currentItem.Info);
                            break;
                        case ScheduleType.Weekday:
                            currentItem.Info.EventTime =
                                ScheduleHelper.RollForwardToNextWeekdayDay(currentItem.Info);
                            break;
                        case ScheduleType.Weekend:
                            currentItem.Info.EventTime =
                                ScheduleHelper.RollForwardToNextWeekendDay(currentItem.Info);
                            break;
                        case ScheduleType.Once:
                            this.RemoveAt(i);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

    }
}
