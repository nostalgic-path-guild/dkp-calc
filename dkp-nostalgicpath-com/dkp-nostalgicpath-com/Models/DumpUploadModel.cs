using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dkp_nostalgicpath_com.Models
{
    public class DumpUploadModel
    {
        [Required(ErrorMessage = "Please select one or more files.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        [Required(ErrorMessage = "Please select your time zone.")]
        [Display(Name = "Time Zone")]
        public string TimeZoneId { get; set; } = "Eastern Standard Time";

        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId); }
            set { TimeZoneId = value.Id; }
        }

    }
}