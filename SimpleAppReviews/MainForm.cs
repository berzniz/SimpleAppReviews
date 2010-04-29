using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace SimpleAppReviews
{
    public partial class MainForm : Form
    {
        delegate void ApplyChangesToTableCallback(ReviewEntry[] reviews);
        delegate void SetStatusCallback(string status);
        delegate void SetProgressCallback(int value);
        delegate void SetUpdateButtonTextCallback(string text);
        private bool shouldStop = false;
        private Thread worker = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void SetStatus(string status)
        {
            statusLabel.Text = status;
        }

        private void SetProgress(int value)
        {
            fetchProgressBar.Value = value;
        }

        private void SetUpdateButtonText(string text)
        {
            UpdateButton.Text = text;
        }

        private void ApplyChangesToTable(ReviewEntry[] reviews)
        {
            double sum = 0;
            double avg = 0;
            double count = reviews.Count();

            foreach (ReviewEntry review in reviews)
            {
                object[] tempData = new object[8];
                tempData[0] = AppStoreManager.GetImageFromID(review.storeId);
                tempData[1] = AppStoreManager.GetCountryFromID(review.storeId);
                tempData[2] = review.reviewVersion;
                tempData[3] = review.reviewDate;
                tempData[4] = review.reviewUser;
                tempData[5] = review.reviewStars;
                tempData[6] = review.reviewTitle;
                tempData[7] = review.reviewText;
                tableView.Rows.Add(tempData);

                sum += Convert.ToDouble(review.reviewStars);
            }

            if (count > 0)
            {
                avg = sum / count;
                object[] tempData = new object[8];
                tempData[0] = AppStoreManager.GetImageFromID(reviews[0].storeId);
                tempData[1] = AppStoreManager.GetCountryFromID(reviews[0].storeId);
                tempData[2] = count + " User(s)";
                tempData[3] = String.Format("{0:0.##}", avg);
                CountryTableView.Rows.Add(tempData);
            }

        }

        private void GetRatings()
        {
            ReviewFetcher r = new ReviewFetcher();
            LinkedList<ReviewEntry> reviews = new LinkedList<ReviewEntry>();
            
            shouldStop = false;
            this.Invoke(new SetProgressCallback(SetProgress), new object[] { 0 });
            double cur = 0;
            double count = AppStoreManager.count();
            foreach (string storeID in AppStoreManager.getStoreIDs())
            {
                cur++;
                this.Invoke(new SetProgressCallback(SetProgress), new object[] { (int)(cur / count * 100) });
                this.Invoke(new SetStatusCallback(SetStatus), new object[] { AppStoreManager.GetCountryFromID(storeID) });
                LinkedList<ReviewEntry> tempReviews = r.fetch(storeID, appIdTextBox.Text);
                this.Invoke(new ApplyChangesToTableCallback(ApplyChangesToTable), new object[] { tempReviews.ToArray() });

                if (shouldStop)
                {
                    this.Invoke(new SetProgressCallback(SetProgress), new object[] { 100 });
                    break;
                }                
            }

            this.Invoke(new SetStatusCallback(SetStatus), new object[] { "Done" });
            this.Invoke(new SetUpdateButtonTextCallback(SetUpdateButtonText), new object[] { "Update" });
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (UpdateButton.Text.Equals("Update"))
            {
                appIdTextBox.BackColor = System.Drawing.SystemColors.Window;
                appIdTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
                if (appIdTextBox.Text.Length == 0)
                {
                    appIdTextBox.Focus();
                    appIdTextBox.BackColor = Color.Red;
                    appIdTextBox.ForeColor = Color.White;
                    return;
                }
                tableView.Rows.Clear();
                CountryTableView.Rows.Clear();
                ThreadStart start = new ThreadStart(GetRatings);
                worker = new Thread(start);
                worker.Start();
                UpdateButton.Text = "Stop";
            }
            else if (UpdateButton.Text.Equals("Stop"))
            {
                shouldStop = true;
                UpdateButton.Text = "Stopping";
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            shouldStop = true;
            if (worker != null && worker.IsAlive)
                worker.Abort();            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tableView.RowCount == 0)
            {
                MessageBox.Show("No data to export.", "No Data");
                return;
            }

            if (worker != null && worker.IsAlive)
            {
                MessageBox.Show("Can not export while updating.", "Busy");
                return;
            }

            try
            {
                string fileName = Path.GetTempFileName() + ".csv";
                StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);

                int iColCount = tableView.ColumnCount;
                for (int i = 1; i < iColCount; i++)
                {
                    sw.Write(tableView.Columns[i].Name);
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);

                foreach (DataGridViewRow dr in tableView.Rows)
                {
                    for (int i = 1; i < iColCount; i++)
                    {
                        if (dr.Cells[i].Value != null)
                        {
                            sw.Write(dr.Cells[i].Value.ToString().Replace(",", ""));
                        }
                        sw.Write((i == iColCount - 1) ? sw.NewLine : ",");
                    }
                }
                sw.Close();
                System.Diagnostics.Process.Start(fileName);
            }
            catch (Exception ex)
            {
                ex.ToString();
                MessageBox.Show("Something went wrong during export. Sorry!", "Oops...");
            }
        }
    }
}
