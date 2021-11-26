using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WpfAudit.Classes
{
    public class CustomItem : INotifyPropertyChanged
    {
        public string PolicyType { get; set; } = "";
        public string PolicyDescription { get; set; } = "";
        public string PolicyInfo { get; set; } = "";
        public string PolicySolution { get; set; } = "";
        public string PolicyReference { get; set; } = "";
        public string PolicySeeAlso { get; set; } = "";
        public string PolicyValueType { get; set; } = "";
        public string PolicyValueData { get; set; } = "";
        public string PolicyNote { get; set; } = "";
        public string PolicyRegex { get; set; } = "";
        public string PolicyExpect { get; set; } = "";


        private bool _policyBox;
        public bool PolicyBox
        {
            get { return _policyBox; }
            set
            {
                _policyBox = value;
                OnPropertyChanged("PolicyBox");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return PolicyType;
                        
                    case 1:
                        return PolicyDescription;
                        
                    case 2:
                        return PolicyInfo ;
                        
                    case 3:
                        return PolicySolution;
                        
                    case 4:
                        return PolicyReference;
                        
                    case 5:
                        return PolicySeeAlso;
                        
                    case 6:
                        return PolicyValueType;
                        
                    case 7:
                        return PolicyValueData;
                        
                    case 8:
                        return PolicyNote;
                        
                    case 9:
                        return PolicyRegex;
                        
                    case 10:
                        return PolicyExpect;
                    default:
                        return PolicyDescription;
                }
            }
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
}
