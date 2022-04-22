using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;

    public float tension;
    public float smoothness;

    // Update is called once per frame
    void Update()
    {
        Camera camera = GetComponent<Camera>();
        Vector2 position = player.position;
        Vector2 cursor = camera.ScreenToWorldPoint(Input.mousePosition);

        float posx = Mathf.Lerp(transform.position.x, Mathf.Lerp(position.x, cursor.x, tension), smoothness);
        float posy = Mathf.Lerp(transform.position.y, Mathf.Lerp(position.y, cursor.y, tension), smoothness);

        transform.position = new Vector3(posx, posy, transform.position.z);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            camera.orthographicSize--;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            camera.orthographicSize++;
        }
    }
}
