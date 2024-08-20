public interface IAmToggleable
{
    public bool IsActive { get; set; }

    void SetToggleState(bool toggle) { } 
}
