using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulticastScript : MonoBehaviour
{
    delegate void MultiDelegate();
    MultiDelegate myMultiDelegate;
    // Start is called before the first frame update
    void Start()
    {
        myMultiDelegate += PowerUp;
        myMultiDelegate += TurnRed;

        myMultiDelegate();
    }

    void PowerUp()
    {
        print("Orb is powering Up");
    }

    void TurnRed()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
    }

}
