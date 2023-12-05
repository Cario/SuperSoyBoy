using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateScript : MonoBehaviour
{
    delegate void MyDelegate(int num);
    MyDelegate myDelegate;
    // Start is called before the first frame update
    void Start()
    {
        myDelegate = PrintNum;
        myDelegate(50);

        myDelegate = DoubleNum;
        myDelegate(50);
    }
    void PrintNum(int num)
    {
        Debug.Log("Print num: " + num);
    }
    void DoubleNum(int num)
    {
        num *= 2;
        Debug.Log("Double num: " + num);
    }
}
