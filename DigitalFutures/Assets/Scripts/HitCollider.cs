using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 

 * keep in mind all the input scene you introduced 

Scene 0 - Intro to Dashboard 
Scene 1 - link to "ProjectDashboard" [world]
Scene 2 - link to "Join" [Subscription]

Scene 3 - link to "NationalMuseum" [TestARscene] or "TemplateProjectScene"

 */


public class HitCollider : MonoBehaviour
    {
        void Update()
        {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)&& hit.transform.tag == "News") 
            {
                if (hit.collider != null)
                {

                    Application.OpenURL("https://www.morphosis.com/news");//websitelink news
                }
             }
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "About") 
            {
                if (hit.collider != null)
                {

                    Application.OpenURL("https://www.morphosis.com/about"); //websitelink about 
                }
            }
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Join")
            {
                if (hit.collider != null)
                {
                  
                    SceneManager.LoadScene(2); //authetication/newsletter
                }
            }
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Projects")
            {
                if (hit.collider != null)
                {
                   
                    SceneManager.LoadScene(1); //worlddash
                }
            }
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "NationalMuseum")
            {
                if (hit.collider != null)
                {
                   
                    SceneManager.LoadScene(3); //projectsample
                }
            }
        }
}

}


