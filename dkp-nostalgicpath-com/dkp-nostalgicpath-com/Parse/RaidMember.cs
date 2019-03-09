using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dkp_nostalgicpath_com.Parse
{
    public class RaidMember
    {
        public int Group { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Class { get; set; }
        public string Role { get; set; }
        public bool Flagged { get; set; }
    }
}