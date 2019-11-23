using OnlineStore.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    [Table("GPU")]
    public class GPU
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Введите VRAM")]
        public string VRAM { get; set; }
    }
}