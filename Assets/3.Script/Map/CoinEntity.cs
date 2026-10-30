using TMS.Event;
using TMS.Player;
using UnityEngine;

namespace TMS.Map
{
    public interface IInteractable
    {
        public void OnInteract(PlayerEntity player);
    }

    public class CoinEntity : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _coinScore = 10;
        [SerializeField] private AudioClip _collectSound; // Inspector에서 지정
        private AudioSource _audioSource;

        private bool _collected = false;

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f; // 2D 사운드
        }

        public void OnInteract(PlayerEntity player)
        {
            if (_collected || player == null) return;

            _collected = true;

            EventBus.Publish(new AcquireCoinEvent(_coinScore));

            if (_collectSound != null)
            {
                _audioSource.PlayOneShot(_collectSound);
            }

            Destroy(gameObject, _collectSound != null ? _collectSound.length : 0f);
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