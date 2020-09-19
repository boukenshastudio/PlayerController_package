using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteration : MonoBehaviour
{
    public GameObject forcusItem;

    void InterationObj(GameObject item)
    {
        forcusItem = item;
        Debug.Log(item.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            InterationObj(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (forcusItem != null)
        {
            forcusItem = null;
        }
    }
}
