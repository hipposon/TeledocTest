//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeledocTest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FirmsFounder
    {
        public int Id { get; set; }
        public int FirmId { get; set; }
        public int FounderId { get; set; }
    
        public virtual Firm Firm { get; set; }
        public virtual Founder Founder { get; set; }
    }
}
