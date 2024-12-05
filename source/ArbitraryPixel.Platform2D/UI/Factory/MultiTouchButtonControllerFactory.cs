using ArbitraryPixel.Platform2D.UI.Controller;

namespace ArbitraryPixel.Platform2D.UI.Factory
{
    public class MultiTouchButtonControllerFactory : IButtonControllerFactory
    {
        public IButtonController Create(IButton targetButton)
        {
            return new MultiTouchButtonController(targetButton);
        }
    }
}
