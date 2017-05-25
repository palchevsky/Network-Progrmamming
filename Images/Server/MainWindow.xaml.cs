using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPEndPoint ipEndPoint;
        private IPAddress ipAddress= Dns.GetHostEntry("127.0.0.1").AddressList[0];
        private Socket socket;
        private Socket handler;
        private bool started = false;
        private Thread thread;

        private int port;
        public int Port
        {
            get
            {
                if (port == 0)
                {
                    port = 5000;
                    return port;
                }
                return port;
            }
            set { port = value; }
        }

        private ObservableCollection<BitmapImage> imagesCollection;
        public ObservableCollection<BitmapImage> ImagesCollection
        {
            get
            {
                if (imagesCollection == null)
                {
                    imagesCollection = new ObservableCollection<BitmapImage>();
                    return imagesCollection;
                }
                else
                { return imagesCollection; }
            }
            set { imagesCollection = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            handler = socket;
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            if (started == false)
            {
                try
                {
                    started = true;
                    btStart.IsEnabled = false;
                    btStop.IsEnabled = true;
                    Int32.TryParse(tbPort.Text, out port);
                    thread = new Thread( new ThreadStart(ProcessAndRespond));
                    thread.IsBackground = true;
                    thread.Start();
                    lbStatus.Content = "Connected";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Server");
                }
            }
        }

        private void ProcessAndRespond()
        {
            try
            {
                if (ipEndPoint == null)
                {
                    ipEndPoint = new IPEndPoint(ipAddress, Port);
                }
                else
                {
                    ipEndPoint.Port = Port;
                }

                socket.Bind(ipEndPoint);
                socket.Listen(10);

                handler = socket.Accept();
                while (started)
                {
                    byte[] imageBuffer = new byte[2 * 1024 * 1024]; //2Mb
                    int bytesRead = handler.Receive(imageBuffer);
                    byte[] image = new byte[bytesRead];
                    Buffer.BlockCopy(imageBuffer, 0, image, 0, bytesRead);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        ImagesCollection.Add(ToImage(image));
                    }));
                    var reply = String.Format("Image of size {0} received.", bytesRead);
                    byte[] replyArray = Encoding.UTF8.GetBytes(reply);
                    handler.Send(replyArray);
                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.NativeErrorCode + " " + se.Message, "Server");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server");
            }
        }

        private void btStop_Click(object sender, RoutedEventArgs e)
        {
            if (started == true)
            {
                started = false;
                btStart.IsEnabled = true;
                btStop.IsEnabled = false;
                lbStatus.Content = "Disconnected";
                if (handler.Connected)
                {
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    
                }
            }
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (handler.Connected)
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}
