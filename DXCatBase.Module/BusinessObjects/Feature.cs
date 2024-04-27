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
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.SystemModule;

namespace dxTestSolution.Module.BusinessObjects {
     [DefaultClassOptions]
     [DefaultProperty("Subject")]
    public class Feature : BaseObject {
        public Feature(Session session)
            : base(session) {
        }
        string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                SetPropertyValue(nameof(Name), ref _name, value);
            }
        }
        Category _parentCategory;
        [Association]
        public Category ParentCategory {
            get {
                return _parentCategory;
            }
            set {
                SetPropertyValue(nameof(ParentCategory), ref _parentCategory, value);
            }
        }
      
    }

}