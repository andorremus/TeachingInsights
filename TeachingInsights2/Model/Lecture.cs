namespace TeachingInsights2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeachingInsights.Lecture")]
    public partial class Lecture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int lectureId { get; set; }

        public double? averageEngagement { get; set; }

        public double? averageAnxiety { get; set; }

        public double? averageConfusion { get; set; }

        public int courseId { get; set; }

        public DateTime? dateTimeTakenPlace { get; set; }

        public virtual Course Course { get; set; }
    }
}
