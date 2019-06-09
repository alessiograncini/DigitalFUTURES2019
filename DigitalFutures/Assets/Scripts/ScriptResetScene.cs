using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptResetScene : MonoBehaviour
{
    // Start is called before the first frame update
    public Button Reset;


    void Awake()
    {
        Reset = GameObject.FindGameObjectWithTag("Reset").GetComponent<Button>();
        Reset.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        SceneManager.LoadScene("_TestInteract2");
    }
}
