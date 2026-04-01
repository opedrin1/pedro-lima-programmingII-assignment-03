using UnityEngine;

public class CagedChest : MonoBehaviour
{
    [Header("Cage Setup")]
    public GameObject cage;
    public bool isLocked = true;

    private Collider _chestCollider;

    void Start()
    {
        _chestCollider = GetComponent<Collider>();
        SetLocked(true);
    }

    public void UnlockChest()
    {
        isLocked = false;
        SetLocked(false);

        if (cage != null)
            Destroy(cage);
    }

    void SetLocked(bool locked)
    {
        // for locked, collider must be trigger not checked
        isLocked = locked;
    }
}
