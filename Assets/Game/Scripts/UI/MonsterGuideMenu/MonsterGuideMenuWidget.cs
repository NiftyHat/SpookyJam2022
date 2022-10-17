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

    private int currentIndex = 0; 

    void Start()
    {
        DisplayPages();
    }

    public void DisplayPages()
    {
        //if (currentPage == 0)
        //{
        //    //Table of Contents
        //}
        if (currentIndex < data.Count && currentIndex >= 0)
        {
            leftPage.SetData(data[currentIndex]);
        }
        leftPage.SetDisplay(currentIndex >= 0);


        if (currentIndex + 1 < data.Count)
        {
            rightPage.SetData(data[currentIndex + 1]);
        }
        rightPage.SetDisplay(currentIndex + 1 < data.Count);

        nextButton.SetActive(currentIndex + 2 < data.Count);
        previousButton.SetActive(currentIndex > 0);
    }

    public void NextPage()
    {
        currentIndex += 2;

        DisplayPages();

    }

    public void PreviousPage()
    {
        if (currentIndex > 0)
            currentIndex -= 2;

        DisplayPages();
    }
}
