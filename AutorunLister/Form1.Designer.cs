namespace AutorunLister
{
  partial class Form1
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Registry", System.Windows.Forms.HorizontalAlignment.Left);
      System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Start menu", System.Windows.Forms.HorizontalAlignment.Left);
      System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Scheduler", System.Windows.Forms.HorizontalAlignment.Left);
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.listViewMain = new System.Windows.Forms.ListView();
      this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnParameters = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnCompany = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnSignature = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // listViewMain
      // 
      this.listViewMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnPath,
            this.columnParameters,
            this.columnCompany,
            this.columnSignature});
      this.listViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listViewMain.FullRowSelect = true;
      listViewGroup1.Header = "Registry";
      listViewGroup1.Name = "listViewGroupRegistry";
      listViewGroup2.Header = "Start menu";
      listViewGroup2.Name = "listViewGroupStartMenu";
      listViewGroup3.Header = "Scheduler";
      listViewGroup3.Name = "listViewGroupScheduler";
      this.listViewMain.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
      this.listViewMain.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewMain.Location = new System.Drawing.Point(0, 0);
      this.listViewMain.MultiSelect = false;
      this.listViewMain.Name = "listViewMain";
      this.listViewMain.ShowItemToolTips = true;
      this.listViewMain.Size = new System.Drawing.Size(1005, 600);
      this.listViewMain.SmallImageList = this.imageListIcons;
      this.listViewMain.TabIndex = 0;
      this.listViewMain.UseCompatibleStateImageBehavior = false;
      this.listViewMain.View = System.Windows.Forms.View.Details;
      this.listViewMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewMain_KeyDown);
      this.listViewMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewMain_MouseDoubleClick);
      // 
      // columnName
      // 
      this.columnName.Text = "Name";
      this.columnName.Width = 200;
      // 
      // columnPath
      // 
      this.columnPath.Text = "Path";
      this.columnPath.Width = 320;
      // 
      // columnParameters
      // 
      this.columnParameters.Text = "Command line parameters";
      this.columnParameters.Width = 220;
      // 
      // columnCompany
      // 
      this.columnCompany.Text = "Company";
      this.columnCompany.Width = 120;
      // 
      // columnSignature
      // 
      this.columnSignature.Text = "Digital signature";
      this.columnSignature.Width = 120;
      // 
      // imageListIcons
      // 
      this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
      this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListIcons.Images.SetKeyName(0, "no.png");
      this.imageListIcons.Images.SetKeyName(1, "yes.png");
      this.imageListIcons.Images.SetKeyName(2, "alert.png");
      this.imageListIcons.Images.SetKeyName(3, "appl.png");
      // 
      // backgroundWorker1
      // 
      this.backgroundWorker1.WorkerReportsProgress = true;
      this.backgroundWorker1.WorkerSupportsCancellation = true;
      this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
      this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      // 
      // progressBar1
      // 
      this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.progressBar1.Location = new System.Drawing.Point(0, 577);
      this.progressBar1.Maximum = 5;
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(1005, 23);
      this.progressBar1.Step = 1;
      this.progressBar1.TabIndex = 1;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1005, 600);
      this.Controls.Add(this.progressBar1);
      this.Controls.Add(this.listViewMain);
      this.Name = "Form1";
      this.Text = "Autorun lister";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listViewMain;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.ColumnHeader columnName;
    private System.Windows.Forms.ColumnHeader columnPath;
    private System.Windows.Forms.ColumnHeader columnParameters;
    private System.Windows.Forms.ColumnHeader columnCompany;
    private System.Windows.Forms.ColumnHeader columnSignature;
    private System.Windows.Forms.ImageList imageListIcons;
    private System.Windows.Forms.ProgressBar progressBar1;
  }
}

