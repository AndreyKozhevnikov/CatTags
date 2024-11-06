using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace dxTestSolution.Module.BusinessObjects {
    [DefaultClassOptions]
    public class FeaturePrompt :BaseObject {

        public FeaturePrompt(Session session)
        : base(session) {
        }

        string _lastName;
        [Size(SizeAttribute.Unlimited)]
        public string Prompt {
            get {
                return _lastName;
            }
            set {
                SetPropertyValue(nameof(Prompt), ref _lastName, value);
            }
        }

        DateTime _enteredDate;
        public DateTime EnteredDate {
            get {
                return _enteredDate;
            }
            set {
                SetPropertyValue(nameof(EnteredDate), ref _enteredDate, value);
            }
        }
        Feature _assignedTo;
        [Association]
        public Feature Feature {
            get {
                return _assignedTo;
            }
            set {
                SetPropertyValue(nameof(Feature), ref _assignedTo, value);
            }
        }

    }

}