using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace printer
{
    internal class Client
    {
        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient Client)
        {
            string Str = "";
            foreach (printersDataSet.CurrentErorrsRow item in Form1.CEdt)
            {
                Str += item.printer_id + ":" + item.Error + ";";
                // Приведем строку к виду массива байт
                byte[] Buffer = Encoding.ASCII.GetBytes(Str);
                // Отправим его клиенту
                Client.GetStream().Write(Buffer, 0, Buffer.Length);
            }
            Client.Close();
        }
    }

    internal class Server
    {
        private TcpListener Listener; // Объект, принимающий TCP-клиентов

        private static void ClientThread(Object StateInfo)
        {
            new Client((TcpClient)StateInfo);
        }

        // Запуск сервера
        public Server(int Port)
        {
            // Создаем "слушателя" для указанного порта
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start(); // Запускаем его

            Thread Thread = new Thread(new ParameterizedThreadStart(ServerThread));
            Thread.IsBackground = true;
            Thread.Start();
        }

        private void ServerThread(object obj)
        {
            // В бесконечном цикле
            while (true)
            {
                // Принимаем нового клиента
                TcpClient Client = Listener.AcceptTcpClient();
                // Создаем поток
                Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
                Thread.IsBackground = true;
                // И запускаем этот поток, передавая ему принятого клиента
                Thread.Start(Client);
            }
        }

        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }
    }
}