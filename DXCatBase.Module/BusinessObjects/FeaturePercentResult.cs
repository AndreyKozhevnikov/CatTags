using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using dxTestSolution.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXCatBase.Module.BusinessObjects {

    [DefaultClassOptions]
    public class FeaturePercentResult :BaseObject {
        public FeaturePercentResult(Session session)
        : base(session) {
        }
        Feature _feature;
        public Feature Feature {
            get {
                return _feature;
            }
            set {
                SetPropertyValue(nameof(Feature), ref _feature, value);
            }
        }

        int _result;
        public int Percentage {
            get {
                return _result;
            }
            set {
                SetPropertyValue(nameof(Percentage), ref _result, value);
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
        TicketData _assignedTo;
        [Association]
        public TicketData AssignedTicket {
            get {
                return _assignedTo;
            }
            set {
                SetPropertyValue(nameof(AssignedTicket), ref _assignedTo, value);
            }
        }

    }

    //    [DefaultClassOptions]
    //public class PromptResult :BaseObject {
    //    public PromptResult(Session session)
    //      : base(session) {
    //    }
    //    DateTime _birthDate;
    //    public DateTime DateRespond {
    //        get {
    //            return _birthDate;
    //        }
    //        set {
    //            SetPropertyValue(nameof(DateRespond), ref _birthDate, value);
    //        }
    //    }
    //    FeaturePrompt _prompt;
    //    public FeaturePrompt Prompt {
    //        get {
    //            return _prompt;
    //        }
    //        set {
    //            SetPropertyValue(nameof(Prompt), ref _prompt, value);
    //        }
    //    }
    //    Result _result;
    //    public Result Result {
    //        get {
    //            return _result;
    //        }
    //        set {
    //            SetPropertyValue(nameof(Result), ref _result, value);
    //        }
    //    }
    //    TicketData _ticketId;
    //    public TicketData TicketData {
    //        get {
    //            return _ticketId;
    //        }
    //        set {
    //            SetPropertyValue(nameof(TicketData), ref _ticketId, value);
    //        }
    //    }

    //    //Guid _actualFeatureSetManually;
    //    //public Guid ActualFeatureSetManually {
    //    //    get {
    //    //        return _actualFeatureSetManually;
    //    //    }
    //    //    set {
    //    //        SetPropertyValue(nameof(ActualFeatureSetManually), ref _actualFeatureSetManually, value);
    //    //    }
    //    //}



    //}
    //[DomainComponent]
    //public class PromptResultDataStub {
    //    public Guid PromptId { get; set; }
    //    public Result Result { get; set; }

    //    public Guid FeatureSetManually { get; set; }

    //    public string TicketId { get; set; }
    //}

    //public enum Result {
    //    Success = 0,
    //    Miss=2,
    //    NotApplicable=4
    //}
}
