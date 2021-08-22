using System.Threading.Tasks;
using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        Task Load();
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId shop);
    }
}