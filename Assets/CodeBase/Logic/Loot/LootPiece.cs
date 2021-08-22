using System.Collections;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure;
using TMPro;
using UnityEngine;

namespace CodeBase.Logic.Loot
{
    [RequireComponent(typeof(UniqueId))]
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private GameObject _skull;
        [SerializeField] private ParticleSystem _pickupFxPrefab;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;

        private LootData _lootData;
        private bool _pickedUp;

        public string Id { get; private set; }

        public void Initialize(LootData lootData)
        {
            _lootData = lootData;
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(Id)) 
                Id = GetComponent<UniqueId>().Id;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<HeroWallet>(out var heroWallet))
                PickUp(heroWallet);
        }

        public void LoadProgress(PlayerProgress @from)
        { }

        public void UpdateProgress(ref PlayerProgress to)
        {
            NonPickedUpLoot lootPiecesSavedData = to.WorldData.NonPickedUpLoot;
            if (_pickedUp) 
                lootPiecesSavedData.SafeRemove(Id);
            else if (!lootPiecesSavedData.Contains(Id)) 
                UpdateWorldData(lootPiecesSavedData);

        }

        private void PickUp(HeroWallet wallet)
        {
            if (_pickedUp)
                return;

            _pickedUp = true;


            UpdateHeroWallet(wallet);

            HideSkull();
            ShowText();
            SpawnPickupFx();

            StartCoroutine(DestroyTimer());
        }

        private void ShowText()
        {
            _lootText.text = $"{_lootData.Money.Amount}";
            _pickupPopup.SetActive(true);
        }

        private void HideSkull() =>
            _skull.SetActive(false);

        private void SpawnPickupFx() =>
            Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity, transform);

        private void UpdateHeroWallet(HeroWallet heroWallet) =>
            heroWallet.Collect(_lootData.Money.Amount);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
        

        private void UpdateWorldData(NonPickedUpLoot savedLoot)
        {
            savedLoot.Add(
                Id,
                new LootPieceData(_lootData, transform.position.AsVector3Data())
            );
        }
        
    }
}