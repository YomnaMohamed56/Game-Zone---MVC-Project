using GameZone.Data;
using GameZone.Services;
using GameZone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesServices _devicesServices;
        private readonly IGameServices _gameServices;

        public GamesController(ICategoriesService categoriesService, IDevicesServices devicesServices, IGameServices gameServices)
        {
            this._categoriesService = categoriesService;
            this._devicesServices = devicesServices;
            this._gameServices = gameServices;
        }
        
        public IActionResult Index()
        {
            var games = _gameServices.GetAll();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game = _gameServices.GetById(id);

            if (game is null)
                return NotFound();

            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesServices.GetSelectList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesServices.GetSelectList();
                return View(model);
            }
            await _gameServices.Create(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gameServices.GetById(id);
            
            if(game is null)
                return NotFound();

            EditGameFormViewModel viewModel = new EditGameFormViewModel()
            {
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.GameDevice.Select(g => g.DeviceId).ToList(),
                Devices = _devicesServices.GetSelectList(),
                Categories = _categoriesService.GetSelectList(),
                CurrentCover = game.Cover,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesServices.GetSelectList();
                return View(model);
            }
            
            var game = await _gameServices.Update(model);

            if (game is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gameServices.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }

    }
}
