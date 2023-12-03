using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int port;
            string adress, message = "";

            do
            {
                Console.Write("Введите номер порта: ");
                port = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите адрес: ");
                adress = Console.ReadLine();

                if (port != 8005 && adress != "127.0.0.1")
                {
                    Console.WriteLine("Неверный номер порта или/и адрес\n");
                }
            }
            while (port != 8005 && adress != "127.0.0.1");

            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(adress), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(point);

                StringBuilder data = new StringBuilder();
                Console.WriteLine();

                while (message != "BYE")
                {
                    Console.Write("Введите сообщение: ");
                    message = Console.ReadLine();

                    socket.Send(Encoding.UTF8.GetBytes(message));
                    byte[] buffer = new byte[256];
                    data = new StringBuilder();

                    do
                    {
                        int size = socket.Receive(buffer);
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    while (socket.Available > 0);

                    Console.WriteLine("Ответ сервера: " + data.ToString());
                };

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}