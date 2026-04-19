using UnityEngine;

public class Bullseye : MonoBehaviour
{
    [Header("Caged Chest")]
    public CagedChest linkedChest;

    void Start()
    {
        if (linkedChest == null)
        {
            Debug.LogError("No chest linked in the inspector");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Arrow")) return;

        TriggerBullseye(collision.contacts[0].point);
    }
    
    void TriggerBullseye(Vector3 hitPoint)
    {
        if (linkedChest != null)
            linkedChest.UnlockChest();
    }
}
