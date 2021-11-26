using System;
using System.Collections.Generic;
using System.Text;

namespace WpfAudit.Classes
{
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
}
