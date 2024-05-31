

using GameZone.Data;
using GameZone.Models;
using GameZone.Settings;
using GameZone.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class GameServices : IGameServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly string _imagesPath;

        public GameServices(ApplicationDbContext context, IWebHostEnvironment WebHostEnvironment)
        {
            this._context = context;
            this._WebHostEnvironment = WebHostEnvironment;
            _imagesPath = $"{_WebHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }
        public async Task Create(CreateGameFormViewModel model)
        {
            var CoverName = await SaveCover(model.Cover);

            Game game = new Game()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = CoverName,
                GameDevice = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList(),
            };

            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevice)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToList();
        }

        public Game? GetById(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevice)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);

        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                        .Include(g => g.GameDevice)
                        .SingleOrDefault(g => g.Id ==  model.Id);
                

            if (game is null)
                return null;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.GameDevice = model.SelectedDevices.Select(d => new GameDevice() { DeviceId = d }).ToList();

            var hasNewCover = model.Cover is not null;
            var oldeCover = game.Cover;

            if (hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!);
            }

            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldeCover);
                    File.Delete(cover);
                }

                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
                return null;
            }
        }

        private async Task<String> SaveCover(IFormFile Cover)
        {
            var CoverName = $"{Guid.NewGuid()}{Path.GetExtension(Cover.FileName)}";

            var path = Path.Combine(_imagesPath, CoverName);

            using var stream = File.Create(path);
            await Cover.CopyToAsync(stream);

            return CoverName;
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.Find(id);

            if (game is null)
                return isDeleted;

            _context.Games.Remove(game);
            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0)
            {
                isDeleted = true;
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }
    }
}
