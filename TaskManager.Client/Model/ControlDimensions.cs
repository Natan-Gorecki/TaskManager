using System.Windows;

namespace TaskManager.Client.Model;

public class ControlDimensions
{
    public Point TopLeft { get; set; }
    public Point Center
    { 
        get => new Point((int)(TopLeft.X + (Width / 2)), (int)(TopLeft.Y + (Height / 2)));  
    }
    public Point RightBottom
    {
        get => new Point((int)(TopLeft.X + Width), (int)(TopLeft.Y + Height));
    }

    public double Width { get; set; }
    public double Height { get; set; }
}
