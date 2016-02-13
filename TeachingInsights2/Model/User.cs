namespace TeachingInsights2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeachingInsights.User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            UserHasCourses = new HashSet<UserHasCourse>();
            CalibrationSettings = new HashSet<CalibrationSetting>();
        }

        [Key]
        [StringLength(64)]
        public string username { get; set; }

        [Required]
        [StringLength(128)]
        public string password { get; set; }

        [Required]
        [StringLength(45)]
        public string firstName { get; set; }

        [Required]
        [StringLength(45)]
        public string lastName { get; set; }

        [Required]
        [StringLength(45)]
        public string emailAddress { get; set; }

        public int userTypeId { get; set; }

        public int studentId { get; set; }

        [StringLength(45)]
        public string currentIpAddress { get; set; }

        [Column(TypeName = "bit")]
        public bool isConnected { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserHasCourse> UserHasCourses { get; set; }

        public virtual UserType UserType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationSetting> CalibrationSettings { get; set; }


    }
}
