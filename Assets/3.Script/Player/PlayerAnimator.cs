using TMS.Core;
using UnityEngine;

namespace TMS.Player
{
    public class PlayerAnimator : EntityComponent
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private AudioClip _wireStartSound; // Inspector에서 드래그
        private AudioSource _audioSource;

        private static readonly int IsWiring = Animator.StringToHash("IsWiring");
        private bool _wasWiring = false;

        public override void OnPlay()
        {
            base.OnPlay();
            _animator.SetTrigger("Play");

            // 오디오소스 추가
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f;
        }

        public override void OnDead()
        {
            base.OnDead();
            _animator.SetTrigger("Reset");
        }

        public override void ManualUpdate()
        {
            UpdateAnimatorParameters();
        }

        private void UpdateAnimatorParameters()
        {
            bool isWiring = _playerMovement.IsWiring;

            // 상태 전이 감지: false → true일 때만 사운드 재생
            if (!_wasWiring && isWiring)
            {
                if (_wireStartSound != null)
                {
                    _audioSource.PlayOneShot(_wireStartSound);
                }
            }

            _wasWiring = isWiring;
            _animator.SetBool(IsWiring, isWiring);
        }
    }
}