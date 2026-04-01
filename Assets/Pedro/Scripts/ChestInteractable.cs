using UnityEngine;
using DG.Tweening;

public class ChestInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private Animator anim;

    private int _isOpenHash;
    private Tween _loopTween;
    private Tween _collectTween;

    
    void Start()
    {
        if (!anim) return;

        _isOpenHash = Animator.StringToHash("IsOpen");

        transform.DOScale(1.1f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    public void OnHoverIn()
    {
        anim?.SetBool(_isOpenHash, true);
        
        Toast.Instance.ShowToast("[E] Interact");
    }
    
    public void OnInteract()
    {
        CagedChest cagedChest = GetComponent<CagedChest>();
        if (cagedChest != null && cagedChest.isLocked)
        {
            return;
        }
        
        GameUI.Instance.RegisterChestCollected();

        _collectTween = transform.DOScale(0, .5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    void OnDestroy()
    {
        DOTween.Kill(this.gameObject);
    }

    public void OnHoverOff()
    {
        anim?.SetBool(_isOpenHash, false);
        
        Toast.Instance.HideToast();
    }
}