namespace gif
{
    partial class gif
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gif));
            this.lblAddonsPath = new System.Windows.Forms.Label();
            this.txtboxFilePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnFileDialog = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblScannedFile = new System.Windows.Forms.Label();
            this.lstInfectedFiles = new System.Windows.Forms.ListBox();
            this.btnDownloadIDs = new System.Windows.Forms.Button();
            this.bwrLoadAddonList = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // lblAddonsPath
            // 
            this.lblAddonsPath.AutoSize = true;
            this.lblAddonsPath.Location = new System.Drawing.Point(11, 9);
            this.lblAddonsPath.Name = "lblAddonsPath";
            this.lblAddonsPath.Size = new System.Drawing.Size(75, 15);
            this.lblAddonsPath.TabIndex = 0;
            this.lblAddonsPath.Text = "Addons path";
            // 
            // txtboxFilePath
            // 
            this.txtboxFilePath.Location = new System.Drawing.Point(12, 27);
            this.txtboxFilePath.Name = "txtboxFilePath";
            this.txtboxFilePath.Size = new System.Drawing.Size(430, 23);
            this.txtboxFilePath.TabIndex = 3;
            this.txtboxFilePath.Text = "Garry\'s Mod Addon Folder Path";
            this.txtboxFilePath.Enter += new System.EventHandler(this.txtboxFilePath_Enter);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnFileDialog
            // 
            this.btnFileDialog.Location = new System.Drawing.Point(448, 27);
            this.btnFileDialog.Name = "btnFileDialog";
            this.btnFileDialog.Size = new System.Drawing.Size(75, 23);
            this.btnFileDialog.TabIndex = 2;
            this.btnFileDialog.Text = "Browse";
            this.btnFileDialog.UseVisualStyleBackColor = true;
            this.btnFileDialog.Click += new System.EventHandler(this.btnFileDialog_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(11, 336);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(102, 23);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(229, 336);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(294, 23);
            this.progressBar.TabIndex = 4;
            // 
            // lblScannedFile
            // 
            this.lblScannedFile.AutoSize = true;
            this.lblScannedFile.Location = new System.Drawing.Point(12, 367);
            this.lblScannedFile.Name = "lblScannedFile";
            this.lblScannedFile.Size = new System.Drawing.Size(53, 15);
            this.lblScannedFile.TabIndex = 5;
            this.lblScannedFile.Text = "File: N/A";
            // 
            // lstInfectedFiles
            // 
            this.lstInfectedFiles.FormattingEnabled = true;
            this.lstInfectedFiles.HorizontalScrollbar = true;
            this.lstInfectedFiles.ItemHeight = 15;
            this.lstInfectedFiles.Location = new System.Drawing.Point(12, 56);
            this.lstInfectedFiles.Name = "lstInfectedFiles";
            this.lstInfectedFiles.Size = new System.Drawing.Size(511, 274);
            this.lstInfectedFiles.TabIndex = 4;
            this.lstInfectedFiles.DoubleClick += new System.EventHandler(this.lstInfectedFiles_DoubleClick);
            // 
            // btnDownloadIDs
            // 
            this.btnDownloadIDs.Location = new System.Drawing.Point(120, 336);
            this.btnDownloadIDs.Name = "btnDownloadIDs";
            this.btnDownloadIDs.Size = new System.Drawing.Size(102, 23);
            this.btnDownloadIDs.TabIndex = 6;
            this.btnDownloadIDs.Text = "Download IDs";
            this.btnDownloadIDs.UseVisualStyleBackColor = true;
            this.btnDownloadIDs.Click += new System.EventHandler(this.btnDownloadIDs_Click);
            // 
            // bwrLoadAddonList
            // 
            this.bwrLoadAddonList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrLoadAddonList_DoWork);
            // 
            // gif
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 392);
            this.Controls.Add(this.btnDownloadIDs);
            this.Controls.Add(this.lstInfectedFiles);
            this.Controls.Add(this.lblScannedFile);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnFileDialog);
            this.Controls.Add(this.txtboxFilePath);
            this.Controls.Add(this.lblAddonsPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "gif";
            this.Text = "Goatse Infection Finder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblAddonsPath;
        private TextBox txtboxFilePath;
        private OpenFileDialog openFileDialog1;
        private Button btnFileDialog;
        private Button btnScan;
        private ProgressBar progressBar;
        private Label lblScannedFile;
        private ListBox lstInfectedFiles;
        private Button btnDownloadIDs;
        private System.ComponentModel.BackgroundWorker bwrLoadAddonList;
    }
}