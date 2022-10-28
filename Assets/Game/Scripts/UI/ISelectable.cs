namespace UI
{
    public class ISelectable<TTarget>
    {
        private bool isSelected { get; }
        private TTarget Target { get; } 
    }
}