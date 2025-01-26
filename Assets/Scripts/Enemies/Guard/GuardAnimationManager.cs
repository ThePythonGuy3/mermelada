using Unity.VisualScripting;
using UnityEngine;

public class GuardAnimationManager : MonoBehaviour
{
    private bool _isAnimationAttackRunning = true;

    public void AnimationAttackHaveStarted()
    {
        _isAnimationAttackRunning = true;
    }

    // AnimationAttackHaveFinished
    public void Bbb()
    {
        _isAnimationAttackRunning = false;
    }

    public bool IsAnimationAttackRunning()
    {
        return _isAnimationAttackRunning;
    }
}
