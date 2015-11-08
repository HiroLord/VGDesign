using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    
    public float shakeSpeedMax = 50f;

    private float shakeSpeed = 0f;
    private Vector3 shakeRange = new Vector3(1, 1, 1);
    private Vector3 shake;
    private bool isShake = false;

    Vector3 originalCamPos;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.K))
        {
            CamShake();
        } else { isShake = false; }

        if (isShake)
        {
            shake = SmoothRandom.GetVector3(shakeSpeed--);
            Camera.main.transform.position = originalCamPos + Vector3.Scale(new Vector3(shake.x,shake.y), shakeRange);

            shakeSpeed *= -1;
            shakeRange = new Vector3(shakeRange.x *= -1, shakeRange.y);
        }
    }

    public void CamShake()
    {
        originalCamPos = Camera.main.transform.position;
        shakeSpeed = shakeSpeedMax;
        isShake = true;
    }
}
