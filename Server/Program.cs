using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 8005;
            const string adress = "127.0.0.1";
            string[] quotes =
            {
                 "Или не берись, или доводи до конца",
                 "Капля долбит камень не силой, но частым падением",
                 "Счастлив тот, кто умело берет под свою защиту то, что любит",
                 "Бесцветные зелёные идеи спят яростно",
                 "Глокая куздра штеко будланула бокра и кудрячит бокрёнка"
            };
            Random random = new Random();

            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(adress), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(point);
                socket.Listen(3);

                Console.WriteLine("Сервер успешно запущен...\n");
                StringBuilder data = new StringBuilder();
                Socket listener = socket.Accept();

                while (data.ToString() != "BYE")
                {
                    byte[] buffer = new byte[256];
                    data = new StringBuilder();

                    do
                    {
                        int size = listener.Receive(buffer);
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    while (listener.Available > 0);

                    Console.WriteLine("Сообщение клиента: " + data.ToString());
                    listener.Send(Encoding.UTF8.GetBytes(quotes[random.Next(0, 5)]));
                };

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}