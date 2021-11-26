using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WpfAudit.EdgePolicies
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
        public string PolicyRegKey { get; set; } = "";
        public string PolicyRegItem { get; set; } = "";
        public string PolicyRegOption { get; set; } = "";


        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set
            {
                _reason = value;
                OnPropertyChanged("Reason");

            }
        }

        private bool _visibleReason;
        public bool VisibleReason
        {
            get { return _visibleReason; }
            set
            {
                _visibleReason = value;
                OnPropertyChanged("VisibleReason");

            }
        }

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
                        return PolicyInfo;

                    case 3:
                        return PolicySolution;

                    case 4:
                        return PolicySeeAlso;

                    case 5:
                        return PolicyValueType;

                    case 6:
                        return PolicyValueData;

                    case 7:
                        return PolicyRegKey;

                    case 8:
                        return PolicyRegItem;

                    case 9:
                        return PolicyRegOption;

                    case 10:
                        return PolicyReference;

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
                        PolicySeeAlso = value;
                        break;
                    case 5:
                        PolicyValueType = value;
                        break;
                    case 6:
                        PolicyValueData = value;
                        break;
                    case 7:
                        PolicyRegKey = value;
                        break;
                    case 8:
                        PolicyRegItem = value;
                        break;
                    case 9:
                        PolicyRegOption = value;
                        break;
                    case 10:
                        PolicyReference = value;
                        break;

                }
            }
        }
    }
}
