using System.Linq;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField] private PageController[] pageControllers = default;

    public void ShopPage(PageController page)
    {
        CanvasGroup pageGroup;
        foreach (var pageController in pageControllers.Where(x => x != page))
        {
            pageGroup = pageController.PageGroup;
            UITool.State(ref pageGroup, false);
        }
        
        pageGroup = page.PageGroup;
        UITool.State(ref pageGroup, true);
    }
}