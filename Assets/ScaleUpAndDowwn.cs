using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpAndDowwn : MonoBehaviour
{
    public void HandleScale(bool isON)
        {

        if (isON)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
