using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectStudent.Models
{
    [Table("TB_STUDENT")] 
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [StringLength(255)]
        [Column("NAME")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A Idade é obrigatória.")]
        [Column("AGE")]
        public int Age { get; set; }

        [Column("BIRTH_DATE")]
        public DateTime BirthDate { get; set; }

        [StringLength(100)]
        [Column("COURSE")]
        public string Course { get; set; }
    }
}