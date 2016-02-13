namespace TeachingInsights2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeachingInsights.UserHasCourse")]
    public partial class UserHasCourse
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(64)]
        public string username { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int courseId { get; set; }

        [Column(TypeName = "bit")]
        public bool approved { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }
    }
}
