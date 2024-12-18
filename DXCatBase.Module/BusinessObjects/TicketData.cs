using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;

using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using System.Diagnostics;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.SystemModule;
using DXCatBase.Module.BusinessObjects;

namespace dxTestSolution.Module.BusinessObjects {
    [DefaultClassOptions]

    public class TicketData :BaseObject {
        public TicketData(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            this.EnteredDate = DateTime.Now;
        }

        string _firstName;
        [Size(SizeAttribute.Unlimited)]
        public string Subject {
            get {
                return _firstName;
            }
            set {
                SetPropertyValue(nameof(Subject), ref _firstName, value);
            }
        }
        string _lastName;
        [Size(SizeAttribute.Unlimited)]
        public string Question {
            get {
                return _lastName;
            }
            set {
                SetPropertyValue(nameof(Question), ref _lastName, value);
            }
        }
        string _featureId;
        public string FeatureId {
            get {
                return _featureId;
            }
            set {
                SetPropertyValue(nameof(FeatureId), ref _featureId, value);
            }
        }

        public Feature SelectedFeature {
            get {
                var f = this.Session.FindObject<Feature>(CriteriaOperator.FromLambda<Feature>(x => x.Oid.ToString() == FeatureId));
                return f;
            }
        }
        string _ticketId;
        public string TicketId {
            get {
                return _ticketId;
            }
            set {
                SetPropertyValue(nameof(TicketId), ref _ticketId, value);
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

        [PersistentAlias("SuggestedFeatures.Count()")]
        public int SuggestedFeaturesCount {
            get { return Convert.ToInt32(EvaluateAlias(nameof(SuggestedFeaturesCount))); }
        }

        [PersistentAlias("SuggestedFeatures[ToStr(Feature.Oid) = ^.FeatureId]")]
        public bool IsSuggestedFeaturesContainSelected {
            get { return Convert.ToBoolean(EvaluateAlias(nameof(IsSuggestedFeaturesContainSelected))); }
        }


        [Association]
        public XPCollection<FeaturePercentResult> SuggestedFeatures {
            get {
                return GetCollection<FeaturePercentResult>(nameof(SuggestedFeatures));
            }
        }
        //office#3		
        //[Association("Contact-Tasks")]
        //public XPCollection<MyTask> Tasks {
        //    get {
        //        return GetCollection<MyTask>(nameof(Tasks));
        //    }
        //}

        // byte[] _text;
        // [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        // public byte[] Text {
        // get {
        // return _text;
        // }
        // set {
        // SetPropertyValue(nameof(Text), ref _text, value);
        // }
        // }
        // private byte[] data;
        // [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)] 
        // public byte[] Data { 
        // get { return data; }
        // set { SetPropertyValue(nameof(Data), ref data, value); }
        // }
    }
}