using System;
using TVS.API.Entities;

namespace TVS.API.Models
{
    public class ReportMoveModel
    {
        public bool HasEndDate { get; set; }
        public Address Address { get; set; }
        public DateTime? MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
        public Person Landlord { get; set; }
    }
}
