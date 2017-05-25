using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Text;
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
            serverIP = "127.0.0.1";
            //serverIP = Console.ReadLine();
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
                //NetworkStream netStream = tcpClient.GetStream();
                NetworkStream stream = tcpClient.GetStream();





                byte[] inputData = new byte[4];

                do
                {
                    int bytes = stream.Read(inputData, 0, inputData.Length);
                    interval = BitConverter.ToInt32(inputData, 0);

                    //response.Append(Encoding.UTF8.GetString(inputData, 0, bytes));
                }
                while (stream.DataAvailable);

                //do
                //{
   
                    timer = new System.Threading.Timer(new TimerCallback(
                        x =>
                        {
                            if (stream.CanWrite)
                            {
                                byte[] data = BitmapToByte(MakeScreenshot());
                                stream.Write(data, 0, data.Length);
                            }
                        }
                        ), null, 0, interval);
                //}
                //} while (stream.CanWrite);




                //TimeSpan ts = TimeSpan.FromSeconds(interval);
                ///////////////////////
                //    int num = 0;
                //    TimerCallback tm = new TimerCallback(
                //       x => () 
                //    { byte[] data = BitmapToByte(bmp);
                //    stream.Write(data, 0, data.Length);
                //});
                //    // создаем таймер
                //    System.Threading.Timer timer = new System.Threading.Timer(tm, num, 0, interval);


                ////////////////////////////

                //var timer = new Timer(ts);
                //int bytes = stream.Read(inputData, 0, inputData.Length); // получаем количество считанных байтов
                //interval = Encoding.UTF8.GetString(inputData, 0, bytes);
                //                string response = "Hello world!";


                // byte[] data = BitmapToByte(bmp);
                //stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //stream.Close();
                //tcpClient.Close();
            }

            //bmp.Save(String.Format("{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss")), ImageFormat.Png);
            Console.ReadLine();
        }

        private static void Send(object state)
        {
            byte[] data = BitmapToByte(MakeScreenshot());

        }




        private static byte[] BitmapToByte(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }


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
            //bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);
        }
    }
}
