using ArbitraryPixel.Platform2D.UI.Controller;

namespace ArbitraryPixel.Platform2D.UI.Factory
{
    public interface IButtonControllerFactory
    {
        IButtonController Create(IButton targetButton);
    }
}
