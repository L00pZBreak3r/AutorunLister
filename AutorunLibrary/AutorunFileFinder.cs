using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using IWshRuntimeLibrary;

using AutorunLibrary.Helpers.TaskScheduler;

namespace AutorunLibrary
{
  public class AutorunFileFinder
  {
    private const string RUN_KEY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string RUN_KEY_PATH_WOW64 = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";

    public readonly List<AutorunFileItem> AutorunFiles = new List<AutorunFileItem>();

    private readonly ImageList mFileIcons;

    public AutorunFileFinder(ImageList fileIcons)
    {
      mFileIcons = fileIcons;
    }

    private static string[] ParseCommandLine(string s)
    {
      string[] res = null;
      if (!string.IsNullOrWhiteSpace(s))
      {
        res = new string[2];
        res[1] = "";
        s = s.Trim();
        if (s[0] == '"')
        {
          int k = s.IndexOf('"', 1);
          if (k < 0)
            k = s.Length;
          res[0] = s.Substring(1, k - 1);
          if (k < s.Length)
            s = s.Substring(k + 1);
          else
            s = "";
        }
        int i = s.IndexOf(' ');
        if (i >= 0)
        {
          if (res[0] == null)
          {
            int iext = s.IndexOf(".exe", StringComparison.OrdinalIgnoreCase);
            if (iext >= 0)
              i = iext + 4;
          }
          if (i < s.Length)
            res[1] = s.Substring(i).TrimStart();
        }
        else
          i = s.Length;
        if (res[0] == null)
          res[0] = s.Substring(0, i);
      }
      return res;
    }

    private void AddAutorunFileItem(AutorunFileItem.AutorunUserType userType, AutorunFileItem.AutorunStorageType storageType, string[] cp, string description, bool isActive = true)
    {
      var i = new AutorunFileItem(userType, storageType, cp[0], cp[1], description, isActive);
      if (mFileIcons != null)
      {
        if (i.FileIconImage != null)
        {
          mFileIcons.Images.Add(i.FileIconImage);
          i.IconIndex = mFileIcons.Images.Count - 1;
        }
        else
          i.IconIndex = 3;
      }
      AutorunFiles.Add(i);
    }

    private int SearchStartMenu(AutorunFileItem.AutorunUserType st)
    {
      int res = 0;
      string path = Environment.GetFolderPath((st == AutorunFileItem.AutorunUserType.Common) ? Environment.SpecialFolder.CommonStartup : Environment.SpecialFolder.Startup);
      if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
      {
        var di = new DirectoryInfo(path);
        foreach (var fi in di.GetFiles())
          if (!".ini".Equals(fi.Extension, StringComparison.OrdinalIgnoreCase))
          {
            string fileName = fi.Name;
            string[] cp = new string[2];
            cp[0] = fi.FullName;
            cp[1] = "";
            if (".lnk".Equals(fi.Extension, StringComparison.OrdinalIgnoreCase))
            {
              WshShell shell = new WshShell();
              IWshShortcut link = (IWshShortcut)shell.CreateShortcut(cp[0]);
              cp[0] = link.TargetPath;
              cp[1] = link.Arguments;
            }
            AddAutorunFileItem(st, AutorunFileItem.AutorunStorageType.StartMenu, cp, Path.GetFileNameWithoutExtension(fileName));
            res++;
          }
      }

      return res;
    }

    private int SearchRegistry(AutorunFileItem.AutorunUserType st)
    {
      int res = 0;
      RegistryKey rk = (st == AutorunFileItem.AutorunUserType.Common) ? Registry.LocalMachine : Registry.CurrentUser;
      foreach (string rp in new string[] { RUN_KEY_PATH, RUN_KEY_PATH_WOW64 })
        using (RegistryKey startupKey = rk.OpenSubKey(rp))
          if (startupKey != null)
          {
            foreach (string valueName in startupKey.GetValueNames())
              if (!string.IsNullOrEmpty(valueName))
              {
                var cp = ParseCommandLine(startupKey.GetValue(valueName).ToString());
                if (cp != null)
                {
                  AddAutorunFileItem(st, AutorunFileItem.AutorunStorageType.Registry, cp, valueName);
                  res++;
                }
              }
          }
      return res;
    }

    private int SearchScheduler()
    {
      int res = 0;
      // Get the service on the local machine
      using (TaskService ts = new TaskService())
      {
        foreach (var t in ts.AllTasks)
        {
          var td = t.Definition;
          string[] cp = new string[2];

          foreach (var ta in td.Actions)
          {
            switch (ta.ActionType)
            {
              case TaskActionType.Execute:
                var ae = ta as ExecAction;
                cp[0] = ae.Path;
                cp[1] = ae.Arguments;
                if (string.IsNullOrEmpty(cp[1]))
                {
                  var cp2 = ParseCommandLine(cp[0]);
                  if ((cp2 != null) && !string.IsNullOrEmpty(cp2[1]))
                    cp = cp2;
                }
                break;
              /*case TaskActionType.ComHandler:
                var ac = ta as ComHandlerAction;
                cp[0] = "ComHandler";
                cp[1] = "ClassName: " + ac.ClassName + "; data: " + ac.Data;
                break;*/
              case TaskActionType.SendEmail:
                var am = ta as EmailAction;
                cp[0] = "SendEmail";
                cp[1] = "To: " + am.To + "; from: " + am.From + "; subject: " + am.Subject;
                break;
              case TaskActionType.ShowMessage:
                var ab = ta as ShowMessageAction;
                cp[0] = "ShowMessage";
                cp[1] = "Title: " + ab.Title + "; message: " + ab.MessageBody;
                break;
            }
            break;
          }

          if (!string.IsNullOrEmpty(cp[0]))
            foreach (var tg in td.Triggers)
              if (tg.Enabled)
              {
                if (tg.TriggerType == TaskTriggerType.Boot)
                {
                  AddAutorunFileItem(AutorunFileItem.AutorunUserType.Common, AutorunFileItem.AutorunStorageType.Scheduler, cp, t.Name, t.IsActive);
                  res++;
                }
                else if (tg.TriggerType == TaskTriggerType.Logon)
                {
                  AddAutorunFileItem(AutorunFileItem.AutorunUserType.User, AutorunFileItem.AutorunStorageType.Scheduler, cp, t.Name, t.IsActive);
                  res++;
                }
              }
        }
      }
      return res;
    }



    public int Search(int stage = 0, bool clearList = false)
    {
      if (clearList)
        AutorunFiles.Clear();
      int res = 0;
      if ((stage <= 0) || (stage == 1))
        res += SearchRegistry(AutorunFileItem.AutorunUserType.Common);
      if ((stage <= 0) || (stage == 2))
        res += SearchRegistry(AutorunFileItem.AutorunUserType.User);
      if ((stage <= 0) || (stage == 3))
        res += SearchStartMenu(AutorunFileItem.AutorunUserType.Common);
      if ((stage <= 0) || (stage == 4))
        res += SearchStartMenu(AutorunFileItem.AutorunUserType.User);
      if ((stage <= 0) || (stage == 5))
        res += SearchScheduler();

      return res;
    }
  }
}
