public interface BulletObject
{
    public BulletPool BulletPool { get; set; }
    public void Restart();
    public void DestroyObject();
}
