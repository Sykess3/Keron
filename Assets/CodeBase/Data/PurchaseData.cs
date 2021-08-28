using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseData
    {
        public SerializableDictionary<string, BoughtIAP> BoughtIAPs = new SerializableDictionary<string, BoughtIAP>();
        public event Action Changed;
        
        public void AddIAP(string id)
        {

            if (BoughtIAPs.Contains(id))
                CreateBoughtIAP(with: id);
            else
                BoughtIAPs[id].Count++;
            
            Changed?.Invoke();
        }

        private void CreateBoughtIAP(string with)
        {
            BoughtIAPs.Add(
                with, 
                new BoughtIAP() {Id = with, Count = 1});
        }
    }
}