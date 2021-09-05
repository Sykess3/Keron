using System.Threading.Tasks;
using CodeBase.Services;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory 
    {
        void CreateShop();
        Task CreateUIRoot();
    }
}