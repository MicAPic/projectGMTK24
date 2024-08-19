using Management;
using TMPro;
using UniTools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverView : UIView<GameOverManager>
    {
        [SerializeField] private ScrollWindow _scrollWindow;
        [Space]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _returnButton;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private string _tokenToReplace = "00";
        
        protected override void InitializeInternal()
        {
            _restartButton.Bind(Model.Restart);
            _returnButton.Bind(Model.Return);
        }

        public override void ShowScreen()
        {
            _description.text = _description.text.Replace(_tokenToReplace, Model.DayCount.ToString("00"));
            base.ShowScreen();
            _scrollWindow.Open();
        }
    }
}