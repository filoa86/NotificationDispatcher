using Domain.Model.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    [Table("MESSAGE")]
    [AppIdGenerationAttribute]
    public class Message : AppBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id_message")]
        public override int Id { get; set; }
    }
}
