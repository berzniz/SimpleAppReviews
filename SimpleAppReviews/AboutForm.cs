﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleAppReviews
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void websiteLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            websiteLabel.LinkVisited = true;
            System.Diagnostics.Process.Start(websiteLabel.Text);
        }

    }
}
