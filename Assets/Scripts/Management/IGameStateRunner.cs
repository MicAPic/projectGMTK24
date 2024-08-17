using System.Collections;
using Configs;

namespace Management
{
    public interface IGameStateManager
    {
        void Initialize(ConfigurationsHolder configuration);
        IEnumerator Run();
    }
}