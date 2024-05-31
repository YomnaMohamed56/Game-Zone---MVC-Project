using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Game : BaseEntity
    {
        [MaxLength(length: 2500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(length: 500)]
        public string Cover { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = default!;

        public ICollection<GameDevice> GameDevice { get; set; } = new List<GameDevice>();
    }
}
