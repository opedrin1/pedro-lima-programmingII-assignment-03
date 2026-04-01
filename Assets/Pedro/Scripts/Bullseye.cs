using UnityEngine;

public class Bullseye : MonoBehaviour
{
    [Header("Linked Chest")]
    public CagedChest linkedChest;

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
