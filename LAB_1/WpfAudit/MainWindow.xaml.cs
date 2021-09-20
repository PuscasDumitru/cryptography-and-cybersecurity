using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.IO;
using System.Text.RegularExpressions;



namespace audit
{
    public static class Regexes
    {
        public static Regex[] generalInfo;
        public static Regex[] customTypes;
        public static Regex[] variablesInfo;
        static Regexes()
        {
            generalInfo = new Regex[]
            {
            new Regex(@"(?<=Revision: )(.*)(?=\$)"),
            new Regex(@"(?<=Date: )(.*)(?=\$)"),
            new Regex(@"(?<=# description(.*): )(.*)"),
            new Regex(@"(?<=<display_name>)(.*)(?=</display_name>)"),
            new Regex(@"(?<=<check_type:"")(.*)(?="" version)"),
            new Regex(@"(?<=version:"")(.*)(?="">)"),
            new Regex(@"(?<=<group_policy:"")(.*)(?="">)")
            };

            customTypes = new Regex[]
            {
            new Regex(@"(?<= type(\s*): )(.*)"),
            new Regex(@"(?<=description(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=info(\s*): "")(.*?)(?="")", RegexOptions.Singleline),
            new Regex(@"(?<=solution(\s*): "")(.*?)(?="")", RegexOptions.Singleline),
            new Regex(@"(?<=reference(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=see_also(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=value_type(\s*): )(.*)"),
            new Regex(@"(?<=# Note: )(.*)"),
            new Regex(@"(?<=value_data(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=regex(\s*): ([""']))(.*)(?=([""']))"),
            new Regex(@"(?<=expect(\s*): ([""']))(.*)(?=([""']))")
            };


            variablesInfo = new Regex[]
            {
            new Regex(@"(?<=#    <name>)(.*)(?=</name>)"),
            new Regex(@"(?<=<default>)(.*)(?=</default>)"),
            new Regex(@"(?<=<description>)(.*)(?=</description>)"),
            new Regex(@"(?<=<info>)(.*)(?=</info>)")
            };
        }

    }
    public class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);

        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadDocument<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
    }

    public class AuditModel
    {
        [BsonId]
        public Guid Id { get; set; }

        public GeneralInfo GenInfo { get; set; }
        public CustomItem[] CustomItems { get; set; }
        public Variable[] Variables { get; set; }
    }


    public class GeneralInfo
    {

        public string Revision { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string CheckTypeOS { get; set; }
        public string CheckTypeVers { get; set; }
        public string GroupPolicy { get; set; }
    }
    public class CustomItem
    {
        public string PolicyType { get; set; }
        public string PolicyDescription { get; set; }
        public string PolicyInfo { get; set; }
        public string PolicySolution { get; set; }
        public string PolicyReference { get; set; }
        public string PolicySeeAlso { get; set; }
        public string PolicyValueType { get; set; }
        public string PolicyValueData { get; set; }
        public string PolicyNote { get; set; }
        public string PolicyRegex { get; set; }
        public string PolicyExpect { get; set; }


        public string this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0:
                        PolicyType = value;
                        break;
                    case 1:
                        PolicyDescription = value;
                        break;

                    case 2:
                        PolicyInfo = value;
                        break;
                    case 3:
                        PolicySolution = value;
                        break;
                    case 4:
                        PolicyReference = value;
                        break;
                    case 5:
                        PolicySeeAlso = value;
                        break;
                    case 6:
                        PolicyValueType = value;
                        break;
                    case 7:
                        PolicyValueData = value;
                        break;
                    case 8:
                        PolicyNote = value;
                        break;
                    case 9:
                        PolicyRegex = value;
                        break;
                    case 10:
                        PolicyExpect = value;
                        break;

                }
            }
        }
    }

    public class Variable
    {
        public string VariableName { get; set; }
        public string DefaultPath { get; set; }
        public string Description { get; set; }
        public string InfoPath { get; set; }

        public string this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0:
                        VariableName = value;
                        break;
                    case 1:
                        DefaultPath = value;
                        break;

                    case 2:
                        Description = value;
                        break;
                    case 3:
                        InfoPath = value;
                        break;


                }
            }
        }

    }

    class Program
    {
        static string path = @"C:\Users\dimasio\Desktop\mongodb\CIS_MongoDB_3.2_Benchmark_Level_1_OS_Windows_v1.0.0.audit";
        static string dbName = "AUDIT";
        static string documentName = "Audits";
        static MongoCRUD db = new MongoCRUD(dbName);
        static MatchCollection matches;
        static async Task Main(string[] args)
        {

            string fileText = "";
            try
            {

                using (StreamReader sr = new StreamReader(path))
                {
                    fileText = await sr.ReadToEndAsync();

                }
            }

            catch (Exception e)
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

            db.InsertRecord(documentName, auditModel);
            var dcm = db.LoadDocument<AuditModel>(documentName);

            try
            {

                using (StreamWriter sr = new StreamWriter(@"C:\Users\dimasio\Desktop\somefile.json"))
                {
                    await sr.WriteLineAsync(dcm.ToJson());
                }
            }

            catch (Exception e)
            {

            }
        }
    }
}

