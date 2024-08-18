using Management;
using UniTools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverView : UIView<GameOverManager>
    {
        [SerializeField] private Button _restartButton;
        
        protected override void InitializeInternal()
        {
            _restartButton.Bind(Model.Restart);
        }
    }
}