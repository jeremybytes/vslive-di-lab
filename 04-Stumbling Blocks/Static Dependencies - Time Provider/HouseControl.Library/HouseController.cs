using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace HouseControl.Library
{
    public class HouseController
    {
        private ICommander commander;
        public ICommander Commander
        {
            get
            {
                if (commander == null)
                    commander = new SerialCommander();
                return commander;
            }
            set
            {
                if (commander != value)
                    commander = value;
            }
        }

        private Timer scheduler = new Timer(60000);
        private Schedule schedule = new Schedule(
            AppDomain.CurrentDomain.BaseDirectory + "ScheduleData.txt");

        public HouseController()
        {
            scheduler.Elapsed += scheduler_Elapsed;
            scheduler.AutoReset = true;
            scheduler.Start();
        }

        private void scheduler_Elapsed(object sender, ElapsedEventArgs e)
        {
            var itemsToProcess = schedule.Where(si =>
                si.IsEnabled &&
                si.Info.EventTime.DurationFromNow() < TimeSpan.FromSeconds(30));

            foreach (var item in itemsToProcess)
                SendCommand(item.Device, item.Command);

#if DEBUG
            Console.Write("Schedule Items Processed: {0} - ",
                itemsToProcess.Count().ToString());
#endif

            schedule.RollSchedule();

#if DEBUG
            Console.WriteLine("Total Items: {0} - Active Items: {1}",
                schedule.Count.ToString(),
                schedule.Count(si => si.IsEnabled));
#endif
        }

        public void ResetAll()
        {
            for (int i = 1; i <= 8; i++)
            {
                SendCommand(i, DeviceCommands.Off);
            }
        }

        public void ScheduleOneTimeItem(DateTime time, int device,
            DeviceCommands command)
        {
            var scheduleItem = new ScheduleItem()
            {
                Device = device,
                Command = command,
                Info = new ScheduleInfo()
                {
                    EventTime = time,
                    Type = ScheduleType.Once,
                },
                IsEnabled = true,
                ScheduleSet = "",
            };
            schedule.Add(scheduleItem);
        }

        public void SendCommand(int device, DeviceCommands command)
        {
            var message = MessageGenerator.GetMessage(device, command);
            Commander.SendCommand(message);
            Console.WriteLine("{0} - Device: {1}, Command: {2}",
                DateTime.Now.ToString("G"), device.ToString(), command.ToString());
        }

        public List<ScheduleItem> GetCurrentScheduleItems()
        {
            var result = new List<ScheduleItem>();
            foreach (var item in schedule.Where(i => i.IsEnabled))
                result.Add(item);
            return result;
        }
    }
}