namespace WpfAudit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public static class Regexes
    {
        public static Regex[] generalInfo;
        public static Regex[] customTypes;
        public static Regex[] variablesInfo;
        static Regexes()
        {
            generalInfo = new Regex[]
            {
            new Regex(@"(?<=Revision: )(.*)(?=\$)"),
            new Regex(@"(?<=Date: )(.*)(?=\$)"),
            new Regex(@"(?<=# description(.*): )(.*)"),
            new Regex(@"(?<=<display_name>)(.*)(?=</display_name>)"),
            new Regex(@"(?<=<check_type:"")(.*)(?="" version)"),
            new Regex(@"(?<=version:"")(.*)(?="">)"),
            new Regex(@"(?<=<group_policy:"")(.*)(?="">)")
            };

            customTypes = new Regex[]
            {
            new Regex(@"(?<= type(\s*): )(.*)"),
            new Regex(@"(?<=description(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=info(\s*): "")(.*?)(?="")", RegexOptions.Singleline),
            new Regex(@"(?<=solution(\s*): "")(.*?)(?="")", RegexOptions.Singleline),
            new Regex(@"(?<=reference(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=see_also(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=value_type(\s*): )(.*)"),
            new Regex(@"(?<=# Note: )(.*)"),
            new Regex(@"(?<=value_data(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=regex(\s*): ([""']))(.*)(?=([""']))"),
            new Regex(@"(?<=expect(\s*): ([""']))(.*)(?=([""']))")
            };


            variablesInfo = new Regex[]
            {
            new Regex(@"(?<=#    <name>)(.*)(?=</name>)"),
            new Regex(@"(?<=<default>)(.*)(?=</default>)"),
            new Regex(@"(?<=<description>)(.*)(?=</description>)"),
            new Regex(@"(?<=<info>)(.*)(?=</info>)")
            };
        }

    }
    public class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);

        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadDocument<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
    }

    public class AuditModel
    {
        [BsonId]
        public Guid Id { get; set; }

        public GeneralInfo GenInfo { get; set; }
        public CustomItem[] CustomItems { get; set; }
        public Variable[] Variables { get; set; }
    }


    public class GeneralInfo
    {

        public string Revision { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string CheckTypeOS { get; set; }
        public string CheckTypeVers { get; set; }
        public string GroupPolicy { get; set; }
    }
    public class CustomItem
    {
        public string PolicyType { get; set; }
        public string PolicyDescription { get; set; }
        public string PolicyInfo { get; set; }
        public string PolicySolution { get; set; }
        public string PolicyReference { get; set; }
        public string PolicySeeAlso { get; set; }
        public string PolicyValueType { get; set; }
        public string PolicyValueData { get; set; }
        public string PolicyNote { get; set; }
        public string PolicyRegex { get; set; }
        public string PolicyExpect { get; set; }


        public string this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0:
                        PolicyType = value;
                        break;
                    case 1:
                        PolicyDescription = value;
                        break;

                    case 2:
                        PolicyInfo = value;
                        break;
                    case 3:
                        PolicySolution = value;
                        break;
                    case 4:
                        PolicyReference = value;
                        break;
                    case 5:
                        PolicySeeAlso = value;
                        break;
                    case 6:
                        PolicyValueType = value;
                        break;
                    case 7:
                        PolicyValueData = value;
                        break;
                    case 8:
                        PolicyNote = value;
                        break;
                    case 9:
                        PolicyRegex = value;
                        break;
                    case 10:
                        PolicyExpect = value;
                        break;

                }
            }
        }
    }

    public class Variable
    {
        public string VariableName { get; set; }
        public string DefaultPath { get; set; }
        public string Description { get; set; }
        public string InfoPath { get; set; }

        public string this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0:
                        VariableName = value;
                        break;
                    case 1:
                        DefaultPath = value;
                        break;

                    case 2:
                        Description = value;
                        break;
                    case 3:
                        InfoPath = value;
                        break;


                }
            }
        }

    }

    public partial class MainWindow : Window
    {
        static string path = "";
        static string dbName = "AUDIT";
        static string documentName = "Audits";
        static MongoCRUD db = new MongoCRUD(dbName);
        static MatchCollection matches;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            UploadPath.Content = "";
            OpenFileDialog fileDialog = new OpenFileDialog();
            
            fileDialog.ShowDialog();
            UploadPath.Content = fileDialog.FileName;
            path = fileDialog.FileName;
            
        }
        
        private void SaveConvert_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;

            dialog.FileName = "Folder Selection.";
            dialog.ShowDialog();
            MessageBox.Show("Converted file saved!");

            if (ToSaveName.Text != "")
            {
                string convertedFilePath = @dialog.FileName.Replace("Folder Selection", ToSaveName.Text + ".json");
                
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

                db.InsertRecord(documentName, auditModel);
                var dcm = db.LoadDocument<AuditModel>(documentName);

                try
                {

                    using (StreamWriter sr = new StreamWriter(convertedFilePath))
                    {
                        sr.WriteLine(dcm.ToJson());
                    }
                }

                catch (Exception ex)
                {

                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
    }
}
