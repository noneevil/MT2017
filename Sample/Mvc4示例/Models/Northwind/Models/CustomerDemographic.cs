using System;
using System.Collections.Generic;

namespace Mvc4Example.Models
{
    public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            this.Customers = new List<Customer>();
        }

        public string CustomerTypeID { get; set; }
        public string CustomerDesc { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
