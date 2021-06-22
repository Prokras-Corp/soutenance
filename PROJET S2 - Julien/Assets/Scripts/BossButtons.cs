using UnityEngine;

public class BossButtons : MonoBehaviour
{
    [SerializeField] GameObject bu1;
    [SerializeField] GameObject bu2;
    [SerializeField] GameObject bu3;
    [SerializeField] GameObject bu4;

    [SerializeField] GameObject Boss;

    bool b1;
    bool b2;
    bool b3;
    bool b4;

    // Update is called once per frame
    void Update()
    {
        b1 = bu1.GetComponent<ButtonBoss>().isActive;
        b2 = bu2.GetComponent<ButtonBoss>().isActive;
        b3 = bu3.GetComponent<ButtonBoss>().isActive;
        b4 = bu4.GetComponent<ButtonBoss>().isActive;

        if (b1 && b2 && b3 && b4)
            EndBoss();
    }

    void EndBoss()
    {
        Debug.Log("Boss dead");
        Boss.SetActive(false);
    }
}
