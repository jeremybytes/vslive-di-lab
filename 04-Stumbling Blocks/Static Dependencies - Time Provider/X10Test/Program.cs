using HouseControl.Library;
using HouseControl.Sunset;
using System;
using System.Threading;

namespace X10Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var sunsetProvider = new SunsetTDD();
            var sunset = sunsetProvider.GetSunset(DateTime.Today.AddDays(1));
            Console.WriteLine("Sunset Tomorrow: {0}", sunset.ToString("G"));

            Console.WriteLine("Starting Test");

            var controller = new HouseController();
            controller.Commander = new FakeCommander();
            controller.SendCommand(5, DeviceCommands.On);
            controller.SendCommand(5, DeviceCommands.Off);

            var currentTime = DateTime.Now;
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(1), 1, DeviceCommands.On);
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(2), 2, DeviceCommands.On);
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(3), 1, DeviceCommands.Off);
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(4), 2, DeviceCommands.Off);


            Console.WriteLine("Test Completed");

            string command = "";
            while (command != "q")
            {
                command = Console.ReadLine();
                if (command == "s")
                {
                    var schedule = controller.GetCurrentScheduleItems();
                    foreach (var item in schedule)
                    {
                        Console.WriteLine("{0} - {1} ({2}), Device: {3}, Command: {4}",
                            item.Info.EventTime.ToString("G"), 
                            item.Info.TimeType.ToString(),
                            item.Info.RelativeOffset.ToString(),
                            item.Device.ToString(), 
                            item.Command.ToString());
                    }
                }
            }
        }
    }
}
