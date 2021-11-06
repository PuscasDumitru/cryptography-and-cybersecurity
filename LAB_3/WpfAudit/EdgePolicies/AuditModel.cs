
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WpfAudit.EdgePolicies
{
    public class AuditModel : INotifyPropertyChanged
    {
        [BsonId]
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public CustomItem[] CustomItems { get; set; }

        private bool _policyBoxes;
        public bool PolicyBoxes
        {
            get { return _policyBoxes; }
            set
            {
                _policyBoxes = value;
                OnPropertyChanged("PolicyBoxes");
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
