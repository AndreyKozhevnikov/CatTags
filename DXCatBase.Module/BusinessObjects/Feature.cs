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
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.SystemModule;
using System.Diagnostics;

namespace dxTestSolution.Module.BusinessObjects {
    [DefaultClassOptions]
    
    [DebuggerDisplay("{Name} - {Oid}")]
    public class Feature :XPCustomObject, ITreeNode {
        public Feature(Session session)
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

        public ITreeNode Parent => ParentCategory;

        public IBindingList Children => new BindingList<object>();

        [Association]
        public XPCollection<FeaturePrompt> Prompts {
            get {
                return GetCollection<FeaturePrompt>(nameof(Prompts));
            }
        }

        public int TicketDataCount {
            get {
                return TicketData.Count;
            }
        }

        int _forSort;
        public int ForSort {
            get {
                return _forSort;
            }
            set {
                SetPropertyValue(nameof(ForSort), ref _forSort, value);
            }
        }

        private XPCollection<TicketData> noAssociation;
        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        public XPCollection<TicketData> TicketData {
            get {
                if(noAssociation == null) {
                    noAssociation = new XPCollection<TicketData>(Session, CriteriaOperator.FromLambda<TicketData>(x => x.FeatureId == this.Oid.ToString()));
                }
                return noAssociation;
            }
        }

    }

}