﻿using System;
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

namespace dxTestSolution.Module.BusinessObjects {
     [DefaultClassOptions]
	  
    public class TicketData : BaseObject { 
        public TicketData(Session session)
            : base(session) {
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
        string _ticketId;
        public string TicketId {
            get {
                return _ticketId;
            }
            set {
                SetPropertyValue(nameof(TicketId), ref _ticketId, value);
            }
        }
        // DateTime _birthDate;
        // public DateTime BirthDate {
        // get {
        // return _birthDate;
        // }
        // set {
        // SetPropertyValue(nameof(BirthDate), ref _birthDate, value);
        // }
        // }	
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