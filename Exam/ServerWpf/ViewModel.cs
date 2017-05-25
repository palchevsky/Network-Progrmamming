using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace ServerWpf
{
    public class ViewModel : Base
    {
        #region Properties and fields
        private bool _isStartEnabled;

        public bool IsStartEnabled
        {
            get { return _isStartEnabled; }
            set
            {
                _isStartEnabled = value;
                OnPropertyChanged("IsStartEnabled");
            }
        }

        private bool _isStopEnabled;

        public bool IsStopEnabled
        {
            get { return _isStopEnabled; }
            set
            {
                _isStopEnabled = value;
                OnPropertyChanged("IsStopEnabled");
            }
        }

        private int _interval;

        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                OnPropertyChanged("Interval");
            }
        }

        private int _intervalForSmtp;

        public int IntervalForSmtp
        {
            get { return _intervalForSmtp; }
            set
            {
                _intervalForSmtp = value;
                OnPropertyChanged("IntervalForSmtp");
            }
        }

        private int _port;

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged("Port");
            }
        }

        private string _serverIP;

        public string ServerIP
        {
            get { return _serverIP; }
            set
            {
                _serverIP = value;
                OnPropertyChanged("ServerIP");
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private int _smtpPort;

        public int SmtpPort
        {
            get { return _smtpPort; }
            set
            {
                _smtpPort = value;
                OnPropertyChanged("SmtpPort");
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _smtpAddress;

        public string SmtpAddress
        {
            get { return _smtpAddress; }
            set
            {
                _smtpAddress = value;
                OnPropertyChanged("SmtpAddress");
            }
        }

        private IPAddress _ipAddress;

        public IPAddress IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                OnPropertyChanged("IpAddress");
            }
        }

        private List<string> _listOfFiles;

        public List<string> ListOfFiles
        {
            get
            {
                if (_listOfFiles == null)
                {
                    _listOfFiles = new List<string>();
                }
                return _listOfFiles;
            }
            set { _listOfFiles = value; }
        }

        #region Commands
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
        #endregion
        
        #endregion

        private static readonly ImageConverter _imageConverter = new ImageConverter();
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private TcpListener _server;
        private Thread _thread;
        private Timer _timer;
        private object _synclock = new object();

        public ViewModel()
        {
            IsStartEnabled = true;
            IsStopEnabled = false;
        }

        /// <summary>
        /// Обработка команды запуска сервера
        /// </summary>
        /// <param name="param"></param>
        private void ExecuteStartServerCommand(object param)
        {
            if (_thread == null)
            {
                _cts = null;
                _cts = new CancellationTokenSource();
                _token = _cts.Token;

                _thread = new Thread(StartServer);
                _thread.IsBackground = true;
                _thread.Start(_token);

                IsStartEnabled = false;
                IsStopEnabled = true;
            }
        }

        /// <summary>
        /// Запуск сервера
        /// </summary>
        /// <param name="state">Токен отмены</param>
        private void StartServer(object state)
        {
            CancellationToken token = (CancellationToken)state;

            IPAddress.TryParse(ServerIP, out _ipAddress);

            try
            {
                _server = new TcpListener(_ipAddress, Port);

                _server.Start();

                var tcpClient = _server.AcceptTcpClient();

                NetworkStream stream = tcpClient.GetStream();

                byte[] intervalSend = BitConverter.GetBytes(Interval * 1000);

                stream.Write(intervalSend, 0, intervalSend.Length);

                _timer = new Timer(new TimerCallback(Send), null, 20000, IntervalForSmtp * 1000);

                while (stream.CanRead)
                {
                    if (token.IsCancellationRequested)
                    {
                        if (_timer != null)
                        {
                            _timer.Dispose();
                            _timer = null;
                        }
                        break;
                    }
                    byte[] data = new byte[2 * 1024 * 1024];
                    int imageBytes = stream.Read(data, 0, data.Length);
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
                tcpClient.Close();
                if (_server != null)
                { _server.Stop(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                if (_server != null)
                { _server.Stop(); }
            }
        }

        /// <summary>
        /// Обработка команды остановки сервера
        /// </summary>
        /// <param name="param"></param>
        private void ExecuteStopServerCommand(object param)
        {
            IsStartEnabled = true;
            IsStopEnabled = false;
            if (_thread.IsAlive)
            {
                _cts.Cancel();
                _thread = null;
            }

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            if (_server != null)
            {
                _server.Stop();
                _server = null;
            }

            ListOfFiles.Clear();
        }

        /// <summary>
        /// Конвертация массива байтов в Bitmap
        /// </summary>
        /// <param name="byteArray">массив байт</param>
        /// <returns>Изображение Bitmap</returns>
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

        /// <summary>
        /// Отправка электронной почты
        /// </summary>
        /// <param name="obj"></param>
        private void Send(object obj)
        {
            if (ListOfFiles.Count > 0)
            {
                lock (_synclock)
                {
                    try
                    {
                        SmtpClient smtp = new SmtpClient(SmtpAddress, SmtpPort);
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(Email, Password);
                        MailAddress from = new MailAddress(Email, "Скриншот пользователей");
                        MailAddress to = new MailAddress(Email);
                        MailMessage m = new MailMessage(from, to);

                        m.Subject = "Изображения от пользователя";
                        m.Body = "Смотрите вложения";
                        foreach (var file in ListOfFiles)
                        {
                            m.Attachments.Add(new Attachment(file));
                        }
                        smtp.Send(m);
                        ListOfFiles.Clear();
                    }
                    catch (SmtpException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
    }
}
