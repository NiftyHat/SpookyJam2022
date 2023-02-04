using Data.GameOver;
using UI;

public class GameOverState
{
        private GameOverReasonData _reason;
        private GameOverRevealView.Data _revealAnimViewData;

        public GameOverReasonData Reason => _reason;
        public GameOverRevealView.Data RevealAnimData => _revealAnimViewData;

        public GameOverState(GameOverReasonData reason, GameOverRevealView.Data viewData)
        {
                _reason = reason;
                _revealAnimViewData = viewData;
        }
        
        public GameOverState(GameOverReasonData reason)
        {
                _reason = reason;
                _revealAnimViewData = default;
        }
        
}