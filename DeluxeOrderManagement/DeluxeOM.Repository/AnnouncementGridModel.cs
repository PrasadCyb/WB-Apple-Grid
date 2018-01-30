//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeluxeOM.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnnouncementGridModel
    {
        public int Id { get; set; }
        public Nullable<int> VIDId { get; set; }
        public Nullable<int> TerritoryId { get; set; }
        public Nullable<int> LanguageId { get; set; }
        public Nullable<int> LanguageTypeId { get; set; }
        public string MPM { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<bool> PrimaryVID { get; set; }
        public Nullable<int> PreOrderSDTitleStatus { get; set; }
        public Nullable<int> PreOrderHDTitleStatus { get; set; }
        public Nullable<int> ESTSDTitleStatus { get; set; }
        public Nullable<int> ESTHDTitleStatus { get; set; }
        public Nullable<int> VODSDTitleStatus { get; set; }
        public Nullable<int> VODHDTitleStatus { get; set; }
        public Nullable<System.DateTime> AnnouncementPreOrderSDDae { get; set; }
        public Nullable<System.DateTime> AnnouncementPreOrderHDDate { get; set; }
        public Nullable<System.DateTime> AnnouncementESTSDStart { get; set; }
        public Nullable<System.DateTime> AnnouncementESTHDStart { get; set; }
        public Nullable<System.DateTime> AnnouncementVODSDStart { get; set; }
        public Nullable<System.DateTime> AnnouncementVODHDStart { get; set; }
        public string FirstAnnouncedDate { get; set; }
        public string LastAnnouncementDate { get; set; }
        public int CustomerId { get; set; }
    }
}
