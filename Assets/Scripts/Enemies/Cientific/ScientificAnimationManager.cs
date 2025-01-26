using Unity.VisualScripting;
using UnityEngine;

public class ScientificAnimationManager : MonoBehaviour
{
    private bool _isAnimationAttackRunning = true;
    private bool _haveBeenCapturedByBubble = false;

    public void AnimationAttackHaveStarted()
    {
        _isAnimationAttackRunning = true;
    }

    // AnimationAttackHaveFinished
    public void Aaa()
    {
        _isAnimationAttackRunning = false;
    }

    public bool IsAnimationAttackRunning()
    {
        return _isAnimationAttackRunning;
    }
}
