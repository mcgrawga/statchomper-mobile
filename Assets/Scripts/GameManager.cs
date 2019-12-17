using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour
{
    public InputField statLine;
    public InputField player;
    public InputField date;
    public InputField opponent;
    public Text points;
    public Text rebounds;
    public Text assists;
    public Text turnovers;
    public Text blocks;
    public Text steals;
    public Text fouls;
    public Text threepointers;
    public Text twopointers;
    public Text freethrows;
    private int threePointersAttempted;
    private int threePointersMade;
    private int twoPointersAttempted;
    private int twoPointersMade;
    private int freeThrowsAttempted;
    private int freeThrowsMade;

    // Start is called before the first frame update
    void Start()
    {
        DateTime today = DateTime.Today;
        date.text = today.ToString("yyyy-MM-dd");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void calculateGameSummary() {
        Debug.Log(statLine.text);
        zeroOutGameSummary();
        char [] statLineArray = statLine.text.ToCharArray();
        for (int i = 0; i < statLineArray.Length; i++) {
            if (statLineArray[i] == '-'){
                char char1 = statLineArray[i];
                i++;
                char char2 = statLineArray[i];
                processStat($"{char1}{char2}");
            } else {
                processStat($"{statLineArray[i]}");
            }
        }
    }

    public void zeroOutGameSummary() {
        points.text = "0";
        rebounds.text = "0";
        assists.text = "0";
        turnovers.text = "0";
        blocks.text = "0";
        steals.text = "0";
        fouls.text = "0";
        threePointersAttempted = 0;
        threePointersMade = 0;
        twoPointersAttempted = 0;
        twoPointersMade = 0;
        freeThrowsAttempted = 0;
        freeThrowsMade = 0;
    }

    public void updateShootingPercentage(int pointVal) {
        if ( pointVal == 1 ) {
            freeThrowsAttempted += 1;
            freeThrowsMade += 1;
            int percent = (int)Math.Round( (double)(100 * freeThrowsMade / freeThrowsAttempted) );
            freethrows.text = $"{freeThrowsMade} for {freeThrowsAttempted} ({percent}%)";
        }
        if ( pointVal == -1 ) {
            freeThrowsAttempted += 1;
            int percent = (int)Math.Round( (double)(100 * freeThrowsMade / freeThrowsAttempted) );
            freethrows.text = $"{freeThrowsMade} for {freeThrowsAttempted} ({percent}%)";
        }
        if ( pointVal == 2 ) {
            twoPointersAttempted += 1;
            twoPointersMade += 1;
            int percent = (int)Math.Round( (double)(100 * twoPointersMade / twoPointersAttempted) );
            twopointers.text = $"{twoPointersMade} for {twoPointersAttempted} ({percent}%)";
        }
        if ( pointVal == -2 ) {
            twoPointersAttempted += 1;
            int percent = (int)Math.Round( (double)(100 * twoPointersMade / twoPointersAttempted) );
            twopointers.text = $"{twoPointersMade} for {twoPointersAttempted} ({percent}%)";
        }
        if ( pointVal == 3 ) {
            threePointersAttempted += 1;
            threePointersMade += 1;
            int percent = (int)Math.Round( (double)(100 * threePointersMade / threePointersAttempted) );
            threepointers.text = $"{threePointersMade} for {threePointersAttempted} ({percent}%)";
        }
        if ( pointVal == -3 ) {
            threePointersAttempted += 1;
            int percent = (int)Math.Round( (double)(100 * threePointersMade / threePointersAttempted) );
            threepointers.text = $"{threePointersMade} for {threePointersAttempted} ({percent}%)";
        }
    }

    public void processStat( string val ){
        if (val == "1" || val == "2" || val == "3") {
            int num = Int32.Parse(points.text);
            num += Int32.Parse(val);
            points.text = $"{num}";
            updateShootingPercentage(Int32.Parse(val));
        }
        if (val == "-1" || val == "-2" || val == "-3") {
            updateShootingPercentage(Int32.Parse(val));
        }
        if (val == "r") {
            int num = Int32.Parse(rebounds.text);
            num += 1;
            rebounds.text = $"{num}";
        }
        if (val == "a") {
            int num = Int32.Parse(assists.text);
            num += 1;
            assists.text = $"{num}";
        }
        if (val == "s") {
            int num = Int32.Parse(steals.text);
            num += 1;
            steals.text = $"{num}";
        }
        if (val == "b") {
            int num = Int32.Parse(blocks.text);
            num += 1;
            blocks.text = $"{num}";
        }
        if (val == "t") {
            int num = Int32.Parse(turnovers.text);
            num += 1;
            turnovers.text = $"{num}";
        }
        if (val == "f") {
            int num = Int32.Parse(fouls.text);
            num += 1;
            fouls.text = $"{num}";
        }
    }

    public void updateStatlineText( string val ) {
        statLine.text = statLine.text + val;
    }
}
