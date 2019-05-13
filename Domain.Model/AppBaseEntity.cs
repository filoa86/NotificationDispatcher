using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class AppBaseEntity
    {
        public virtual int Id { get; set; }

        [StringLength(50)]
        [Column("insert_user")]
        public string InsertUser { get; set; }

        [Column("insert_datetime")]
        public DateTime? InsertDatetime { get; set; }

        [StringLength(50)]
        [Column("update_user")]
        public string UpdateUser { get; set; }

        [Column("update_datetime")]
        public DateTime? UpdateDatetime { get; set; }

        [Column("insert_guid")]
        public Guid? InsertGuid { get; set; }

        [Column("update_guid")]
        public Guid? UpdateGuid { get; set; }
    }
}
