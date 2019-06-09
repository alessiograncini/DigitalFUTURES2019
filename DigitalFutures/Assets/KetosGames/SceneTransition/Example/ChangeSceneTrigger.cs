using UnityEngine;
using KetosGames.SceneTransition;

public class ChangeSceneTrigger : MonoBehaviour
{
    public string ChangeToScene;
    public GameObject WhenTriggeredBy;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == WhenTriggeredBy)
        {
            SceneLoader.LoadScene(ChangeToScene);
        }
    }
}
