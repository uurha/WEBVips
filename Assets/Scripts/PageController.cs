using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PageController : MonoBehaviour
{
    [SerializeField] private CanvasGroup pageGroup = default;

    public CanvasGroup PageGroup => pageGroup;

    private void Awake()
    {
        pageGroup = GetComponent<CanvasGroup>();
    }
}
