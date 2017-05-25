namespace Fiddler
{
    partial class FiddlerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbURI = new System.Windows.Forms.Label();
            this.tbURI = new System.Windows.Forms.TextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.tbControl = new System.Windows.Forms.TabControl();
            this.tbRequest = new System.Windows.Forms.TabPage();
            this.tbResponse = new System.Windows.Forms.TabPage();
            this.rtbRequest = new System.Windows.Forms.RichTextBox();
            this.rtbResponse = new System.Windows.Forms.RichTextBox();
            this.tbControl.SuspendLayout();
            this.tbRequest.SuspendLayout();
            this.tbResponse.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbURI
            // 
            this.lbURI.AutoSize = true;
            this.lbURI.Location = new System.Drawing.Point(13, 18);
            this.lbURI.Name = "lbURI";
            this.lbURI.Size = new System.Drawing.Size(29, 13);
            this.lbURI.TabIndex = 0;
            this.lbURI.Text = "URI:";
            // 
            // tbURI
            // 
            this.tbURI.Location = new System.Drawing.Point(49, 14);
            this.tbURI.Name = "tbURI";
            this.tbURI.Size = new System.Drawing.Size(236, 20);
            this.tbURI.TabIndex = 1;
            this.tbURI.TextChanged += new System.EventHandler(this.tbURI_TextChanged);
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(291, 13);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(75, 23);
            this.btSend.TabIndex = 2;
            this.btSend.Text = "Go!";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(16, 49);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(180, 20);
            this.tbSearch.TabIndex = 3;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(203, 48);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 23);
            this.btSearch.TabIndex = 4;
            this.btSearch.Text = "Search";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // tbControl
            // 
            this.tbControl.Controls.Add(this.tbRequest);
            this.tbControl.Controls.Add(this.tbResponse);
            this.tbControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbControl.Location = new System.Drawing.Point(0, 77);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.Size = new System.Drawing.Size(434, 384);
            this.tbControl.TabIndex = 5;
            // 
            // tbRequest
            // 
            this.tbRequest.Controls.Add(this.rtbRequest);
            this.tbRequest.Location = new System.Drawing.Point(4, 22);
            this.tbRequest.Name = "tbRequest";
            this.tbRequest.Padding = new System.Windows.Forms.Padding(3);
            this.tbRequest.Size = new System.Drawing.Size(412, 346);
            this.tbRequest.TabIndex = 0;
            this.tbRequest.Text = "Request";
            this.tbRequest.UseVisualStyleBackColor = true;
            // 
            // tbResponse
            // 
            this.tbResponse.Controls.Add(this.rtbResponse);
            this.tbResponse.Location = new System.Drawing.Point(4, 22);
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.Padding = new System.Windows.Forms.Padding(3);
            this.tbResponse.Size = new System.Drawing.Size(426, 358);
            this.tbResponse.TabIndex = 1;
            this.tbResponse.Text = "Response";
            this.tbResponse.UseVisualStyleBackColor = true;
            // 
            // rtbRequest
            // 
            this.rtbRequest.BackColor = System.Drawing.Color.White;
            this.rtbRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbRequest.Location = new System.Drawing.Point(3, 3);
            this.rtbRequest.Name = "rtbRequest";
            this.rtbRequest.ReadOnly = true;
            this.rtbRequest.Size = new System.Drawing.Size(406, 340);
            this.rtbRequest.TabIndex = 2;
            this.rtbRequest.Text = "";
            // 
            // rtbResponse
            // 
            this.rtbResponse.BackColor = System.Drawing.Color.White;
            this.rtbResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbResponse.Location = new System.Drawing.Point(3, 3);
            this.rtbResponse.Name = "rtbResponse";
            this.rtbResponse.ReadOnly = true;
            this.rtbResponse.Size = new System.Drawing.Size(420, 352);
            this.rtbResponse.TabIndex = 2;
            this.rtbResponse.Text = "";
            // 
            // FiddlerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.tbURI);
            this.Controls.Add(this.lbURI);
            this.MaximumSize = new System.Drawing.Size(1920, 500);
            this.MinimumSize = new System.Drawing.Size(450, 500);
            this.Name = "FiddlerForm";
            this.Text = "Mini Fiddler";
            this.tbControl.ResumeLayout(false);
            this.tbRequest.ResumeLayout(false);
            this.tbResponse.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbURI;
        private System.Windows.Forms.TextBox tbURI;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.TabPage tbRequest;
        private System.Windows.Forms.TabPage tbResponse;
        private System.Windows.Forms.RichTextBox rtbRequest;
        private System.Windows.Forms.RichTextBox rtbResponse;
    }
}

