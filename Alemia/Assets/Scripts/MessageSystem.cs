using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSystem : MonoBehaviour
{
    [Range(0,1)]
    public float emergeSpeed;
    public float awaitTime;
    public float spacing;
    public float offscreenspace;
    public GameObject MessagesZone;
    public GameObject Message;

    private List<GameObject> Messages;

    // Start is called before the first frame update
    void Start()
    {
        Messages = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            StartCoroutine(CreateMessage("+ is pressed"));
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            StartCoroutine(CreateMessage("- is pressed"));
    }

    IEnumerator CreateMessage(string text)
    {
        GameObject newMessage = Instantiate(Message,MessagesZone.transform);
        newMessage.GetComponent<Text>().text = text;
        Messages.Add(newMessage);
        RectTransform T = newMessage.GetComponent<RectTransform>();
        Vector3 pos = T.position;
        Debug.Log($"pos={pos}");
        Vector3 offscreen = new Vector3(offscreenspace, 0, 0);
        T.position -= offscreen;
        Debug.Log($"pos={pos}");

        //emerging loop:
        while (Mathf.Abs((pos-T.position).x) >=  0.1f)
        {
            int order = Messages.Count - Messages.IndexOf(newMessage);
            Vector3 yOffset = Vector3.down * (order - 1) * spacing;
            T.position = Vector3.Lerp(T.position, pos + yOffset, emergeSpeed);
            yield return new WaitForEndOfFrame();
        }
        //wait loop;
        float time = 0;
        while (time < awaitTime)
        {
            int order = Messages.Count - Messages.IndexOf(newMessage);
            Vector3 yOffset = Vector3.down * (order - 1) * spacing;
            T.position = Vector3.Lerp(T.position, pos + yOffset, emergeSpeed);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //quit loop;
        pos = T.position - offscreen;
        while (Mathf.Abs((pos - T.position).x) >= 0.1f)
        {
            T.position = Vector3.Lerp(T.position, pos, emergeSpeed);
            yield return new WaitForEndOfFrame();
        }
        Messages.Remove(newMessage);
        Destroy(newMessage);

    }
}
