using UnityEngine;

public class Coin : MonoBehaviour, IObjectPool<Coin>
{
    public int poolID { get; set; }
    public ObjectPooler<Coin> pool { get; set; }


}
