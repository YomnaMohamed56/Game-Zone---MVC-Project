using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Device : BaseEntity
    {
        [MaxLength(length:100)]
        public string Icon { get; set; } = string.Empty;

        public ICollection<GameDevice> GameDevice { get; set; } = new List<GameDevice>();
    }
}
