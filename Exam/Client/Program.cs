using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    class Program
    {
        private static int port;
        private static System.Threading.Timer timer;
        private static string serverIP;
        private static int interval;


        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WindowHeight = 50;
            Console.WindowWidth = 50;
            Console.WriteLine("Please input Server IP-address and port number!");
            Console.WriteLine("IP:\n-> ");
            serverIP = Console.ReadLine();
            Console.Write("Port:\n-> ");
            Int32.TryParse(Console.ReadLine(), out port);
            if (port < 1 || port > 65536)
            {
                Console.WriteLine("Incorrect port number!\n Please try again.");
                Console.Write("Port:\n-> ");
                Int32.TryParse(Console.ReadLine(), out port);
            }

            try
            {
                var bmp = MakeScreenshot();
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(serverIP, port);
                NetworkStream stream = tcpClient.GetStream();

                byte[] inputData = new byte[4];

                do
                {
                    int bytes = stream.Read(inputData, 0, inputData.Length);
                    interval = BitConverter.ToInt32(inputData, 0);
                }
                while (stream.DataAvailable);

                    timer = new System.Threading.Timer(new TimerCallback(
                        x =>
                        {
                            try
                            {
                                if (stream.CanWrite)
                                {
                                    byte[] data = BitmapToByte(MakeScreenshot());
                                    stream.Write(data, 0, data.Length);
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Connection closed!\nCheck the Server!");
                                if (stream != null)
                                { stream.Close(); }
                            }
                        }
                        ), null, 0, interval);
            }
            catch (Exception)
            {
                Console.WriteLine("Connection closed!\nCheck the Server!");
            }
            Console.WriteLine("To close press Enter...");
            Console.ReadLine();
        }

        /// <summary>
        /// Конвертация Bitmap в массив байт
        /// </summary>
        /// <param name="bitmap">изображение в Bitmap</param>
        /// <returns>массив байт</returns>
        private static byte[] BitmapToByte(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Делаем снимок экрана
        /// </summary>
        /// <returns>снимок экрана в Bitmap</returns>
        private static Bitmap MakeScreenshot()
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                 Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                 CopyPixelOperation.SourceCopy);
            return bmpScreenshot;
        }
    }
}
