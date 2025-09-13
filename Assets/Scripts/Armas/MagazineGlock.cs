using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineGlock : MonoBehaviour, IPegavel
{
    public void Pegar()
    {
        Glock g = GameObject.FindWithTag("Arma").GetComponent<Glock>();
        g.AddCarregador();
    }

}
