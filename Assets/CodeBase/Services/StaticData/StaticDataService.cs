using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowId, WindowConfig> _windows;
        private readonly IAssets _assets;

        public StaticDataService(IAssets assets) => 
            _assets = assets;

        public async Task Load() => 
            await Task.WhenAll(LoadMonsterStaticDataAsync(), LoadLevelStaticDataAsync(), LoadWindowsStaticData());

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters.TryGetValue(typeId, out var monsterStaticData) 
                ? monsterStaticData 
                : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out var levelStaticData) 
                ? levelStaticData 
                : null;

        public WindowConfig ForWindow(WindowId id) =>
            _windows.TryGetValue(id, out var windowConfig) 
                ? windowConfig 
                : null;

        private async Task LoadWindowsStaticData()
        {
            WindowsStaticData windowsStaticData = await _assets.LoadSingleForEntireLiceCycle<WindowsStaticData>(AssetAddress.WindowsStaticData);
            _windows = windowsStaticData
                .Configs
                .ToDictionary(x => x.WindowId);
        }

        private async Task LoadLevelStaticDataAsync()
        {
            IList<LevelStaticData> levelsStaticData = await _assets.LoadCollectionForEntireLiceCycle<LevelStaticData>(AssetAddress.LevelsStaticData);
            _levels = levelsStaticData
                .ToDictionary(x => x.SceneKey);
        }


        private async Task LoadMonsterStaticDataAsync()
        {
            IList<MonsterStaticData> monstersStaticData =
                await _assets.LoadCollectionForEntireLiceCycle<MonsterStaticData>(AssetAddress.MonstersStaticData);
            _monsters = monstersStaticData
                .ToDictionary(x => x.monsterTypeId);
        }
    }
}