using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ResponseButton is the behaviour associated with the ResponseButtons
public class ResponseButton : MonoBehaviour {

    private DialogDemo demo;
    private DialogResponse resp;
    private Button b;
    private GameObject g;

    public void Setup(DialogDemo demo, DialogResponse resp, Transform c, float y, string txt)
    {
        this.demo = demo;
        this.resp = resp;
        this.b = GetComponent<Button>();
        this.g = gameObject;


        this.b.onClick.AddListener(() => this.demo.ChoseResponse(resp));
        this.b.GetComponentInChildren<Text>().text = txt;
        this.b.transform.SetParent(c);
        this.b.transform.position = new Vector3(500f, y, 0f);
    }

    private void OnDestroy()
    {
        if (this.resp != null)
        {
            Debug.Log("destroying button " + this.resp.GetKeyPhrase());
        }

        Destroy(g);
    }

}
