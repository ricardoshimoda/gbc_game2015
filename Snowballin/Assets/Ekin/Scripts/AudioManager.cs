using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip fortBuild;
    public AudioClip[] snowballHit;
    public AudioClip snowballMake;
    public AudioClip snowballThrowPlayer;
    public AudioClip snowStep;
    public AudioClip[] snowballThrowBrody;
    public AudioClip[] snowballThrowDylan;
    public AudioClip[] snowballThrowEkin;
    public AudioClip[] snowballThrowJackson;
    public AudioClip[] snowballThrowMichael;
    public AudioClip[] snowballThrowRicardo;
    public AudioClip[] ouchBrody;
    public AudioClip[] ouchDylan;
    public AudioClip[] ouchEkin;
    public AudioClip[] ouchJackson;
    public AudioClip[] ouchMichael;
    public AudioClip[] ouchRicardo;
    public AudioClip[] cryBrody;
    public AudioClip[] cryDylan;
    public AudioClip[] cryEkin;
    public AudioClip[] cryJackson;
    public AudioClip[] cryMichael;
    public AudioClip[] cryRicardo;

    public AudioClip GetSnowballMake()
    {
        return snowballMake;
    }

    public AudioClip GetSnowballThrow()
    {
        return snowballThrowPlayer;
    }

    public AudioClip GetSnowStep()
    {
        return snowStep;
    }

    public AudioClip GetSnowballHit(int index)
    {
        return snowballHit[index];
    }

    public AudioClip GetSnowballThrowEnemy(int index)
    {
        int randomInt;
        if (index == 0)
        {
            randomInt = Random.Range(0, snowballThrowBrody.Length);
            return snowballThrowBrody[randomInt];
        }
        else if (index == 1)
        {
            randomInt = Random.Range(0, snowballThrowDylan.Length);
            return snowballThrowDylan[randomInt];
        }
        else if (index == 2)
        {
            randomInt = Random.Range(0, snowballThrowEkin.Length);
            return snowballThrowEkin[randomInt];
        }
        else if (index == 3)
        {
            randomInt = Random.Range(0, snowballThrowJackson.Length);
            return snowballThrowJackson[randomInt];
        }
        else if (index == 4)
        {
            randomInt = Random.Range(0, snowballThrowMichael.Length);
            return snowballThrowMichael[randomInt];
        }
        else
        {
            randomInt = Random.Range(0, snowballThrowRicardo.Length);
            return snowballThrowRicardo[randomInt];
        }
    }

    public AudioClip GetOuch(int index)
    {
        int randomInt;
        if (index == 0)
        {
            randomInt = Random.Range(0, ouchBrody.Length);
            return ouchBrody[randomInt];
        }
        else if (index == 1)
        {
            randomInt = Random.Range(0, ouchDylan.Length);
            return ouchDylan[randomInt];
        }
        else if (index == 2)
        {
            randomInt = Random.Range(0, ouchEkin.Length);
            return ouchEkin[randomInt];
        }
        else if (index == 3)
        {
            randomInt = Random.Range(0, ouchJackson.Length);
            return ouchJackson[randomInt];
        }
        else if (index == 4)
        {
            randomInt = Random.Range(0, ouchMichael.Length);
            return ouchMichael[randomInt];
        }
        else
        {
            randomInt = Random.Range(0, ouchRicardo.Length);
            return ouchRicardo[randomInt];
        }
    }

    public AudioClip GetCry(int index)
    {
        int randomInt;
        if (index == 0)
        {
            randomInt = Random.Range(0, cryBrody.Length);
            return cryBrody[randomInt];
        }
        else if (index == 1)
        {
            randomInt = Random.Range(0, cryDylan.Length);
            return cryDylan[randomInt];
        }
        else if (index == 2)
        {
            randomInt = Random.Range(0, cryEkin.Length);
            return cryEkin[randomInt];
        }
        else if (index == 3)
        {
            randomInt = Random.Range(0, cryJackson.Length);
            return cryJackson[randomInt];
        }
        else if (index == 4)
        {
            randomInt = Random.Range(0, cryMichael.Length);
            return cryMichael[randomInt];
        }
        else
        {
            randomInt = Random.Range(0, cryRicardo.Length);
            return cryRicardo[randomInt];
        }
    }
}
