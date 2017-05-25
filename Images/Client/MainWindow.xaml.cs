using Microsoft.Win32;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPHostEntry ipHost;
        private IPAddress ipAddress;
        private IPEndPoint ipEndPoint;

        private Socket socket;
        private bool connected = false;

        private byte[] imageData;

        private int port;
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            port = 5000;
            tbPort.Text = port.ToString();
            ipHost = Dns.GetHostEntry("127.0.0.1");
            ipAddress = ipHost.AddressList[0];
            socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {
            string fileName;
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Filter = "Jpeg Files|*.jpg|Gif files (*.gif)|*.gif|Png files (*.png)|*.png";
            filedialog.ShowDialog();
            fileName = filedialog.FileName;
            if (!fileName.Equals(String.Empty))
            {
                imageData = System.IO.File.ReadAllBytes(filedialog.FileName);
                btConnect.IsEnabled = true;
                btSend.IsEnabled = true;
            }
        }

        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            if (connected == false)
            {
                try
                {
                    Int32.TryParse(tbPort.Text, out port);
                    ipEndPoint = new IPEndPoint(ipAddress, port);
                    if (socket.Connected)
                    {
                        socket.Connect(ipEndPoint);
                    }
                    else
                    {
                        socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(ipEndPoint);
                    }
                    connected = true;
                    btOpen.IsEnabled = true;
                    btDisconnect.IsEnabled = true;
                    btConnect.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Client");
                }
            }
        }

        private void btSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                socket.Send(imageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Client");
            }
        }

        private void btDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (socket.Connected)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    btDisconnect.IsEnabled = false;
                    btConnect.IsEnabled = true;
                    btSend.IsEnabled = false;
                    btOpen.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Client");
                }
            }
            else
            {
                btDisconnect.IsEnabled = false;
                btConnect.IsEnabled = true;
            }
            connected = false;
        }

        private void tbPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Int32.TryParse(tbPort.Text, out port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Client");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Client");
                }
            }
        }
    }
}
