using System;

namespace DroneLib
{
    public class DroneApi : IDisposable
    {
        private string ip = "192.168.10.1";
        private int port = 8889;

        private UdpSend udpSend;
        public DroneApi()
        {
            udpSend = new UdpSend(ip, port);
        }
        public void Init()
        {
            udpSend.Send("command");
        }

        public void Dispose()
        {
            udpSend.Close();
        }

        public void TakeOff()
        {
            udpSend.Send("takeoff");
        }
        public void Land()
        {
            udpSend.Send("land");
        }
    }
}
