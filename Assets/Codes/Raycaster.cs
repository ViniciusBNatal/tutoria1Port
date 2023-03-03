using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    public TextMesh textDebug;
    public GameObject crosshair;
    float counter=2;
    public FPSWalk fpswalk;

    [SerializeField] private GameObject _interactionButton;
    private RaycastHit _currentTarget;
    private enum HitResult
    {
        InteractableObject,
        SaltRing,
        Irrelevant
    };
    private HitResult _currentHitResult;

    private void Update()
    {
        NewDetectInteraction();
    }
    private void NewDetectInteraction()
    {
        // se o raio q sai da camera bate em alguma coisa
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 6))
        {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //coloca o nome do objeto na frente do raio na saida de debug
            textDebug.text = hit.transform.name;
            //posiciona o crosshair no ponto de impacto do raio
            crosshair.transform.position = hit.point;
            //crosshair.transform.forward = hit.normal;

            //faz o crosshair sempre se alinhar com a camera
            crosshair.transform.forward = -transform.forward;

            _currentHitResult = DefineInteraction(hit.transform);
            //se o objeto tiver tag player (iteragivel)
            if (_currentHitResult == HitResult.InteractableObject || _currentHitResult == HitResult.SaltRing)
            {
                _currentTarget = hit;
                _interactionButton.SetActive(true);
                crosshair.GetComponent<Image>().CrossFadeColor(hit.transform.gameObject.CompareTag("Player") ? Color.green : Color.blue, .5f, false, false);
                counter -= Time.deltaTime;
            } // senao verifica se o objeto é com o tag andavel            
            else
            {
                _interactionButton.SetActive(false);
                //se nao for nada disso reseta o contador
                //pinta o crossrair de vermelho
                crosshair.GetComponent<Image>().CrossFadeColor(Color.red, .5f, false, false);
            }
        }
        else
        {
            _interactionButton.SetActive(false);
            //se nao da raycast o crosshair some
            crosshair.GetComponent<Image>().CrossFadeColor(Color.black, .0f, false, false);
        }
    }

    private HitResult DefineInteraction(Transform target)
    {
        switch (target.tag)
        {
            case "Player":
                return HitResult.InteractableObject;
            case "Walkable":
                return HitResult.SaltRing;
            default:
                return HitResult.Irrelevant;
        }
    }
    public void Interact()
    {
        if (counter < 0)
        {
            if(_currentHitResult == HitResult.InteractableObject)_currentTarget.transform.gameObject.SendMessageUpwards("ButtonAction");
            else if (_currentHitResult == HitResult.SaltRing) fpswalk.positionToGo = _currentTarget.transform.position;
            counter = 3;//reseta o contador
        }
    }
    private void OldDetectInteraction()
    {
        // se o raio q sai da camera bate em alguma coisa
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 6))
        {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //coloca o nome do objeto na frente do raio na saida de debug
            textDebug.text = hit.transform.name;
            //posiciona o crosshair no ponto de impacto do raio
            crosshair.transform.position = hit.point;
            //crosshair.transform.forward = hit.normal;

            //faz o crosshair sempre se alinhar com a camera
            crosshair.transform.forward = -transform.forward;

            //se o objeto tiver tag player (iteragivel)
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                //troca cor do crosshair
                crosshair.GetComponent<Image>().CrossFadeColor(Color.green, .5f, false, false);
                //decrementa o contador 
                counter -= Time.deltaTime;
                //se o contador for < 0 chama a funça no objeto ButtonAction()
                if (counter < 0)
                {
                    hit.transform.gameObject.SendMessageUpwards("ButtonAction");
                    counter = 3;//reseta o contador
                }
            } // senao verifica se o objeto é com o tag andavel
            else if (hit.transform.gameObject.CompareTag("Walkable"))
            {
                crosshair.GetComponent<Image>().CrossFadeColor(Color.blue, .5f, false, false);
                counter -= Time.deltaTime;
                if (counter < 0)
                {
                    //anda com o personagem até o ponto de caminhada
                    fpswalk.positionToGo = hit.transform.position;
                    counter = 3;//reseta o contador
                }
            }
            else
            {
                //se nao for nada disso reseta o contador
                counter = 3;
                //pinta o crossrair de vermelho
                crosshair.GetComponent<Image>().CrossFadeColor(Color.red, .5f, false, false);
            }
        }
        else
        {
            //se nao da raycast o crosshair some
            crosshair.GetComponent<Image>().CrossFadeColor(Color.black, .0f, false, false);
        }
    }
}
