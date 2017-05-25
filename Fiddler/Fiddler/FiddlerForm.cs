using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Fiddler
{
    public partial class FiddlerForm : Form
    {
        private string site;
        int start = 0;
        int indexOfSearchText = 0;

        public FiddlerForm()
        {
            InitializeComponent();
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            rtbRequest.Clear();
            rtbResponse.Clear();
            try
            {
                var req = (HttpWebRequest)HttpWebRequest.Create(site);
                var resp = (HttpWebResponse)req.GetResponse();

                var respHeaders = resp.Headers;
                var reqHeaders = req.Headers;
                req.UserAgent = "Gecko";

                using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    StringBuilder request = new StringBuilder();

                    StringBuilder response = new StringBuilder();

                    foreach (var item in req.Headers)
                    {
                        rtbRequest.Text += String.Format("{0} : {1} ", item, req.Headers[item.ToString()] + "\n");
                    }

                    foreach (var item in resp.Headers)
                    {
                        rtbResponse.Text += String.Format("{0} : {1}", item, resp.Headers[item.ToString()]) + "\n";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            int startindex = 0;

            var tb = tbControl.SelectedTab;
            if (tb == tbRequest)
            {
                if (rtbRequest.Text.Length > 0)
                    startindex = Search(tbSearch.Text.Trim(), start, rtbRequest.Text.Length);
                if (startindex >= 0)
                {
                    rtbRequest.SelectionColor = Color.Red;
                    int endindex = tbSearch.Text.Length;
                    rtbRequest.Select(startindex, endindex);
                    start = startindex + endindex;
                }
            }
            else if (tb == tbResponse)
            {
                if (rtbResponse.Text.Length > 0)
                    startindex = Search(tbSearch.Text.Trim(), start, rtbResponse.Text.Length);
                if (startindex >= 0)
                {
                    rtbResponse.SelectionColor = Color.Red;
                    int endindex = tbSearch.Text.Length;
                    rtbResponse.Select(startindex, endindex);
                    start = startindex + endindex;
                }
            }

        }

        private void tbURI_TextChanged(object sender, EventArgs e)
        {
            if (!tbURI.Text.Equals(String.Empty))
            {
                site = tbURI.Text.ToLower();
            }
        }

        private int Search(string textToSearch, int searchFrom, int searchTo)
        {
            var tb = tbControl.SelectedTab;

            if (searchFrom > 0 && searchTo > 0 && indexOfSearchText >= 0)
            {
                if (tb == tbRequest)
                {
                    rtbRequest.Undo();
                }
                else
                {
                    rtbResponse.Undo();
                }
            }
            int index = -1;
            if (searchFrom >= 0 && indexOfSearchText >= 0)
            {
                if (searchTo > searchFrom || searchTo == -1)
                {
                    if (tb == tbRequest)
                    {
                        indexOfSearchText = rtbRequest.Find(textToSearch, searchFrom, searchTo, RichTextBoxFinds.None);
                    }
                    else
                    {
                        indexOfSearchText = rtbResponse.Find(textToSearch, searchFrom, searchTo, RichTextBoxFinds.None);
                    }
                    if (indexOfSearchText != -1)
                    {
                        index = indexOfSearchText;
                    }
                }
            }
            return index;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            start = 0;
            indexOfSearchText = 0;
        }
    }
}
