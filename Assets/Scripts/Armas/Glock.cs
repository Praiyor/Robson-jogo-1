using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glock : MonoBehaviour
{
    public Text textoMunicao;
    public GameObject imgCursor;

    private Animator anim;
    private bool estahAtirando;
    private RaycastHit hit;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
    public GameObject faisca;
    private AudioSource somTiro;
    private int carregador = 3;
    private int municao = 17;
    public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        estahAtirando = false;
        anim = GetComponent<Animator>();
        somTiro = GetComponent<AudioSource>();
        AtualizarTextoMunicao();
    }

    // Update is called once per frame
    void Update()
    {

        if (anim.GetBool("acaoOcorrendo"))
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!estahAtirando && municao > 0)
            {
                somTiro.clip = clips[0];
                municao--;
                estahAtirando = true;
                StartCoroutine(Atirando());

            }else
            {
                if (!estahAtirando && municao == 0 && carregador > 0)
                {
                    Recarregar();
                }
                else
                {
                    somTiro.clip = clips[2];
                    somTiro.time = 0;
                    somTiro.Play();
                }
            }
        } else
        {
            if (Input.GetButtonDown("Recarregar"))
            {
                if (carregador > 0 && municao < 17)
                {
                    Recarregar();
                }
                else
                {
                    somTiro.clip = clips[2];
                    somTiro.time = 0;
                    somTiro.Play();
                }
            }
        }

        AtualizarTextoMunicao();
        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("mirar", true);
            imgCursor.SetActive(false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
        }
        else
        {
            anim.SetBool("mirar", false);
            imgCursor.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
        }
    }

    IEnumerator Atirando()
    {

        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
        anim.Play("AtirarGlock");
        somTiro.time = 0;
        somTiro.Play();

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        int layerMask = ~LayerMask.GetMask("Arma");

        GameObject faiscaObj = null;
        if (Physics.SphereCast(ray, 0.1f, out hit, 100f, layerMask))
        {
            faiscaObj = Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            if(hit.transform.tag == "Arrastar")
            {
                Vector3 direcaoBala = ray.direction;
                hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
            }else
            {
                if(hit.transform.tag == "LevarDano")
                {
                    ILevarDano levarDano = hit.transform.GetComponent<ILevarDano>();
                    levarDano.LevarDano(10);
                }
            }
        }

            yield return new WaitForSeconds(0.3f);
        Destroy(efeitoTiroObj);
        Destroy(faiscaObj);

        estahAtirando = false;
    }

    private void Recarregar()
    {
        somTiro.clip = clips[1];
        somTiro.time = 1.05f;
        somTiro.Play();

        anim.Play("RecarregarGlock");
        municao = 17;
        carregador--;
    }

    private void AtualizarTextoMunicao()
    {
        textoMunicao.text = municao.ToString() + "/" + carregador.ToString();
    }

    public void AddCarregador()
    {
        carregador++;
        AtualizarTextoMunicao();
        somTiro.clip = clips[3];
        somTiro.time = 0;
        somTiro.Play();
    }
}
