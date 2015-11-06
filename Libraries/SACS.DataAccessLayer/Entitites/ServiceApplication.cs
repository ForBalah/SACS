//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SACS.DataAccessLayer.Entitites
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceApplication
    {
        public ServiceApplication()
        {
            this.ServiceApplicationAudits = new HashSet<ServiceApplicationAudit>();
            this.ServiceApplicationPerfomances = new HashSet<ServiceApplicationPerfomance>();
            this.ServiceApplicationHistories = new HashSet<ServiceApplicationHistory>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Environment { get; set; }
        public string AppFilePath { get; set; }
        public string CronSchedule { get; set; }
        public bool Active { get; set; }
        public string CreatedByUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedByUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public StartupType StartupType { get; set; }
    
        public virtual ICollection<ServiceApplicationAudit> ServiceApplicationAudits { get; set; }
        public virtual ICollection<ServiceApplicationPerfomance> ServiceApplicationPerfomances { get; set; }
        public virtual ICollection<ServiceApplicationHistory> ServiceApplicationHistories { get; set; }
    }
}
