using System;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

using AutorunLibrary;

namespace AutorunLister
{
  public partial class Form1 : Form
  {
    private const string MAIN_TITLE = "Autorun lister";

    private readonly AutorunFileFinder mAutorunFileFinder;

    public Form1()
    {
      InitializeComponent();

      mAutorunFileFinder = new AutorunFileFinder(imageListIcons);
    }

    private void StartSearching()
    {
      Text = MAIN_TITLE + " (searching...)";
      progressBar1.Visible = true;
      while (imageListIcons.Images.Count > 4)
        imageListIcons.Images.RemoveAt(4);
      backgroundWorker1.RunWorkerAsync();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      StartSearching();
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;

      for (int i = 1; i <= 5; i++)
      {
        if (worker.CancellationPending)
        {
          e.Cancel = true;
          break;
        }
        else
        {
          mAutorunFileFinder.Search(i, i == 1);
          worker.ReportProgress(i);
        }
      }

    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      listViewMain.Items.Clear();
      if (e.Cancelled)
      {
        progressBar1.Text = "Search canceled";
      }
      else if (e.Error != null)
      {
        progressBar1.Text = "Error: " + e.Error.Message;
      }
      else
      {
        listViewMain.BeginUpdate();
        foreach (var i in mAutorunFileFinder.AutorunFiles)
        {
          ListViewGroup lvg = listViewMain.Groups[0];
          if (i.StorageType == AutorunFileItem.AutorunStorageType.StartMenu)
            lvg = listViewMain.Groups[1];
          else if (i.StorageType == AutorunFileItem.AutorunStorageType.Scheduler)
            lvg = listViewMain.Groups[2];
          ListViewItem lvi = new ListViewItem(new string[] { i.FileName, i.FilePath, i.Parameters, i.Company, i.DigitalSignatureVerificationResult }, i.IconIndex, lvg)
          {
            Tag = i,
            ToolTipText = i.FullDescription
          };
          if (!i.IsActive)
            lvi.ForeColor = Color.LightGray;
          if (i.DigitalSignature == 1)
            lvi.BackColor = Color.PaleGreen;
          else if (i.DigitalSignature == 2)
            lvi.BackColor = Color.LightPink;

          listViewMain.Items.Add(lvi);
        }
        listViewMain.EndUpdate();
        progressBar1.Visible = false;
      }
      Text = MAIN_TITLE;
    }

    private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      progressBar1.PerformStep();
    }

    private void listViewMain_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      ListView lv = sender as ListView;
      ListViewHitTestInfo info = lv.HitTest(e.X, e.Y);
      ListViewItem item = info.Item;

      if (item != null)
      {
        var ao = item.Tag as AutorunFileItem;
        string fp = ao.FilePath;
        if (fp.EndsWith("rundll32.exe", StringComparison.OrdinalIgnoreCase))
        {
          string args = ao.Parameters;
          if (!string.IsNullOrWhiteSpace(args))
          {
            int i = args.IndexOf(',');
            if (i < 0)
              i = args.Length;
            fp = args.Substring(0, i).Trim();
            if ((fp.Length > 0) && (fp[0] == '"'))
              fp = fp.Substring(1, fp.Length - 1);
            if ((fp.Length > 0) && (fp[fp.Length - 1] == '"'))
              fp = fp.Substring(0, fp.Length - 1);
            fp = fp.Trim();
            if (fp.StartsWith("/d ", StringComparison.OrdinalIgnoreCase))
            {
              fp = fp.Substring(3).Trim();
              string d = Path.GetDirectoryName(fp);
              if (string.IsNullOrEmpty(d))
                d = Path.GetDirectoryName(ao.FilePath);
              fp = Path.Combine(d, fp);
            }
          }
        }
        string argument = "/select, \"" + Environment.ExpandEnvironmentVariables(fp) + "\"";
        System.Diagnostics.Process.Start("explorer.exe", argument);
      }

    }

    private void listViewMain_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F5)
      {
        StartSearching();
        e.Handled = true;
      }
    }
  }
}
