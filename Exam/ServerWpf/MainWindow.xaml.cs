using System.Windows;

namespace ServerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //#region Properties
        //private int interval;

        //public int Interval
        //{
        //    get { return interval; }
        //    set { interval = value; }
        //}

        //private int port;

        //public int Port
        //{
        //    get { return port; }
        //    set { port = value; }
        //}
        //private string serverIP;

        //public string ServerIP
        //{
        //    get { return serverIP; }
        //    set { serverIP = value; }
        //}
        //private string email;

        //public string Email
        //{
        //    get { return email; }
        //    set { email = value; }
        //}
        //private int smtpPort;

        //public int SmtpPort
        //{
        //    get { return smtpPort; }
        //    set { smtpPort = value; }
        //}
        //private string password;

        //public string Password
        //{
        //    get { return password; }
        //    set { password = value; }
        //}
        //private string smtpAddress;

        //public string SmtpAddress
        //{
        //    get { return smtpAddress; }
        //    set { smtpAddress = value; }
        //}
        //private IPAddress ipAddress;

        //public IPAddress IpAddress
        //{
        //    get { return ipAddress; }
        //    set { ipAddress = value; }
        //}
        //private List<string> listOfFiles;

        //public List<string> ListOfFiles
        //{
        //    get {
        //        if(listOfFiles==null)
        //        {
        //            listOfFiles= new List<string>();
        //        }
        //        return listOfFiles;
        //    }
        //    set { listOfFiles = value; }
        //}
        //#endregion

        //Timer timer;
        //object synclock = new object();
        //private static readonly ImageConverter _imageConverter = new ImageConverter();
        ////bool sent = false;

        ////private int interval;//+
        ////private string serverIP;//+
        ////private int port;//+
        ////private string email;//+
        ////private string password;//+
        ////private string smtpAddress;
        ////private int smtpPort;//+
        ////private IPAddress ipAddress;//+
        //TcpListener server;
        ////List<string> listOfFiles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
           // this.DataContext = this;
        }

        //private void btStart_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //Int32.TryParse(tbInterval.Text, out interval);
        //        //serverIP = tbIP.Text;
        //        //Int32.TryParse(tbPort.Text, out port);
        //        //email = tbEmail.Text;
        //        //Int32.TryParse(tbSMTPPort.Text, out smtpPort);
        //        //password = tbEmail.Text;
        //        //smtpAddress = tbSMTP.Text;


        //        interval = 10;
        //        serverIP = "127.0.0.1";
        //        port = 5678;
        //        email = "xoreon@gmail.com";
        //        smtpPort = 587;
        //        password = "";
        //        smtpAddress = "smtp.gmail.com";

        //        IPAddress.TryParse(serverIP, out ipAddress);


        //        server = new TcpListener(ipAddress, port);
                
        //        server.Start();
        //        var tcpClient = server.AcceptTcpClient();

        //        NetworkStream stream = tcpClient.GetStream();
        //        byte[] intervalSend = BitConverter.GetBytes(interval);

        //        stream.Write(intervalSend, 0, intervalSend.Length);

        //        do
        //        {
        //            byte[] data = new byte[2*1024*1024];
        //            int imageBytes = stream.Read(data, 0, data.Length); // получаем количество считанных байтов
        //            byte[] imageData = new byte[imageBytes];
        //            Array.Copy(data, imageData, imageBytes);
        //            var message =ByteArrayToBitmap(imageData);
        //            if (message!=null)
        //            {
        //                string fileName=String.Format("{0}.png", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //                message.Save(fileName, ImageFormat.Png);
        //                listOfFiles.Add(fileName);
        //            }
                    
        //            //response.Append(Encoding.UTF8.GetString(inputData, 0, bytes));
        //        }
        //        while (stream.DataAvailable);
        //            timer = new Timer(new TimerCallback(Send), null, 0, 100000);    
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        //private void btStop_Click(object sender, RoutedEventArgs e)
        //{
        //    server.Stop();
        //}

        //public static Bitmap ByteArrayToBitmap(byte[] byteArray)
        //{
        //    Bitmap bitmap = (Bitmap)_imageConverter.ConvertFrom(byteArray);

        //    if (bitmap != null && (bitmap.HorizontalResolution != (int)bitmap.HorizontalResolution ||
        //                       bitmap.VerticalResolution != (int)bitmap.VerticalResolution))
        //    {
        //        bitmap.SetResolution((int)(bitmap.HorizontalResolution + 0.5f),
        //                         (int)(bitmap.VerticalResolution + 0.5f));
        //    }
        //    return bitmap;
        //}

        //private void Send(object obj)
        //{
        //    lock (synclock)
        //    {
        //        SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpAddress, smtpPort);
        //        smtp.EnableSsl = true;
        //        smtp.Credentials = new System.Net.NetworkCredential(email, password);
        //        MailAddress from = new MailAddress(email, "Test");
        //        MailAddress to = new MailAddress("xoreon@gmail.com");
        //        MailMessage m = new MailMessage(from, to);
                
        //        m.Subject = "Изображения от пользователя";
        //        m.Body = "Скриншоты прилагаются";
        //        foreach (var file in listOfFiles)
        //        {
        //            m.Attachments.Add(new Attachment(file));
        //        }
        //        listOfFiles.Clear();
        //        smtp.Send(m);
        //    }
        //}
        //public void Dispose()
        //{ }

    }
}
