using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WpfAudit.Classes
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
}
