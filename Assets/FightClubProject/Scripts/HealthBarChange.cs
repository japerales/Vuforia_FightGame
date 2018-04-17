using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class HealthBarChange : MonoBehaviour {

    private PlayerController Fighter;
    private Slider slider;
    private float maxHealth;
    private float sliderMaxValue;
    private bool isTracked;
    private Text playerNameText;
    // Use this for initialization
    void Start () {

        slider = GetComponent<Slider>();
        //hacemos esto para flexibilizar.
        /*
         Por ejemplo, si decidimos que un luchador
         tenga más vida que otro, deberiamos tener 
         una forma general de hacer una regla de 3 y que
         la barra de vida siempre esté normalizada entre 0
         y 1 al margen de la health de cada personaje.
         */
        
        slider.value = sliderMaxValue = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        if (isTracked)
        { 
            //esto es una regla de 3 de toda la vida
            slider.value = (sliderMaxValue*Fighter.health)/maxHealth;
            //Debug.Log(Fighter.gameObject.name + ": " + Fighter.health);
        }
    }

    public void setPlayerController(PlayerController pc)
    {              
       Fighter = pc;
       maxHealth = Fighter.health;
       isTracked = true;
    
    }

    public void setPlayerBarName(string name)
    {
        if (GetComponentInChildren<Text>())
        {
            playerNameText = GetComponentInChildren<Text>();
        }
        playerNameText.text = name;
    }
}
