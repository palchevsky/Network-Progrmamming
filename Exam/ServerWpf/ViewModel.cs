using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ServerWpf
{
    public class ViewModel : Base
    {
        #region Properties
        private int interval;

        public int Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                OnPropertyChanged("Interval");
            }
        }

        private int intervalForSmtp;

        public int IntervalForSmtp
        {
            get { return intervalForSmtp; }
            set
            {
                intervalForSmtp = value;
                OnPropertyChanged("IntervalForSmtp");
            }
        }

        private int port;

        public int Port
        {
            get { return port; }
            set
            {
                port = value;
                OnPropertyChanged("Port");
            }
        }
        private string serverIP;

        public string ServerIP
        {
            get { return serverIP; }
            set
            {
                serverIP = value;
                OnPropertyChanged("ServerIP");
            }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        private int smtpPort;

        public int SmtpPort
        {
            get { return smtpPort; }
            set
            {
                smtpPort = value;
                OnPropertyChanged("SmtpPort");
            }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private string smtpAddress;

        public string SmtpAddress
        {
            get { return smtpAddress; }
            set
            {
                smtpAddress = value;
                OnPropertyChanged("SmtpAddress");
            }
        }
        private IPAddress ipAddress;

        public IPAddress IpAddress
        {
            get { return ipAddress; }
            set
            {
                ipAddress = value;
                OnPropertyChanged("IpAddress");
            }
        }
        private List<string> listOfFiles;

        public List<string> ListOfFiles
        {
            get
            {
                if (listOfFiles == null)
                {
                    listOfFiles = new List<string>();
                }
                return listOfFiles;
            }
            set { listOfFiles = value; }
        }
        #endregion

        Timer timer;
        object synclock = new object();
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        //bool sent = false;

        //private int interval;//+
        //private string serverIP;//+
        //private int port;//+
        //private string email;//+
        //private string password;//+
        //private string smtpAddress;
        //private int smtpPort;//+
        //private IPAddress ipAddress;//+
        TcpListener server;
        Thread thread;
        //List<string> listOfFiles = new List<string>();

        public ViewModel()
        {

        }

        private RelayCommand _startServerCommand;

        public RelayCommand StartServerCommand
        {
            get
            {
                if (_startServerCommand == null)
                    _startServerCommand = new RelayCommand(ExecuteStartServerCommand);
                return _startServerCommand;
            }
        }

        private void ExecuteStartServerCommand(object param)
        {
            thread = new Thread(new ThreadStart(StartServer));
            thread.IsBackground = true;
            thread.Start();
            //thread.s
            //StartServer();


            //TimerCallback timeCB = new TimerCallback(Send);

            //Timer time = new Timer(timeCB, null, 0, 1000);

            try
            {
                timer = new Timer(new TimerCallback(Send), null, 20000, 30000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StartServer()
        {
            interval = 10;
            intervalForSmtp = 60;
            serverIP = "127.0.0.1";
            port = 5678;
            smtpPort = 587;
            smtpAddress = "smtp.gmail.com";

            IPAddress.TryParse(serverIP, out ipAddress);
            server = null;

            try
            {
                server = new TcpListener(ipAddress, port);

                server.Start();

                var tcpClient = server.AcceptTcpClient();

                //while (true)
                //{
                NetworkStream stream = tcpClient.GetStream();

                byte[] intervalSend = BitConverter.GetBytes(interval * 1000);

                stream.Write(intervalSend, 0, intervalSend.Length);

                while (stream.CanRead)
                {
                    byte[] data = new byte[2 * 1024 * 1024];
                    int imageBytes = stream.Read(data, 0, data.Length); // получаем количество считанных байтов
                    byte[] imageData = new byte[imageBytes];
                    Array.Copy(data, imageData, imageBytes);
                    var message = ByteArrayToBitmap(imageData);
                    if (message != null)
                    {
                        string fileName = String.Format("{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss"));
                        message.Save(fileName, ImageFormat.Png);
                        ListOfFiles.Add(fileName);
                    }
                }
                //while (stream.CanRead);

                ///в отдельный поток вынести цикл получения
                //timer = new Timer(new TimerCallback(Send), null, 0, intervalForSmtp * 1000);
                //}

                tcpClient.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
                //if(thread.IsAlive)
                //{
                //    thread.Abort();
                //}
                //if(server!=null)
                //{
                //    server.Stop();
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //finally
            //{
            //    if (server != null)
            //    { server.Stop(); }
            //}
        }

        private RelayCommand _stopServerCommand;

        public RelayCommand StopServerCommand
        {
            get
            {
                if (_stopServerCommand == null)
                    _stopServerCommand = new RelayCommand(ExecuteStopServerCommand);
                return _stopServerCommand;
            }
        }

        private void ExecuteStopServerCommand(object param)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
                MessageBox.Show("Server Stopped");
            }
            //server.Stop();
        }

        public static Bitmap ByteArrayToBitmap(byte[] byteArray)
        {
            Bitmap bitmap = (Bitmap)_imageConverter.ConvertFrom(byteArray);

            if (bitmap != null && (bitmap.HorizontalResolution != (int)bitmap.HorizontalResolution ||
                               bitmap.VerticalResolution != (int)bitmap.VerticalResolution))
            {
                bitmap.SetResolution((int)(bitmap.HorizontalResolution + 0.5f),
                                 (int)(bitmap.VerticalResolution + 0.5f));
            }
            return bitmap;
        }

        private void Send(object obj)
        {
            if (ListOfFiles.Count > 0)
            {
                lock (synclock)
                {
                    SmtpClient smtp = new System.Net.Mail.SmtpClient(SmtpAddress, SmtpPort);
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential(Email, Password);
                    MailAddress from = new MailAddress(email, "Test");
                    MailAddress to = new MailAddress("xoreon@gmail.com");
                    MailMessage m = new MailMessage(from, to);

                    m.Subject = "Изображения от пользователя";
                    m.Body = "Скриншоты прилагаются";
                    foreach (var file in ListOfFiles)
                    {
                        m.Attachments.Add(new Attachment(file));
                    }
                    smtp.Send(m);
                    //ListOfFiles.Clear();
                }
            }
            else
            {
                MessageBox.Show("Не отправлено!");
            }
        }
    }
}
