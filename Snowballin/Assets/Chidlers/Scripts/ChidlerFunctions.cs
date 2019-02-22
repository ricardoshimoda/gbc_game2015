using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChidlerFunctions : MonoBehaviour
{
    public GameObject[] Boots;
    public GameObject[] Pants;
    public GameObject[] Gloves;
    public GameObject[] Jacket;
    public GameObject Head;
    public GameObject[] Hat;
    public GameObject[] Eyes;
    public GameObject[] Mouth;
    public Material[] Texture;
    public bool israndom;
    public int rando;
    private int texran;

    //this randomizes the texture and items a child is wearing on their spawn, if you don't want it
    //just set israndom to false
    void Start() 
    {
        if (israndom == true)
        {

            texran = Random.Range(0, Texture.Length);
            rando = Random.Range(0, Boots.Length);

            for (int i = 0; i < Boots.Length; i++)
            {
                if (i != rando)
                {
                    Boots[i].SetActive(false);
                }
                else
                {
                    Renderer temp = Boots[i].GetComponent<SkinnedMeshRenderer>();
                    temp.material = Texture[texran];
                }
            }
            rando = Random.Range(0, Pants.Length);
            for (int i = 0; i < Pants.Length; i++)
            {
                if (i != rando)
                {
                    Pants[i].SetActive(false);
                }
                else
                {
                    Renderer temp = Pants[i].GetComponent<SkinnedMeshRenderer>();
                    temp.material = Texture[texran];
                }
            }
            rando = Random.Range(0, Gloves.Length);
            for (int i = 0; i < Gloves.Length; i++)
            {
                if (i != rando)
                {
                    Gloves[i].SetActive(false);
                }
                else
                {
                    Renderer temp = Gloves[i].GetComponent<SkinnedMeshRenderer>();
                    temp.material = Texture[texran];
                }
            }
            rando = Random.Range(0, Jacket.Length);
            for (int i = 0; i < Jacket.Length; i++)
            {
                if (i != rando)
                {
                    Jacket[i].SetActive(false);
                }
                else
                {
                    Renderer temp = Jacket[i].GetComponent<SkinnedMeshRenderer>();
                    temp.material = Texture[texran];
                }
            }
            rando = Random.Range(0, Hat.Length);
            for (int i = 0; i < Hat.Length; i++)
            {
                if (i != rando)
                {
                    Hat[i].SetActive(false);
                }
                else
                {
                    Renderer temp = Hat[i].GetComponent<MeshRenderer>();
                    temp.material = Texture[texran];
                }
            }
            rando = Random.Range(0, Eyes.Length);
            for (int i = 0; i < Eyes.Length; i++)
            {
                if (i != rando)
                {
                    Eyes[i].SetActive(false);
                }
                else
                {
                    Renderer temp = Eyes[i].GetComponent<MeshRenderer>();
                    temp.material = Texture[texran];
                }
            }
            rando = Random.Range(0, Mouth.Length);
            for (int i = 0; i < Mouth.Length; i++)
            {
                if (i != rando)
                {
                    Mouth[i].SetActive(false);
                }
                else
                {
                    Renderer temp = Mouth[i].GetComponent<SkinnedMeshRenderer>();
                    temp.material = Texture[texran];
                }
            }
            Renderer temph = Head.GetComponent<SkinnedMeshRenderer>();
            temph.material = Texture[texran];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
