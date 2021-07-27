using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HikingApp_RSWEB.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the HikingApp_RSWEBUser class
    public class HikingApp_RSWEBUser : IdentityUser
    {
        public int? PlaninarId { get; set; }

        public int? VodichId { get; set; }
    }
}
