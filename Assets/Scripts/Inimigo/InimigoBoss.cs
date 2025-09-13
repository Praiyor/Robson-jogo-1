using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoBoss : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaDoAtaque = 2.0f;
    public int vida = 100;
    public AudioClip somMorte;
    public AudioClip somPasso;
    public AudioClip somInicial;
    private AudioSource audioSrc;
    private Boolean viuHeroi = false;
    private FieldOfView fov;
    private PatrulharAleatorio pal;

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();
    }

    // Update is called once per frame
    void Update()
    {

        if (vida <= 0)
        {
            Morrer();
        }

        if (fov.podeVerPlayer)
        {
            VaiAtrasJogador();
        }
        else
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
            agente.isStopped = false;
            pal.Andar();
        }
    }

    private void VaiAtrasJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(!viuHeroi && fov.podeVerPlayer)
        {
            viuHeroi = true;
            audioSrc.PlayOneShot(somInicial);
        }

        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            agente.isStopped = true;

            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigiEntrar();
        }

        if (distanciaDoPlayer >= distanciaDoAtaque + 3)
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
        }
        if (anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    private void CorrigirRigiEntrar()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void LevarDano(int dano)
    {
        vida -= dano;
        if (agente != null && agente.enabled && agente.isOnNavMesh)
        {
            agente.isStopped = true;
        }
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
    }

    private void Morrer()
    {
        audioSrc.clip = somMorte;
        audioSrc.Play();

        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);

        anim.SetBool("morreu", true);

        agente.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        this.enabled = false;

    }

    public void DarDano()
    {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-20);
    }

    public void Passo()
    {
        audioSrc.PlayOneShot(somPasso, 0.05f);
    }
}
