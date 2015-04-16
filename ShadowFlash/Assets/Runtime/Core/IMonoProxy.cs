public interface IMonoProxy
{
    void Awake();

    void OnEnable();

    void OnDisable();

    void Update();

    void FixedUpdate();

    void OnDestroy();
}
