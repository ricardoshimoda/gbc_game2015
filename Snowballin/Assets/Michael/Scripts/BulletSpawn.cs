using UnityEngine;
using System.Collections;

public class BulletSpawn : MonoBehaviour {
    
    [SerializeField] float speed;
    [SerializeField] float lifeSpan;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Target")
        {
            this.gameObject.SetActive(false);

        }
    }

}

