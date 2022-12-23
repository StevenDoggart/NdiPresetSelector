#pragma warning disable CS0169

namespace NdiPresetSelector
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this._logoPictureBox = new System.Windows.Forms.PictureBox();
            this._titleLabel = new System.Windows.Forms.Label();
            this._authorLabel = new System.Windows.Forms.Label();
            this._helpTextBox = new System.Windows.Forms.TextBox();
            this._versionLabel = new System.Windows.Forms.Label();
            this._linkLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _logoPictureBox
            // 
            this._logoPictureBox.Image = global::NdiPresetSelector.Properties.Resources.Logo;
            this._logoPictureBox.Location = new System.Drawing.Point(127, 12);
            this._logoPictureBox.Name = "_logoPictureBox";
            this._logoPictureBox.Size = new System.Drawing.Size(128, 128);
            this._logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._logoPictureBox.TabIndex = 0;
            this._logoPictureBox.TabStop = false;
            // 
            // _titleLabel
            // 
            this._titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._titleLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._titleLabel.Location = new System.Drawing.Point(12, 143);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(350, 31);
            this._titleLabel.TabIndex = 1;
            this._titleLabel.Text = "NDI Preset Selector";
            this._titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _authorLabel
            // 
            this._authorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._authorLabel.Location = new System.Drawing.Point(12, 197);
            this._authorLabel.Name = "_authorLabel";
            this._authorLabel.Size = new System.Drawing.Size(350, 23);
            this._authorLabel.TabIndex = 2;
            this._authorLabel.Text = "Steven Doggart";
            this._authorLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _helpTextBox
            // 
            this._helpTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._helpTextBox.BackColor = System.Drawing.SystemColors.Info;
            this._helpTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this._helpTextBox.Location = new System.Drawing.Point(12, 246);
            this._helpTextBox.Multiline = true;
            this._helpTextBox.Name = "_helpTextBox";
            this._helpTextBox.ReadOnly = true;
            this._helpTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._helpTextBox.Size = new System.Drawing.Size(350, 284);
            this._helpTextBox.TabIndex = 3;
            this._helpTextBox.Text = resources.GetString("_helpTextBox.Text");
            this._helpTextBox.Enter += new System.EventHandler(this._helpTextBox_Enter);
            // 
            // _versionLabel
            // 
            this._versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._versionLabel.Location = new System.Drawing.Point(12, 174);
            this._versionLabel.Name = "_versionLabel";
            this._versionLabel.Size = new System.Drawing.Size(350, 23);
            this._versionLabel.TabIndex = 4;
            this._versionLabel.Text = "1.1";
            this._versionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _linkLabel
            // 
            this._linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._linkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this._linkLabel.ForeColor = System.Drawing.Color.Blue;
            this._linkLabel.Location = new System.Drawing.Point(12, 220);
            this._linkLabel.Name = "_linkLabel";
            this._linkLabel.Size = new System.Drawing.Size(350, 23);
            this._linkLabel.TabIndex = 5;
            this._linkLabel.Text = "https://github.com/StevenDoggart/NdiPresetSelector";
            this._linkLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this._linkLabel.Click += new System.EventHandler(this._linkLabel_Click);
            this._linkLabel.MouseLeave += new System.EventHandler(this._linkLabel_MouseLeave);
            this._linkLabel.MouseHover += new System.EventHandler(this._linkLabel_MouseHover);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 542);
            this.Controls.Add(this._linkLabel);
            this.Controls.Add(this._versionLabel);
            this.Controls.Add(this._helpTextBox);
            this.Controls.Add(this._authorLabel);
            this.Controls.Add(this._titleLabel);
            this.Controls.Add(this._logoPictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(390, 10000);
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox _logoPictureBox;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label _authorLabel;
        private System.Windows.Forms.TextBox _helpTextBox;
        private System.Windows.Forms.Label _versionLabel;
        private System.Windows.Forms.Label _linkLabel;
    }
}