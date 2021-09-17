using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace audit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                string path = @"C:\Users\dimasio\Desktop\mongodb\CIS_MongoDB_3.2_Benchmark_Level_1_OS_Windows_v1.0.0.audit";
                using (StreamReader sr = new StreamReader(path))
                {
                    string str = await sr.ReadToEndAsync();


                    Regex revision = new Regex(@"Revision(.*)(?=\$)");
                    Regex date = new Regex(@"Date(.*)(?=\$)");
                    Regex auditDescription = new Regex(@"# description(.*)");
                    Regex displayName = new Regex(@"#<display_name>(.*)(?=</display_name>)"); // delete #<display_name>
                    Regex typeOS = new Regex(@"# description(.*)");

                    //Regex singleDescriptionBeginning = new Regex(@"# description(.*)");

                    //Regex singleDescriptionBeginning = new Regex(@"# description(.*)");
                    //Regex singleDescriptionBeginning = new Regex(@"# description(.*)");

                    Regex types = new Regex(@" type(\s*): (.*)\n");
                    Regex descriptions = new Regex(@" description(.*)\n ");
                    Regex infos = new Regex(@" info(.*?)""(.*?)(?="")", RegexOptions.Singleline);
                    Regex solutions = new Regex(@" solution(.*?)""(.*?)(?="")", RegexOptions.Singleline);
                    Regex references = new Regex(@" reference(.*)\n ");
                    Regex seeAlso = new Regex(@" see_also(.*)\n ");
                    Regex notes = new Regex(@"# Note:(.*)\n ");
                    Regex valueData = new Regex(@" value_data(.*)\n ");
                    Regex regex = new Regex(@" regex(.*)\n ");
                    Regex expect = new Regex(@" expect(.*)");
                    

                    Regex regex2 = new Regex(@"<custom_type>(.*)\n(.*)</custom_type>(.*)");
                    MatchCollection matches = displayName.Matches(str);

                    foreach (Match match in matches)
                    {
                        Console.WriteLine(match.Value + "\n=============================\n");
                        
                    }
                    
                }
            }
           
            catch(Exception e)
            {

            }
        }
    }
}
