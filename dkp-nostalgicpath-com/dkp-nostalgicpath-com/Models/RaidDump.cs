using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dkp_nostalgicpath_com.Models
{
    public class RaidDump
    {
        public DateTime DumpTime { get; set; }
        public List<Parse.RaidMember> Members { get; set; }
        public string FileName { get; set; }

    }
}