using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LUCSALEInvestRADZEN.Models.LUCSALE_Exemplos
{
    [Table("Prod", Schema = "dbo")]
    public partial class Prod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}