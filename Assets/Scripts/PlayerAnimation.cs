using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    [SerializeField] private GameObject _playerModel;

    [Space]
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip walkingAnim, idleAnim, inAirAnim, placingAnim,jumpingAnim;


    private PlayerMovement movement;


    public GameObject playerModel {get; private set;}


    private void Awake() {
        movement = GetComponent<PlayerMovement>();
    }

    public void Animate() {
        anim.SetBool("isWalking", movement.IsWalking());
        anim.SetBool("isGrounded", movement.isGrounded);

        if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == inAirAnim.name && movement.isGrounded){
            anim.Play(idleAnim.name);
        }
    }

    public void PlayJumpAnimation() {
        anim.Play(jumpingAnim.name);
    }

    public void PlayBuildAnimation() {
        anim.Play(placingAnim.name);
    }

    public void SetModelFacing(Vector3 forward) {
        playerModel.transform.forward = forward;
    }
}