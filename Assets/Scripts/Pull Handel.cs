using UnityEngine;

public class PullHandel : MonoBehaviour
{
    public Animator handel;
    private float blocktime = 3f;   
    public RollWheel rollWheel;

    private static readonly int PullTrigger = Animator.StringToHash("pull");

    private void OnMouseUpAsButton()
    {
        if (rollWheel.IsHandleLocked) return;

        PlayPullAnimationAndSpin();
        rollWheel.LockHandle();
        Soundcontroller.Instance.handel_sound();
    }

    private void PlayPullAnimationAndSpin()
    {
        if (handel == null) return;

        handel.SetTrigger(PullTrigger);
        rollWheel.rotate_the_wheel();
        rollWheel.cheat_apply_or_not();
    }
}