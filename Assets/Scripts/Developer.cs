using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Developer : MonoBehaviour
{
    public TMP_InputField swordDamage;
    public TMP_InputField shieldMultip;
    public TMP_InputField WultDamage;
    public TMP_InputField cdSword;
    public TMP_InputField cdWult;

    public TMP_InputField arrowDamage;
    public TMP_InputField vanishMultip;
    public TMP_InputField RultDamage;
    public TMP_InputField cdArrow;
    public TMP_InputField cdVanish;
    public TMP_InputField cdRult;

    public GameObject developerPanel;
    public GameObject Warrior;
    public GameObject Ranger;

    private WarriorClass ScriptW;
    private RangerClass ScriptR;
    //ints up to index 3 (incl)
    string[] prefs = {"SwordDmg","WultDmg", "ArrowDmg", "RultDmg", 
        "ShieldMult", "cdSword", "cdWult", "VanishMult",  "cdArrow", "cdVanish", "cdRult" };
    TMP_InputField[] fields;
    // Start is called before the first frame update
    void Start()
    {
        ScriptW = Warrior.GetComponent<WarriorClass>();
        ScriptR = Ranger.GetComponent<RangerClass>();
        fields = new TMP_InputField[] {swordDamage, WultDamage, arrowDamage, RultDamage,
            shieldMultip, cdSword, cdWult, vanishMultip,  cdArrow, cdVanish, cdRult };
        /*
        PlayerPrefs.SetInt("SwordDmg", int.Parse(swordDamage.text));
        PlayerPrefs.SetFloat("ShieldMult", float.Parse(shieldMultip.text));
        PlayerPrefs.SetInt("WultDmg", int.Parse(WultDamage.text));
        PlayerPrefs.SetFloat("cdSword", float.Parse(cdSword.text));
        PlayerPrefs.SetFloat("cdWult", float.Parse(cdWult.text));

        PlayerPrefs.SetInt("ArrowDmg", int.Parse(arrowDamage.text));
        PlayerPrefs.SetFloat("VanishMult", float.Parse(vanishMultip.text));
        PlayerPrefs.SetInt("RultDmg", int.Parse(RultDamage.text));
        PlayerPrefs.SetFloat("cdArrow", float.Parse(cdArrow.text));
        PlayerPrefs.SetFloat("cdVanish", float.Parse(cdVanish.text));
        PlayerPrefs.SetFloat("cdRult", float.Parse(cdRult.text)); */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.P))
        {
            Time.timeScale = 0;
            developerPanel.SetActive(true);
        }
    }

    public void Continue()
    {
        developerPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Save()
    {
        for (int i = 0; i < prefs.Length; i++)
        {
            if (i<= 3)
            {
                PlayerPrefs.SetInt(prefs[i], int.Parse(fields[i].text));
            }
            else
            {
                PlayerPrefs.SetFloat(prefs[i], float.Parse(fields[i].text));
            }
        }
    }

    private void OnEnable()
    {
        writeFields();

    }

    public void writeFields()
    {
        //fields = new TMP_InputField[] {swordDamage, WultDamage, arrowDamage, RultDamage,
        //shieldMultip, cdSword, cdWult, vanishMultip,  cdArrow, cdVanish, cdRult };
        fields[0].text = ScriptW.swordDamage.ToString();
        fields[1].text = ScriptW.ultDamage.ToString();
        fields[2].text = ScriptR.arrowDamage.ToString();
        fields[3].text = ScriptR.ultDamage.ToString();
        fields[4].text = ScriptW.shieldMultiplier.ToString();
        fields[5].text = ScriptW.cdAtk.ToString();
        fields[6].text = ScriptW.cdUlt.ToString();
        fields[7].text = ScriptR.vanishMultiplier.ToString();
        fields[8].text = ScriptR.cdAtk.ToString();
        fields[9].text = ScriptR.cd2nd.ToString();
        fields[10].text = ScriptR.cdUlt.ToString();
    }
}
