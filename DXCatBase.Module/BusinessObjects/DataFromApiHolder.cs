using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXCatBase.Module.BusinessObjects {
    [DomainComponent]
    public class DataFromApiHolder {
        public string URL { get; set; }

       
        public string Data { get; set; }
    }
    public class FeatureFromAPI        {
        public int NodeType { get; set; }
        public string id { get; set; }
        public List<object> items { get; set; }
        public object key { get; set; }
        public string parentId { get; set; }
        public object tag { get; set; }
        public string text { get; set; }
    }
}
