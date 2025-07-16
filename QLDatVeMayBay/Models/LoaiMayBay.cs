﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class LoaiMayBay
    {
        [Key]
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoaiMayBayId { get; set; }

        [Required]
        public int TongSoGhe { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }
    }
}
