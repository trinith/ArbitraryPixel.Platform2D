using ArbitraryPixel.Platform2D.UI.Controller;

namespace ArbitraryPixel.Platform2D.UI.Factory
{
    public class SingleTouchButtonControllerFactory : IButtonControllerFactory
    {
        public IButtonController Create(IButton targetButton)
        {
            return new SingleTouchButtonController(targetButton);
        }
    }
}
