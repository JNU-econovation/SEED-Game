using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shaking
{
    public class CameraShake : MonoBehaviour
    {
        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 orignalPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.position = orignalPosition + new Vector3(x, y, -0.1f);
                elapsed += Time.deltaTime;
                yield return 0;
            }
            transform.position = orignalPosition;
            if (Tester.TestEffects.Instance.activeeffect != null && Tester.TestEffects.Instance.activeeffect.activeSelf == true)
                StartCoroutine(Shake(1f, 0.02f));
        }
    }
}
