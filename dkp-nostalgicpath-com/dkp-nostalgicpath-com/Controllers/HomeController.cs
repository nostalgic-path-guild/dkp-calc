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
            if (ModelState.IsValid)
            {
                ViewBag.ParsedDumps = new List<string>();
                var dumps = new List<RaidDump>();
                var dumpErrors = false;

                try
                {
                    foreach (var file in model.files)
                    {
                        byte[] data;

                        using (var reader = new BinaryReader(file.InputStream))
                            data = reader.ReadBytes(file.ContentLength);

                        var dump = new RaidDump()
                        {
                            Members = Parse.RaidDump.ParseRaidDump(System.Text.Encoding.UTF8.GetString(data)),
                            FileName = file.FileName
                        };

                        try
                        {
                            dump.DumpTime = Parse.RaidDump.ParseRaidTime(file.FileName, model.TimeZone);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, $"Could not parse the dump time.  Please check your filename, the time must be in the format: RaidDump_coirnav-yearmonthday-hourminutesecond.txt.  File: {file.FileName}");
                            dumpErrors = true;
                            break;
                        }

                        dumps.Add(dump);
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    while (ex != null)
                    {
                        msg += ex.Message + "<br />";
                        ex = ex.InnerException;
                    }

                    ModelState.AddModelError(string.Empty, msg);
                    dumpErrors = true;

                }

                try
                {
                    if (!dumpErrors)
                    {
                        using (var db = new DB.NPGuildEntities())
                        {
                            foreach (var dump in dumps)
                            {
                                foreach (var member in dump.Members)
                                {
                                    var attendance = new DB.RaidAttendance()
                                    {
                                        Group = member.Group,
                                        Name = member.Name,
                                        Level = member.Level,
                                        Class = member.Class,
                                        Role = member.Role,
                                        Flagged = member.Flagged,
                                        DumpTime = dump.DumpTime,
                                        DKP = 5,
                                        Include = true,
                                        Processed = false
                                    };

                                    db.RaidAttendances.Add(attendance);
                                }

                                ViewBag.ParsedDumps.Add($"{TimeZoneInfo.ConvertTimeFromUtc(dump.DumpTime, model.TimeZone)} ({dump.Members.Count})");
                            }

                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ParsedDumps = null;

                    var msg = string.Empty;
                    while (ex != null)
                    {
                        msg += ex.Message + "<br />";
                        ex = ex.InnerException;
                    }

                    ModelState.AddModelError(string.Empty, msg);
                    dumpErrors = true;

                }

            }
            return View("Index");
        }
    }
}