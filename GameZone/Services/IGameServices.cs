using GameZone.Models;
using GameZone.ViewModels;

namespace GameZone.Services
{
    public interface IGameServices
    {
        IEnumerable<Game> GetAll();

        Game? GetById(int id);

        Task Create(CreateGameFormViewModel game);

        Task<Game?> Update(EditGameFormViewModel game);

        bool Delete(int id);
    }
}
