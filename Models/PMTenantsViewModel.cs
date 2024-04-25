using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPN.Property_Rental_Management_Final_Project.Models
{
    public class PMTenantsViewModel
    {

        public List<Employee> PropertyManagers { get; set; }
        public List<Tenant> Tenants { get; set; }

    }
}