public interface IControlTypeSubscriber : IGlobalSubscriber
{
    void OnControlTypeChanged(ControlType type);
}