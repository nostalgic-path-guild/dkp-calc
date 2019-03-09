using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dkp_nostalgicpath_com.Parse
{
    public static class RaidDump
    {
        private static string[] columns = new string[] { "Group", "Name", "Level", "Class", "Role", "5", "6", "Flagged" };

        public static List<RaidMember> ParseRaidDump(string dump)
        {
            var members = new List<RaidMember>();

            //Execute a loop over the rows.
            foreach (string row in dump.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    var rowData = row.Split('\t');

                    members.Add(new RaidMember()
                    {
                        Group = int.Parse(rowData[Array.IndexOf(columns, "Group")]),
                        Name = rowData[Array.IndexOf(columns, "Name")],
                        Level = int.Parse(rowData[Array.IndexOf(columns, "Level")]),
                        Class = rowData[Array.IndexOf(columns, "Class")],
                        Role = rowData[Array.IndexOf(columns, "Role")],
                        Flagged = rowData[Array.IndexOf(columns, "Flagged")] == "Yes"
                    });

                }
            }

            return members;
        }

        public static DateTime ParseRaidTime(string dumpFileName, TimeZoneInfo timeZone)
        {
            // RaidRoster_coirnav-20190304-222026
            var values = dumpFileName.Split('-');
            var year = int.Parse(values[1].Substring(0, 4));
            var month = int.Parse(values[1].Substring(4, 2));
            var day = int.Parse(values[1].Substring(6, 2));
            var hour = int.Parse(values[2].Substring(0, 2));
            var min = int.Parse(values[2].Substring(2, 2));
            var sec = int.Parse(values[2].Substring(4, 2));

            var output = new DateTime(year, month, day, hour, min, sec, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(output, timeZone);
        }
    }
}