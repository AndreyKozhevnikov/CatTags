using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXCatBase.Module.BusinessObjects {
    [DebuggerDisplay("{tagName} - {tagId}")]
    public class TagAIResult {
        public string tagName { get; set; }
        public string tagId { get; set; }
        public string parentTagId { get; set; }
        public int percentage { get; set; }
    }
}
