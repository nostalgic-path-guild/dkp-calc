using dkp_nostalgicpath_com.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dkp_nostalgicpath_com.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadParse(DumpUploadModel model)
        {
            if(ModelState.IsValid)
            {
                ViewBag.ParsedDumps = new List<string>();

                using (var db = new DB.NPGuildEntities())
                {
                    foreach (var file in model.files)
                    {
                        byte[] data;

                        using (var reader = new BinaryReader(file.InputStream))
                        {
                            data = reader.ReadBytes(file.ContentLength);
                        }
                        var raidMembers = Parse.RaidDump.ParseRaidDump(System.Text.Encoding.UTF8.GetString(data));
                        var dumpTime = Parse.RaidDump.ParseRaidTime(file.FileName, model.TimeZone);

                        foreach (var member in raidMembers)
                        {
                            var attendance = new DB.RaidAttendance()
                            {
                                Group = member.Group,
                                Name = member.Name,
                                Level = member.Level,
                                Class = member.Class,
                                Role = member.Role,
                                Flagged = member.Flagged,
                                DumpTime = dumpTime,
                                DKP = 5,
                                Include = true,
                                Processed = false
                            };

                            db.RaidAttendances.Add(attendance);
                            db.SaveChanges();
                        }

                        ViewBag.ParsedDumps.Add($"{TimeZoneInfo.ConvertTimeFromUtc(dumpTime, model.TimeZone)} ({raidMembers.Count})");
                    }
                }
            }
            return View("Index");
        }
    }
}