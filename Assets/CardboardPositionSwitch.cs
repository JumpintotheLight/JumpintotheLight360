using UnityEngine;
using System.Collections;

public class CardboardPositionSwitch : MonoBehaviour
{
    public static CardboardPositionSwitch instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindGameObjectWithTag("PositionSwitch").GetComponent<CardboardPositionSwitch>();
            return _instance;
        }
    }
    private static CardboardPositionSwitch _instance;

    public GameObject cardboardGameobject;
    public int currentPosition = 0;

    void Awake()
    {
        Reset();
    }

    void Reset()
    {
        SwitchPosition(0);
    }

    public void SwitchPosition()
    {
        currentPosition = (currentPosition+1 >= transform.childCount)? 0 : currentPosition+1;
        cardboardGameobject.transform.position = transform.GetChild(currentPosition).transform.position;
    }

    public void SwitchPosition(int iPosition)
    {
        currentPosition = iPosition;
        if (cardboardGameobject != null)
            cardboardGameobject.transform.position = transform.GetChild(iPosition).transform.position;
    }

}
