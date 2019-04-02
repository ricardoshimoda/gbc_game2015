using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiSearch : MonoBehaviour
{
    Material m_self;
    Animator anim;
    public bool searching, chasing, smashing;
    [Space]
    public float speed;
    public Transform lineSource, target;
    public GameObject[] fists;


    void Start()
    {
        searching = true;
        Renderer rend = GetComponent<Renderer>();
        m_self = rend.material;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (searching)
        {
            anim.SetBool("searching", true);

            m_self.color = Color.yellow;
            Debug.DrawRay(lineSource.position, lineSource.forward * 10, Color.yellow);
            if (Physics.Raycast(lineSource.position, lineSource.forward, out RaycastHit Hit, 100f))
            {
                if(Hit.transform.CompareTag("Player")|| Hit.transform.CompareTag("Kid"))
                {
                    YetiPrey preyS = Hit.transform.gameObject.GetComponent<YetiPrey>();
                    if (preyS.prey)
                    {
                        Debug.Log("Found someone");
                        searching = false;
                        target = Hit.transform;
                        chasing = true;
                        anim.SetBool("searching", false);
                        preyS.prey = false;
                        return;
                    }
                }
            }
            Debug.Log("Searching now");
        }
        if (chasing)
        {
            searching = false;
            anim.SetBool("running", true);
            m_self.color = Color.red;
            Debug.DrawRay(lineSource.position, lineSource.forward * 10, Color.red, 1f);

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            if(Vector3.Distance(transform.position, target.position) < 3f)
            {
                Debug.Log("In range");
                chasing = false;
                anim.SetBool("running", false);
                smashing = true;
                Invoke("StopSmashing", 3f);
                return;
            }
        }
        if (smashing)
        {
            m_self.color = Color.white;
            anim.SetBool("smashing", true);
            target = null;

            foreach (GameObject go in fists)
            {
                go.transform.RotateAround(transform.position, Vector3.right, 10f);
            }

        }
        else
        {
            m_self.color = Color.white;
        }

        transform.SetPositionAndRotation(new Vector3(transform.position.x, 1.3f, transform.position.z), transform.rotation);
    }

    void StopSmashing()
    {
        Debug.Log("Done Smashing");
        smashing = false;
        anim.SetBool("smashing", false);
        searching = true;
    }
}
