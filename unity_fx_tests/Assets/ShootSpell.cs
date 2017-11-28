using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpell : MonoBehaviour {
    public GameObject[] spellPrefabs;
    private int currentIndex = 0;
    public float animationDuration = 1;
    public Transform spellOrigin;
    public GameObject targetGO;
	// Use this for initialization
	void OnEnable()
    {
        FireSpell();
    }

    public void FireSpell()
    {
        if (spellPrefabs.Length == 0) return;

        currentIndex = currentIndex % spellPrefabs.Length;
        GameObject go = spellPrefabs[currentIndex];

        GameObject currentSpell = GameObject.Instantiate(go);

        currentSpell.transform.SetParent(null);

        currentSpell.transform.position = spellOrigin.position;

        StartCoroutine(AnimateProjectile_CR(currentSpell, targetGO, spellOrigin.position));

        currentIndex++;
    }

    IEnumerator AnimateProjectile_CR(GameObject projectile, GameObject target, Vector3 startPos)
    {
        float t = 0.0f;
        Vector3 disp = target.transform.position - startPos;

        while (true)
        {
            if (t > animationDuration)
            {
                GameObject.Destroy(projectile);
                break;
            }

            Vector3 pos = projectile.transform.position;
            pos = startPos + disp * (t / animationDuration);
            projectile.transform.position = pos;
            projectile.transform.forward = disp.normalized;
            t += Time.deltaTime;
            yield return null;
        }
    }
}
