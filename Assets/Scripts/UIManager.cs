using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Ce code est uniquement crée à but de prototypage 
    /// Certaine accesibilité de propriétés et de méthodes de celui-ci sont à revoir
    /// De plus de sa "proprété" sémantique et structurelle 
    /// </summary>
    /// <remarks>
    /// Cette classe est le moteur principal de l'UI de ce prototype
    /// </remarks>

    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField] private RectTransform UI_playerRtTransform;

    [SerializeField] private Image UI_upForce;
    [SerializeField] private Image UI_downForce;
    [SerializeField] private Image UI_rightForce;
    [SerializeField] private Image UI_leftForce;
    [SerializeField] private Image UI_frontForce;
    [SerializeField] private Image UI_backForce;

    [SerializeField] private Image UI_jetPackFuel;
    [SerializeField] private Text UI_jetPackFuelText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    public void SetPlayerRotationTransformFeedback(Vector3 playerRotation)
    {
        UI_playerRtTransform.rotation = Quaternion.Euler(playerRotation.x, 0, playerRotation.z);
    }

    public void SetUpForce(float force)
    {
        UI_upForce.fillAmount = force;
    }
    public void SetDownForce(float force)
    {
        UI_downForce.fillAmount = force;
    }
    public void SetRightForce(float force)
    {
        UI_rightForce.fillAmount = force;
    }
    public void SetLeftForce(float force)
    {
        UI_leftForce.fillAmount = force;
    }
    public void SetFrontForce(float force)
    {
        UI_frontForce.fillAmount = force;
    }
    public void SetBackForce(float force)
    {
        UI_backForce.fillAmount = force;
    }

    public void SetJetPackFuel(float fuelAmount)
    {
        UI_jetPackFuel.fillAmount = fuelAmount;
        UI_jetPackFuelText.text = (fuelAmount * 100).ToString("###");
    }

}
