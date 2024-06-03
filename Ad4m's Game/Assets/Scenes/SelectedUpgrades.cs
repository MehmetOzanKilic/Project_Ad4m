using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUpgrades : MonoBehaviour
{
    public static GameObject[] selectedCards = new GameObject[2];
    public static int[] selectedUpgrades = new int[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GiveUpgrades()
    {
        GetNames();
        TurnNamesIntoNumbers();
    }

    public static void print()
    {
        if(selectedCards.Length == 1)
        {
            print(selectedCards[0]);
        }
        else
        {
            print(selectedCards[0]);
            print(selectedCards[1]);
        }
    }
    static string upgrade1="";
    static string upgrade2="";
    private static void GetNames()
    {   
        foreach(char letter in selectedCards[0].name)
        {
            if(char.IsDigit(letter))
            {
                break;
            }
            else
            {
                upgrade1+=letter;
            }
        }
        print(upgrade1);

        foreach(char letter in selectedCards[1].name)
        {
            if(char.IsDigit(letter))
            {
                break;
            }
            else
            {
                upgrade2+=letter;
            }
        }
        print(upgrade2);
    }

    public static void reset()
    {
        selectedCards=new GameObject[2];
        selectedUpgrades=new int[2];
    }

    private static void TurnNamesIntoNumbers()
    {
        switch(upgrade1)
        {
            case ("Arrowshot"):
            selectedUpgrades[0]=0;
            break;
            case ("Fireball"):
            selectedUpgrades[0]=0;
            break;
            case ("Regen"):
            selectedUpgrades[0]=1;
            break;
            case ("HealingAura"):
            selectedUpgrades[0]=1;
            break;
            case ("Explosion"):
            selectedUpgrades[0]=2;
            break;
            case ("Thunderstorm"):
            selectedUpgrades[0]=2;
            break;
            case ("StrengthBoost"):
            selectedUpgrades[0]=3;
            break;
            case ("WeakeningCurse"):
            selectedUpgrades[0]=4;
            break;
            case ("Retaliation"):
            selectedUpgrades[0]=6;
            break;
            case ("LastStand"):
            selectedUpgrades[0]=5;
            break;
            case ("ExplosiveSacrifice"):
            selectedUpgrades[0]=5;
            break;
            
        }

        switch(upgrade2)
        {
            case ("Arrowshot"):
            selectedUpgrades[1]=0;
            break;
            case ("Fireball"):
            selectedUpgrades[1]=0;
            break;
            case ("Regen"):
            selectedUpgrades[1]=1;
            break;
            case ("HealingAura"):
            selectedUpgrades[1]=1;
            break;
            case ("Explosion"):
            selectedUpgrades[1]=2;
            break;
            case ("Thunderstorm"):
            selectedUpgrades[1]=2;
            break;
            case ("StrengthBoost"):
            selectedUpgrades[1]=3;
            break;
            case ("WeakeningCurse"):
            selectedUpgrades[1]=4;
            break;
            case ("Retaliation"):
            selectedUpgrades[1]=6;
            break;
            case ("LastStand"):
            selectedUpgrades[1]=5;
            break;
            case ("ExplosiveSacrifice"):
            selectedUpgrades[1]=5;
            break;
            
        }

        print(selectedUpgrades[0]);
        print(selectedUpgrades[1]);
    }
}
