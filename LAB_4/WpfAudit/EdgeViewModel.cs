using Microsoft.Win32;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using WpfAudit.EdgePolicies;

namespace WpfAudit
{
    public class EdgeViewModel : INotifyPropertyChanged
    {
        string path = "";

        Dictionary<string, string> initialSettings;
        RegistryKey key;
        string dbName = "AUDIT";
        string documentName = "Audits";
        MongoCRUD db;
        MatchCollection matches;

          public EdgeViewModel()
          {
               Audits = new ObservableCollection<AuditModel> { };
               db = new MongoCRUD(dbName);

               initialSettings = new Dictionary<string, string>();
               key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Edge", true);

               foreach (var item in key.GetValueNames())
               {
                    initialSettings.Add(item, key.GetValue(item).ToString());
               }
               key.Close();
          }

          private AuditModel selectedAudit;
        public ObservableCollection<AuditModel> Audits { get; set; }

        private bool _checkPolicies;
        public bool CheckPolicies
        {
            get { return _checkPolicies; }
            set
            {
                _checkPolicies = value;
                OnPropertyChanged("CheckPolicies");

                foreach (var item in selectedAudit.CustomItems)
                {
                    item.PolicyBox = _checkPolicies;
                }
            }
        }

          private bool _checkFailedPolicies;
          public bool CheckFailedPolicies
          {
               get { return _checkFailedPolicies; }
               set
               {
                    _checkFailedPolicies = value;
                    OnPropertyChanged("CheckFailedPolicies");

                    foreach (var item in selectedAudit.CustomItems)
                    {
                         if(item.VisibleReason)
                              item.PolicyBox = _checkFailedPolicies;
                    }
               }
          }

          private string _saveName;
        public string SaveName
        {
            get { return _saveName; }
            set
            {
                _saveName = value;
                OnPropertyChanged("SaveName");

            }
        }

        private string _search;
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");

            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      OpenFileDialog fileDialog = new OpenFileDialog();
                      fileDialog.ShowDialog();
                      string path = fileDialog.FileName;

                      string fileText = "";
                      try
                      {

                          using (StreamReader sr = new StreamReader(path))
                          {
                              fileText = sr.ReadToEnd();

                          }
                      }

                      catch (Exception ex)
                      {

                      }

                      string displayName = new Regex(@"(?<=<display_name>)(.*)(?=</display_name>)").Matches(fileText)[0].ToString();

                      MatchCollection customs = new Regex(@"(?<=<custom_item>)(.*?)(?=</custom_item>)", RegexOptions.Singleline).Matches(fileText);

                      int i = 0, j = 0;

                      int len = customs.Count;
                      CustomItem[] customItems = new CustomItem[len];
                     

                      for (i = 0; i < len; i++)
                      {
                          customItems[i] = new CustomItem();
                      }

                      for (i = 0; i < len; i++)
                      {
                          string toMatch = customs.ElementAt(j++).ToString();
                          for (int k = 0; k < Regexes.customTypes.Length; k++)
                          {
                              matches = Regexes.customTypes[k].Matches(toMatch);

                              if (matches.Count > 0)
                                  customItems[i][k] = matches[0].Value;

                          }

                      }

                      AuditModel auditModel = new AuditModel { CustomItems = customItems, DisplayName = displayName };

                      Audits.Insert(0, auditModel);

