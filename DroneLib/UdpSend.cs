using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DroneLib
{
    public class UdpSend
    {
        private IPEndPoint _remoteEndPoint;
        private UdpClient _updClient;

        public UdpSend(string ip, int port)
        {
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _updClient = new UdpClient();

            Debug(_remoteEndPoint.Address + ":" + port);
        }

        public void Close()
        {
            if (_updClient != null)
                _updClient.Close();
        }

        public void Send(string message)
        {
            try
            {
                Debug("Send: " + message);
                byte[] data = Encoding.UTF8.GetBytes(message);
                _updClient.Send(data, data.Length, _remoteEndPoint);
            }
            catch (Exception err)
            {
                Debug(err.ToString());
            }
        }

        private void Debug(string s)
        {
            Console.WriteLine("{0}: {1}", DateTime.Now, s);
        }
    }
}