using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Tester
{
    public class TestEffects : MonoBehaviour
    {
        public static TestEffects Instance;
        public Text UIText;
        public GameObject[] meteors;

        public Shaking.CameraShake cameraShake;
        int index = 0;
        public GameObject activeeffect;
        void Start()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < meteors.Length; i++)
                {
                    meteors[i].SetActive(false);
                }
             
                  StartCoroutine(cameraShake.Shake(1f, 0.02f));
               
                activeeffect = meteors[index];
                activeeffect.SetActive(true);
                UIText.text = (index + 1).ToString() + ". " + meteors[index].name;
                index++;
                if (index == meteors.Length)
                {
                    index = 0;
                }
            }
        }
    }
}
