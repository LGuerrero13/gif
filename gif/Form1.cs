namespace gif
{
    public partial class gif : Form
    {

        private Thread? _thread;

        public gif()
        {
            InitializeComponent();
        }

        private string getGmodDirectory()
        {
            string[] candidatePaths = { @"Program Files\Steam", @"Program Files (x86)\Steam", @"Steam\steamapps\common\GarrysMod\garrysmod\addons" };
            foreach (var drive in DriveInfo.GetDrives())
            {
                foreach (var candPath in candidatePaths)
                {
                    if (Directory.Exists(Path.Combine(drive.ToString(), candPath)))
                    {
                        return Path.Combine(drive.ToString(), candPath);
                    }
                }
            }
            return "Garry's Mod Addon Folder Path";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtboxFilePath.Text = getGmodDirectory();
        }

        private void btnFileDialog_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtboxFilePath.Text = fbd.SelectedPath;
                }
            }
        }

        private void txtboxFilePath_Enter(object sender, EventArgs e)
        {
            if (txtboxFilePath.Text.Equals("Garry's Mod Addon Folder Path"))
            {
                txtboxFilePath.Text = "";
            }
        }

        private void findInfectedFiles()
        {
            if (lstInfectedFiles.InvokeRequired) // required to call component created in a different thread.
            {
                lstInfectedFiles.Invoke(new MethodInvoker(delegate { lstInfectedFiles.Items.Clear(); }));
            }

            progressBar.Value = 0;

            string[] allFiles;
            try
            {
                allFiles = Directory.GetFiles(txtboxFilePath.Text, "*.gma");
            }
            catch (DirectoryNotFoundException dnfEx)
            {
                MessageBox.Show($"Directory \"{txtboxFilePath.Text}\" is not a valid directory.", "Folder path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception genEx)
            {
                MessageBox.Show($"There was an error using this program\nError: {genEx.Message}.", "Folder path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            /*
                Taken from https://www.reddit.com/r/gmod/comments/vl9fie/psa_everything_known_about_june_workshop_incident/
                Thanks u/thejaviertc
            */
            string[] knowListOfInfectedAddonIDs =
            {
                "1489890477",
                "2816948514",
                "2818916773",
                "2818933771",
                "2203246561",
                "2587458626",
                "2589006389",
                "2592662319",
                "2588410453"
            };

            if (progressBar.InvokeRequired) // required to call component created in a different thread.
            {
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.Maximum = allFiles.Length; }));
            }

            foreach (string file in allFiles)
            {

                bool containsKnownInfectedFile = knowListOfInfectedAddonIDs.Any(workshopID => file.Contains(workshopID));

                /* skip parsing the file if we already know the workshopid is that of an infected one */
                if (containsKnownInfectedFile)
                {
                    if (lstInfectedFiles.InvokeRequired)
                    {
                        lstInfectedFiles.Invoke(new MethodInvoker(delegate { lstInfectedFiles.Items.Add(file); }));
                        continue;
                    }
                }

                if (lblScannedFile.InvokeRequired)
                {
                    lblScannedFile.Invoke(new MethodInvoker(delegate { lblScannedFile.Text = $"File: {file.Split(@"\").Last()}"; }));
                    progressBar.Invoke(new MethodInvoker(delegate { progressBar.Value += 1; }));
                }

                string[] lines = File.ReadAllLines(file);
                string? firstOccurance = lines.FirstOrDefault(line => line.Contains("I hope you realize you aren't safe in workshop anymore"));

                if (firstOccurance != null)
                {
                    if (lstInfectedFiles.InvokeRequired)
                    {
                        lstInfectedFiles.Invoke(new MethodInvoker(delegate { lstInfectedFiles.Items.Add(file); }));
                    }
                }
            }

            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.Value = 0; }));

                if (lstInfectedFiles.Items.Count <= 0)
                {
                    lblScannedFile.Invoke(new MethodInvoker(delegate {
                        lblScannedFile.ForeColor = Color.Green; 
                        lblScannedFile.Text = "Done, you are clean!";
                        btnScan.Enabled = true;
                    }));
                }
                else
                {
                    lblScannedFile.Invoke(new MethodInvoker(delegate {
                        lblScannedFile.ForeColor = Color.Red;
                        lblScannedFile.Text = "Infected file(s) detected! Please delete and remove from workshop.";
                        btnScan.Enabled = true;
                    }));
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtboxFilePath.Text) || !Directory.Exists(txtboxFilePath.Text))
            {
                MessageBox.Show("Please enter in a valid folder for GIF to look at", "Folder path error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblScannedFile.ForeColor = Color.Black;
            btnScan.Enabled = false;

            _thread = new Thread(new ThreadStart(findInfectedFiles));
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void lstInfectedFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lstInfectedFiles.SelectedItem != null)
            {
                string path = lstInfectedFiles.SelectedItem.ToString()!;
                string args = string.Format("/e, /select, \"{0}\"", path);
                System.Diagnostics.Process.Start("explorer.exe", args);
            }
        }
    }
}