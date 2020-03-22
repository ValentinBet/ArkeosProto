using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField] private Text UI_gravityText;

    [SerializeField] private Image UI_upForce;
    [SerializeField] private Image UI_downForce;
    [SerializeField] private Image UI_rightForce;
    [SerializeField] private Image UI_leftForce;
    [SerializeField] private Image UI_frontForce;
    [SerializeField] private Image UI_backForce;

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
    
    public void SetGravity(float gravity, string astralName)
    {
        UI_gravityText.text = astralName + " -- G : " + gravity + " m/s²";
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

}
