using System.Windows;

namespace TaskManager.Client.Model;

public class ControlDimensions
{
    public required Point TopLeft { get; init; }
    public Point Center
    { 
        get => new Point((int)(TopLeft.X + (Width / 2)), (int)(TopLeft.Y + (Height / 2)));  
    }
    public Point RightBottom
    {
        get => new Point((int)(TopLeft.X + Width), (int)(TopLeft.Y + Height));
    }

    public required double Width { get; init; }
    public required double Height { get; init; }
}
