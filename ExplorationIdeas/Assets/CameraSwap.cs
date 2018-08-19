using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour {


    private bool isFirst;

    public Camera firstCamera;
    public Camera secondCamera;

	void Start()
	{
        isFirst = false;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isFirst = !isFirst;
            firstCamera.gameObject.SetActive(isFirst);
            secondCamera.gameObject.SetActive(!isFirst);
        }
    }

}