                      selectedAudit = auditModel;

                  }));
            }
        }

          private RelayCommand enforceCommand;
          public RelayCommand EnforceCommand
          {
               get
               {
                    return enforceCommand ??
                      (enforceCommand = new RelayCommand(obj =>
                      {
                           key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Edge", true);

                           foreach (var item in selectedAudit.CustomItems)
                           {
                                RegistryValueKind regValKind = item.PolicyValueType.Contains("DWORD") ? RegistryValueKind.DWord : RegistryValueKind.String;
                                
                                if(item.VisibleReason)
                                {
                                     key.SetValue(item.PolicyRegItem, item.PolicyValueData, regValKind);
                                }
                           }

                           MessageBox.Show("The policy has been enforced!");
                           key.Close();
                      }));
               }
          }

          private RelayCommand backupCommand;
          public RelayCommand BackupCommand
          {
               get
               {
                    return backupCommand ??
                      (backupCommand = new RelayCommand(obj =>
                      {
                           ClearReason();
                           key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Edge", true);

                           foreach (var item in initialSettings)
                           {
                                key.SetValue(item.Key, item.Value, key.GetValueKind(item.Key));
                           }

                           MessageBox.Show("The registry has been backed up!");
                           key.Close();
                      }));
               }
          }

          private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      AuditModel auditModel = obj as AuditModel;
                      if (auditModel != null)
                      {
                          Audits.Remove(auditModel);
                      }
                  },
                 (obj) => Audits.Count > 0));
            }
        }

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return searchCommand ??
                  (searchCommand = new RelayCommand(obj =>
                  {
                      for (int i = 0; i < selectedAudit.CustomItems.Length; i++)
                      {
                          selectedAudit.CustomItems[i].PolicyBox = false;
                          for (int k = 0; k < 11; k++)
                          {
                              matches = new Regex(@"(.*)" + Regex.Escape(Search) + @"(.*)").Matches(selectedAudit.CustomItems[i][k]);
                              if (matches.Count > 0)
                              {
                                  selectedAudit.CustomItems[i].PolicyBox = true;
                                  break;
                              }
                          }
                      }
                  }));

            }
        }

        private RelayCommand scanCommand;
        public RelayCommand ScanCommand
        {
            get
            {
                return scanCommand ??
                  (scanCommand = new RelayCommand(obj =>
                  {
                      ClearReason();
                      string message = "Scan passed!";
                      bool isPassed = true;

                       key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Edge", true);

                       if (key != null)
                      {
                          var subKeys = key.GetSubKeyNames();
                          var regValues = key.GetValueNames();

                          foreach (var item in subKeys)
                          {
                              var tempKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Edge\" + item);

                              foreach (var val in tempKey.GetValueNames())
                              {
                                  if(selectedAudit.CustomItems.Where(x => x.PolicyRegItem == val).Single().PolicyBox)
                                  {
                                      var temp = selectedAudit.CustomItems.Where(x => x.PolicyRegItem == val).Single();
                                      temp.PolicyInfo = "ska";

                                      if (!(temp.PolicyValueData == tempKey.GetValue(val).ToString()))
                                      {
                                          isPassed = false;
                                          

                                      }
                                  }

                              }

                          }
                    
                          
                              foreach (var item in regValues)
                              {
                                  if (selectedAudit.CustomItems.Where(x => x.PolicyRegItem == item).Single().PolicyBox)
                                  {
                                      var temp = selectedAudit.CustomItems.Where(x => x.PolicyRegItem == item).Single();

                                      if (!(temp.PolicyValueData == key.GetValue(item).ToString()))
                                      {
                                          isPassed = false;

                                      temp.VisibleReason = true;
                                          temp.Reason = $"The value in the Audit is {temp.PolicyValueData} while the value in your Registry is {key.GetValue(item).ToString()}";
                                      }
                                  }
                              }
                          
                          

                          if(!isPassed)
                          {
                              message = "Scan not passed! Check out the reasons.";
                          }

                          key.Close();
                      }


                      MessageBox.Show(message);
                  }));
            }
        }

        public void ClearReason()
        {
            foreach (var item in selectedAudit.CustomItems)
            {
                item.Reason = "";
                item.VisibleReason = false;
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      OpenFileDialog dialog = new OpenFileDialog();
                      dialog.ValidateNames = false;
                      dialog.CheckFileExists = false;
                      dialog.CheckPathExists = true;
                      dialog.FileName = "Folder Selection.";
                      dialog.ShowDialog();
                      MessageBox.Show("Converted file saved!");

                      string convertedFilePath = @dialog.FileName.Replace("Folder Selection", SaveName + ".json");

                      AuditModel toSave = new AuditModel();
                      
                      List<CustomItem> temp = new List<CustomItem> { };

                      foreach (var item in selectedAudit.CustomItems)
                      {
                          if (item.PolicyBox)
                              temp.Add(item);
                      }

                      toSave.CustomItems = temp.ToArray();

                      //db.InsertRecord(documentName, toSave);

                      try
                      {

                          using (StreamWriter sr = new StreamWriter(convertedFilePath))
                          {
                              sr.WriteLine(toSave.ToJson());
                          }
                      }

                      catch (Exception ex)
                      {

                      }
                  }));
            }
        }

        public AuditModel SelectedAudit
        {
            get
            {
                return selectedAudit;
            }
            set
            {
                ClearReason();
                selectedAudit = value;
                CheckPolicies = false;
                SaveName = "";
                Search = "";
                OnPropertyChanged("SelectedAudit");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
