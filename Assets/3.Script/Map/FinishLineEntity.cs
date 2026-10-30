using TMS.Core;
using TMS.Event;
using TMS.Player;
using UnityEngine;

namespace TMS.Map
{
    public class FinishLineEntity : MonoBehaviour, IInteractable
    {
        [SerializeField] private AudioClip _finishSound; // Inspector에서 드래그로 할당
        private AudioSource _audioSource;
        private bool _triggered = false;

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f;
        }

        public void OnInteract(PlayerEntity player)
        {
            if (_triggered || player == null) return;
            _triggered = true;

            if (_finishSound != null)
            {
                _audioSource.PlayOneShot(_finishSound);
            }

            GameManager.Instance.ClearGame();
            player.OnDead();

            Destroy(gameObject, _finishSound != null ? _finishSound.length : 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEntity player))
            {
                OnInteract(player);
            }
        }
    }
}