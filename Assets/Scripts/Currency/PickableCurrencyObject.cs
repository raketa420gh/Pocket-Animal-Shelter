using UnityEngine;

namespace Watermelon
{
    public class PickableCurrencyObject : MonoBehaviour, IPickableCurrencyObject
    {
        //private static readonly int PARTICLE_CONFETTI_HASH = ParticlesController.GetHash("Table Confetti");
        
        [SerializeField] private int _currencyAmount = 80;
        [SerializeField] private Currency.Type _currencyType = Currency.Type.Money;
        [SerializeField] private Animator _animator;
        private bool _isPicked = false;
        private bool _isPickable = false;

        private void OnEnable()
        {
            _isPicked = false;
            _isPickable = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.CompareTag(PhysicsHelper.TAG_PLAYER))
            {
                /*//PlayerBehavior playerBehavior = other.GetComponent<PlayerBehavior>();
                
                if (playerBehavior == null)
                    return;
                
                if (!_isPickable)
                    return;
                
                Pickup();
                playerBehavior.PlayMoneyPickUpParticle();
                gameObject.SetActive(false);*/
            }
        }

        public void Pickup()
        {
            if (_isPicked)
                return;
            
            if (!_isPickable)
                return;
            
            //AudioController.PlaySound(AudioController.Sounds.moneyPickUpSound);
            
            //CurrenciesController.Add(_currencyType, _currencyAmount);

            /*FloatingTextController.SpawnFloatingText(FloatingTextStyle.Money, "+" + _currencyAmount, transform.position + new Vector3(0, 6, 2));
            ParticlesController.PlayParticle(PARTICLE_CONFETTI_HASH).SetPosition(transform.position);*/

            _isPicked = true;
        }

        public void EnablePickable()
        {
            _isPickable = true;
            gameObject.SetActive(true);
            _animator.SetTrigger("Open");
        }
    }
}