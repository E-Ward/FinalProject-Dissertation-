using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov_Damage_Over_Time : MonoBehaviour
{

    public Modular_3D_Player_Controller PlayerControllerScript;

    public float totalTime = 0;
    public float fireStayTime = 0;
    public int fireEffectLast = 20;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        fireStayTime += Time.deltaTime;

        if (fireStayTime >=  20)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider player)
    {

        totalTime += Time.deltaTime;
        
        if (player.gameObject.tag == "Player")
        {
            if (totalTime > 1)
            {
                PlayerControllerScript.Health -= PlayerControllerScript.fireDamage;
                totalTime = 0;
            }
        }
    }
}
