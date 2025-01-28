using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LUCSALEInvestRADZEN.Models.CadastroDB
{
    [Table("Alunos", Schema = "dbo")]
    public partial class Aluno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        [Required]
        public int Idade { get; set; }
    }
}