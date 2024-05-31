using GameZone.Attributes;
using GameZone.Settings;

namespace GameZone.ViewModels
{
    public class EditGameFormViewModel : GameFormViewModel
    {
        public int Id {  get; set; }

        public string? CurrentCover { get; set; }

        [AllowedExtentions(FileSettings.AllowedExtentions),
         MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile? Cover { get; set; } = default!;
    }
}
