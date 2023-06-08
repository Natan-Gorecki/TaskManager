namespace TaskManager.Core.Models;

public class Task : ObservableObject, ITask
{
    private int _id;
    public int Id 
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string? _name;
    public string? Name 
    { 
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private ETaskStatus _status;
    public ETaskStatus Status 
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private int _orderValue;
    public int OrderValue 
    {
        get => _orderValue;
        set => SetProperty(ref _orderValue, value); 
    }

    private bool _isPreview;
    public bool IsPreview 
    { 
        get => _isPreview; 
        set => SetProperty(ref _isPreview, value);
    }
}
