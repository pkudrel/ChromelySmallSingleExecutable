namespace ChromelySmallSingleExecutable.Features.Downloader.View
{
    partial class DownloadForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.PackageName = new System.Windows.Forms.TextBox();
            this.CefSharpPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Package name";
            // 
            // PackageName
            // 
            this.PackageName.Location = new System.Drawing.Point(12, 25);
            this.PackageName.Name = "PackageName";
            this.PackageName.ReadOnly = true;
            this.PackageName.Size = new System.Drawing.Size(709, 20);
            this.PackageName.TabIndex = 1;
            // 
            // CefSharpPath
            // 
            this.CefSharpPath.Location = new System.Drawing.Point(12, 66);
            this.CefSharpPath.Name = "CefSharpPath";
            this.CefSharpPath.ReadOnly = true;
            this.CefSharpPath.Size = new System.Drawing.Size(709, 20);
            this.CefSharpPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Package path";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 103);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(709, 23);
            this.progressBar.TabIndex = 4;
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(12, 152);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(709, 193);
            this.LogBox.TabIndex = 5;
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 357);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.CefSharpPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PackageName);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DownloadForm";
            this.Load += new System.EventHandler(this.DownloadForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PackageName;
        private System.Windows.Forms.TextBox CefSharpPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox LogBox;
    }
}