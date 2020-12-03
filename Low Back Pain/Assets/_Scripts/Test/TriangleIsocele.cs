using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class TriangleIsocele : MonoBehaviour
{


    // Start is called before the first frame update
    private void Update()
    {
        /*#if UNITY_EDITOR
            Debug.Log(transform.position);
        #endif*/
    }

    private void OnValidate()
    {
        Debug.Log(transform.position);
    }

}
