using UnityEngine;

public class SwitchBulletAccepter : BulletAccepter
{
    [SerializeField] private MixerController mixer;

    public override void OnHit()
    {
        mixer.Hit();
    }
}
