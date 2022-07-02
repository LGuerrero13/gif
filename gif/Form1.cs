namespace gif
{
    public partial class gif : Form
    {

        Thread? thread;
        HttpResponseMessage? message;
        string[]? knownListOfInfectedAddonIDs;

        readonly string[] backupListOfIDs =
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
        static readonly HttpClient _client = new HttpClient();
        readonly string _infectedAddonListURL = "https://raw.githubusercontent.com/LGuerrero13/gif/main/infectedAddonIDs.txt";

        public gif()
        {
            InitializeComponent();
        }

        private string getGmodDirectory()
        {
            string[] candidatePaths = 
            {
                @"Program Files\Steam",
                @"Program Files (x86)\Steam",
                @"Steam\steamapps\common\GarrysMod\garrysmod\addons",
                @"Steam\steamapps\workshop\content\4000",
                @"SteamLibrary\steamapps\workshop\content\4000"
            };

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
            bwrLoadAddonList.RunWorkerAsync();
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
            catch (DirectoryNotFoundException)
            {
                displayError($"Directory \"{txtboxFilePath.Text}\" is not a valid directory", "Folder directory/path error");
                return;
            }
            catch (IOException ioEx)
            {
                displayError($"There was an error trying get the files from the directory\nError: {ioEx.Message}", "File retrieval/access error");
                return;
            }
            catch (Exception genEx)
            {
                displayError($"There was an error using this program\nError: {genEx.Message}", "General program error");
                return;
            }

            /*
                Taken from https://www.reddit.com/r/gmod/comments/vl9fie/psa_everything_known_about_june_workshop_incident/
                Thanks u/thejaviertc
            */

            if (progressBar.InvokeRequired) // required to call component created in a different thread.
            {
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.Maximum = allFiles.Length; }));
            }

            foreach (string file in allFiles)
            {

                bool containsKnownInfectedFile = knownListOfInfectedAddonIDs.Any(workshopID => file.Contains(workshopID));

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

                try
                {
                    IEnumerable<string> lines = File.ReadLines(file);
                    foreach (string line in lines)
                    {
                        if (line.Contains("I hope you realize you aren't safe in workshop anymore"))
                        {
                            lstInfectedFiles.Invoke(new MethodInvoker(delegate { lstInfectedFiles.Items.Add(file); }));
                            break;
                        }
                    }
                }
                catch (IOException ioEx)
                {
                    displayError($"There was an error trying read the .gma files\nError: {ioEx.Message}", "File read error");
                    return;
                }
                catch (Exception genEx)
                {
                    displayError($"There was an error trying to use the program\nError: {genEx.Message}", "General program error");
                    return;
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

            thread = new Thread(new ThreadStart(findInfectedFiles));
            thread.IsBackground = true;
            thread.Start();
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

        private void bwrLoadAddonList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            updateAddonList();
        }

        private void btnDownloadIDs_Click(object sender, EventArgs e)
        {
            updateAddonList();
        }

        private void updateAddonList()
        {
            try
            {
                lblScannedFile.Invoke(new MethodInvoker(delegate 
                {
                    lblScannedFile.Text = "Attempting download of latest infected IDs";
                }));

                // Create a HttpResponse<essage whhich will get the raw .txt file contents
                message = _client.GetAsync(_infectedAddonListURL).Result;

                // Ensure that the request goes through with Status Code 200, else throw an exception
                message.EnsureSuccessStatusCode();

                // Get the retrieved contents and convert it into a string[] array through splitting the string
                // The result is all the ID's separated by a new line
                knownListOfInfectedAddonIDs = message.Content.ReadAsStringAsync().Result.Split('\n');

                lblScannedFile.Invoke(new MethodInvoker(delegate
                { 
                    displaySuccess("Download successful!");
                }));
            }
            catch (HttpRequestException httpEx)
            {
                displayError($"There was an error trying to get the resource: {_infectedAddonListURL}\nError: {httpEx.Message}", "HTTP request error");
                knownListOfInfectedAddonIDs = backupListOfIDs;
                return;
            }
            catch (Exception genEx)
            {
                displayError($"There was an error trying to use the program\nError: {genEx.Message}", "General program error");
                knownListOfInfectedAddonIDs = backupListOfIDs;
                return;
            }
        }

        private void displayError(string error, string title)
        {
            MessageBox.Show(error, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblScannedFile.Invoke(new MethodInvoker(delegate
            {
                lblScannedFile.ForeColor = Color.Red;
                lblScannedFile.Text = "Error occurred, try running the operation again";
            }));
        }

        private void displaySuccess(string success)
        {
            lblScannedFile.ForeColor = Color.Green;
            lblScannedFile.Text = success;
        }
    }
}