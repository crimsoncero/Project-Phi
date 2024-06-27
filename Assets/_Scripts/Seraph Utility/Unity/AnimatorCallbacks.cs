using UnityEngine;

[RequireComponent (typeof(Animator))]
public class AnimatorCallbacks : MonoBehaviour
{
    // Can only accepts parameters of type float, int, string, object ref, or an AnimationEvent.


    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;

    public void StopAnimation()
    {
        _animator.speed = 0;
    }
    public void HideRenderer()
    {
        _renderer.enabled = false;
    }
    public void ShowRenderer()
    {
        _renderer.enabled = true;
    }
}
