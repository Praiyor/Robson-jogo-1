using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcoesObjeto : MonoBehaviour
{
    private IdentificarObjeto idObjetos;
    private bool pegou = false;

    // Start is called before the first frame update
    void Start()
    {
        idObjetos = GetComponent<IdentificarObjeto>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && idObjetos.getObjPegar() != null)
        {
            Pegar();
        }

        if(Input.GetKeyDown(KeyCode.F) && idObjetos.getObjArrastar() != null)
        {
            if (!pegou)
            {
                Arrastar();
            } else
            {
                Soltar();
            }
            pegou = !pegou;
        }
    }

    private void Pegar()
    {
        Destroy(idObjetos.getObjPegar());
        idObjetos.EsconderTexto();
    }

    private void Arrastar()
    {
        GameObject obj = idObjetos.getObjArrastar();
        obj.AddComponent<DragDrop>();
        obj.GetComponent<DragDrop>().Ativar();
        idObjetos.enabled = false;
    }

    private void Soltar()
    {
        GameObject obj = idObjetos.getObjArrastar();
        Destroy(obj.GetComponent<DragDrop>());
        idObjetos.enabled = true;
    }
}
