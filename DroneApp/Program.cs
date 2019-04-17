using System;

namespace DroneApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Drone drone = new Drone();

            // Example
            drone.DroneTakeOffAndLand();

            // Comment the above line and uncomment the below to work on the exercise for the mobbing session
            //try
            //{
            //    Console.WriteLine("Starting...");
            //    drone.TestDroneCommandFromSqsAsync("DroneCommandQueue1.fifo").Wait();
            //    //Console.Read();
            //}
            //finally
            //{
            //    drone.DroneLand();
            //    Console.WriteLine("Exiting");
            //}
        }
    }
}