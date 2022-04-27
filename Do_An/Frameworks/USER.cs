namespace Do_An.Frameworks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USERS")]
    public partial class USER
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string UserN { get; set; }

        [StringLength(50)]
        public string PassW { get; set; }

        public int? Roles { get; set; }

        public virtual INFORMATION INFORMATION { get; set; }
    }
}
