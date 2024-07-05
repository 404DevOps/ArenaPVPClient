using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFrameUIHandler : MonoBehaviour
{
    public bool IconRightSide;

    public Transform FrameParent;
    public Transform BarHolder;
    public Transform IconHolder;

    public Image HealthbarImage;
    public Image ManabarImage;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (IconRightSide)
        { 
            IconHolder.SetAsLastSibling();
            var verticalLayoutGrp = BarHolder.GetComponent<VerticalLayoutGroup>();
            verticalLayoutGrp.padding.left = 5;
            verticalLayoutGrp.padding.right = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
