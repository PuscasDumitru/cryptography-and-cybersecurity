using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WpfAudit.EdgePolicies
{
    public static class Regexes
    {

        public static Regex[] customTypes;

        static Regexes()
        {


            customTypes = new Regex[]
            {
            new Regex(@"(?<= type(\s*): )(.*)"),
            new Regex(@"(?<=description(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=info(\s*): "")(.*?)(?="")", RegexOptions.Singleline),
            new Regex(@"(?<=solution(\s*): "")(.*?)(?="")", RegexOptions.Singleline),

            new Regex(@"(?<=see_also(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=value_type(\s*): )(.*)"),

            new Regex(@"(?<=value_data(\s*): "")(.*)(?="")"),
            new Regex(@"(?<=reg_key(\s*): ([""']))(.*)(?=([""']))"),
            new Regex(@"(?<=reg_item(\s*): ([""']))(.*)(?=([""']))"),
            new Regex(@"(?<=reg_option(\s*): )(.*)"),
            new Regex(@"(?<=reference(\s*): "")(.*)(?="")")
            };



        }

    }
}
