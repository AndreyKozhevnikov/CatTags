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

namespace dxTestSolution.Module.BusinessObjects {
    [DefaultClassOptions]

    public class Category :XPCustomObject, ITreeNode {
        public Category(Session session)
            : base(session) {
        }


        
       
  
        Guid _oid;
        [Key(false)]
        public Guid Oid {
            get {
                return _oid;
            }
            set {
                SetPropertyValue(nameof(Oid), ref _oid, value);
            }
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

        [Association]
        public XPCollection<Feature> Features {
            get {
                return GetCollection<Feature>(nameof(Features));
            }
        }

        public ITreeNode Parent => null;

        public IBindingList Children => Features;
    }
}