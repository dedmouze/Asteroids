public interface IPlayerMovementSubscriber : 
    IPlayerAccelerationSubscriber, 
    IPlayerSlowdownSubscriber,
    IPlayerRotationSubscriber,
    IGlobalSubscriber {}