using HouseControl.Library;
using System;

namespace HouseControlAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Controller");

            var controller = new HouseController();

            Console.WriteLine("Controller Running ('q' to quit)");

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
