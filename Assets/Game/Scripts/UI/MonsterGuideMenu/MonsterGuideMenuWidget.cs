using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGuideMenuWidget : MonoBehaviour
{
    public List<MonsterGuidePageSO> data;
    [Header("Widget UI Objects")]
    [SerializeField] private MonsterGuidePageEntry leftPage;
    [SerializeField] private MonsterGuidePageEntry rightPage;

    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;

    private static int currentPage; 

    void Start()
    {
        
    }

    public void DisplayPages()
    {
        //if (currentPage == 0)
        //{
        //    //Table of Contents
        //}

    }

    public void NextPage()
    {
        currentPage += 2;

        DisplayPages();
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
            currentPage -= 2;

        DisplayPages();
    }
}
