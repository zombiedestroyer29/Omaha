
namespace HaloBlobViewer
{
    partial class CacheSelector
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.NumTagsText = new System.Windows.Forms.TextBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.LoadCachesButton = new System.Windows.Forms.Button();
            this.CachesToLoadCheckList = new System.Windows.Forms.CheckedListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.NumTagsText);
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.LoadCachesButton);
            this.panel1.Controls.Add(this.CachesToLoadCheckList);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(216, 216);
            this.panel1.TabIndex = 0;
            // 
            // NumTagsText
            // 
            this.NumTagsText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumTagsText.BackColor = System.Drawing.Color.Gray;
            this.NumTagsText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NumTagsText.Location = new System.Drawing.Point(3, 175);
            this.NumTagsText.Name = "NumTagsText";
            this.NumTagsText.ReadOnly = true;
            this.NumTagsText.Size = new System.Drawing.Size(210, 13);
            this.NumTagsText.TabIndex = 4;
            this.NumTagsText.Text = "No Cache(s) Selected";
            // 
            // CancelButton
            // 
            this.CancelButton.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CancelButton.Location = new System.Drawing.Point(109, 190);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(100, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Gray;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(210, 13);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Select Tag Caches To Load:";
            // 
            // LoadCachesButton
            // 
            this.LoadCachesButton.BackColor = System.Drawing.Color.DimGray;
            this.LoadCachesButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LoadCachesButton.Location = new System.Drawing.Point(3, 190);
            this.LoadCachesButton.Name = "LoadCachesButton";
            this.LoadCachesButton.Size = new System.Drawing.Size(100, 23);
            this.LoadCachesButton.TabIndex = 1;
            this.LoadCachesButton.Text = "Load Caches";
            this.LoadCachesButton.UseVisualStyleBackColor = false;
            this.LoadCachesButton.Click += new System.EventHandler(this.LoadCachesButton_Click);
            // 
            // CachesToLoadCheckList
            // 
            this.CachesToLoadCheckList.BackColor = System.Drawing.Color.Gray;
            this.CachesToLoadCheckList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CachesToLoadCheckList.CheckOnClick = true;
            this.CachesToLoadCheckList.FormattingEnabled = true;
            this.CachesToLoadCheckList.Location = new System.Drawing.Point(3, 22);
            this.CachesToLoadCheckList.Name = "CachesToLoadCheckList";
            this.CachesToLoadCheckList.Size = new System.Drawing.Size(210, 135);
            this.CachesToLoadCheckList.TabIndex = 0;
            this.CachesToLoadCheckList.SelectedIndexChanged += new System.EventHandler(this.CachesToLoadCheckList_SelectedIndexChanged);
            // 
            // CacheSelector
            // 
            this.AcceptButton = this.LoadCachesButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(240, 240);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Enabled = false;
            this.MaximumSize = new System.Drawing.Size(256, 256);
            this.MinimumSize = new System.Drawing.Size(256, 256);
            this.Name = "CacheSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckedListBox CachesToLoadCheckList;
        public System.Windows.Forms.Button LoadCachesButton;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Button CancelButton;
        public System.Windows.Forms.TextBox NumTagsText;
    }
}