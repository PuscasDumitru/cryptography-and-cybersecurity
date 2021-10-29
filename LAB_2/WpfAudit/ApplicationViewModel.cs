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
using WpfAudit.Classes;

namespace WpfAudit
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
         string path = "";
      
         string dbName = "AUDIT";
         string documentName = "Audits";
         MongoCRUD db;
         MatchCollection matches;
        
        
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

                      GeneralInfo genInfo = new GeneralInfo();

                      int i = 0;

                      genInfo.Revision = Regexes.generalInfo[i++].Matches(fileText)[0].Value;
                      genInfo.Date = Regexes.generalInfo[i++].Matches(fileText)[0].Value;
                      genInfo.Description = Regexes.generalInfo[i++].Matches(fileText)[0].Value;
                      genInfo.DisplayName = Regexes.generalInfo[i++].Matches(fileText)[0].Value;
                      genInfo.CheckTypeOS = Regexes.generalInfo[i++].Matches(fileText)[0].Value;
                      genInfo.CheckTypeVers = Regexes.generalInfo[i++].Matches(fileText)[0].Value;
                      genInfo.GroupPolicy = Regexes.generalInfo[i++].Matches(fileText)[0].Value;


                      int len = Regexes.customTypes[2].Matches(fileText).Count;
                      CustomItem[] customItems = new CustomItem[len];


                      for (i = 0; i < len; i++)
                      {
                          customItems[i] = new CustomItem();
                      }

                      for (i = 0; i < Regexes.customTypes.Length; i++)
                      {
                          matches = Regexes.customTypes[i].Matches(fileText);

                          for (int k = 0; k < Regexes.customTypes[i].Matches(fileText).Count; k++)
                          {
                              customItems[k][i] = matches[k].Value;
                          }
                      }


                      len = Regexes.variablesInfo[2].Matches(fileText).Count;
                      Variable[] variables = new Variable[len];

                      for (i = 0; i < len; i++)
                      {
                          variables[i] = new Variable();
                      }


                      for (i = 0; i < Regexes.variablesInfo.Length; i++)
                      {
                          matches = Regexes.variablesInfo[i].Matches(fileText);


                          for (int k = 0; k < len; k++)
                          {
                              variables[k][i] = matches[k].Value;
                          }
                      }

                      AuditModel auditModel = new AuditModel { CustomItems = customItems, GenInfo = genInfo, Variables = variables };

                      Audits.Insert(0, auditModel);

                      selectedAudit = auditModel; 
                       
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
                      toSave.GenInfo = selectedAudit.GenInfo;
                      toSave.Variables = selectedAudit.Variables;
                      List<CustomItem> temp = new List<CustomItem> { }; 

                      foreach (var item in selectedAudit.CustomItems)
                      {
                          if (item.PolicyBox)
                              temp.Add(item);
                      }

                      toSave.CustomItems = temp.ToArray();

                      db.InsertRecord(documentName, toSave);
                      
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
                selectedAudit = value;
                CheckPolicies = false;
                SaveName = "";
                Search = "";
                OnPropertyChanged("SelectedAudit");
            }
        }
        public ApplicationViewModel()
        {
            Audits = new ObservableCollection<AuditModel> { };
            db = new MongoCRUD(dbName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
