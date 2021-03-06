﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Http;
public class GameManager : MonoBehaviour
{

    private string statSaveURL = "https://statchomper.herokuapp.com/sms-basketball";
    private static readonly HttpClient client = new HttpClient();
    public GameObject gameSummaryPanel;
    public GameObject gameDetailPanel;
    public GameObject errorPanel;
    public GameObject confirmDeletePanel;
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
    private string gameID;
    public Button buttonPrefab;
    public GameObject StackedGames;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        TouchScreenKeyboard.hideInput = true;
        showGamesSummary();
    }

    private string getGameSummaryText(string gameID) {
        string p = PlayerPrefs.GetString($"{gameID}_player");
        string d = PlayerPrefs.GetString($"{gameID}_date");
        string o = PlayerPrefs.GetString($"{gameID}_opponent");
        return $" {p} vs {o}\n {d}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void calculateGameSummary() {
        zeroOutGameSummary();
        char [] statLineArray = statLine.text.ToCharArray();
        for (int i = 0; i < statLineArray.Length; i++) {
            if (statLineArray[i] == '-') {
                if ( (i+1) < statLineArray.Length) {
                    char char1 = statLineArray[i];
                    i++;
                    char char2 = statLineArray[i];
                    processStat($"{char1}{char2}");
                }
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
        threepointers.text = "0 for 0 (0%)";
        twopointers.text = "0 for 0 (0%)";
        freethrows.text = "0 for 0 (0%)";
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

    private string [] getGameIDs(){
        string gamesString = PlayerPrefs.GetString("games");
        if (gamesString.Length < 1) {
            return new string[0];
        } else {
            return gamesString.Split(' ');
        }
    }

    private string getNewGameID(){
        string [] gameIDs = getGameIDs();
        if (gameIDs.Length < 1) {
            return "1";
        } else {
            string lastGameIDStr = gameIDs[gameIDs.Length - 1];
            int lastGameIDInt = Int32.Parse(lastGameIDStr);
            lastGameIDInt++;
            return $"{lastGameIDInt}";
        }
    }

    private bool validGameInfo() {
        if (player.text.Length < 1){
            return false;
        }
        if (date.text.Length < 1){
            return false;
        }
        if (opponent.text.Length < 1){
            return false;
        }
        return true;
    }

    private void showError(string e) {
        errorPanel.SetActive(true);
        errorPanel.GetComponentInChildren<Text>().text = e;
    }

    public void dismissError() {
        errorPanel.SetActive(false);
    }

    public async void submitGame(string statline, string player, string date, string opponent) {
        var body = new Dictionary<string, string>
        {
            { "Body", $"{date}:{player}:{opponent}:{statline}" }
        };
        var bodyEncoded = new FormUrlEncodedContent(body);
        var result = await client.PostAsync(this.statSaveURL, bodyEncoded);
    }

    public void saveGame() {
        if (validGameInfo()){
            // IF THIS IS A NEW GAME, GENERATE AN ID FOR STORAGE ADD TO GAMES LIST
            if (gameID == null) {
                gameID = getNewGameID();
                string gamesString = PlayerPrefs.GetString("games");
                if (gamesString.Length < 1){
                    PlayerPrefs.SetString("games", $"{gameID}");    
                } else {
                    PlayerPrefs.SetString("games", $"{gamesString} {gameID}".Trim());
                }
            }
            PlayerPrefs.SetString($"{gameID}_statLine", statLine.text);
            PlayerPrefs.SetString($"{gameID}_player", player.text);
            PlayerPrefs.SetString($"{gameID}_date", date.text);
            PlayerPrefs.SetString($"{gameID}_opponent", opponent.text);
            gameID = null;
            if ( statLine.text.ToUpper().Contains("H")) {
                submitGame(statLine.text, player.text, date.text, opponent.text);
            }
            showGamesSummary();
        } else {
            showError("You need a player, date and opponent to save a game.");
        }
    }

    public void cancelNewGame () {
        gameID = null;
        showGamesSummary();
    }

    public void loadGame(string id) {
        showBlankGameDetail();
        gameID = id;
        statLine.text = PlayerPrefs.GetString($"{gameID}_statLine");
        player.text = PlayerPrefs.GetString($"{gameID}_player");
        date.text = PlayerPrefs.GetString($"{gameID}_date");
        opponent.text = PlayerPrefs.GetString($"{gameID}_opponent");
    }

    public void confirmDeleteGame() {
        confirmDeletePanel.SetActive(true);
    }

    public void cancelDeleteGame() {
        confirmDeletePanel.SetActive(false);
    }
    public void deleteGame() {
        PlayerPrefs.DeleteKey($"{gameID}_statLine");
        PlayerPrefs.DeleteKey($"{gameID}_player");
        PlayerPrefs.DeleteKey($"{gameID}_date");
        PlayerPrefs.DeleteKey($"{gameID}_opponent");
        string [] gameIDs = getGameIDs();
        string gameString = "";
        for (int i = 0; i < gameIDs.Length; i++) {
            if (gameIDs[i] != gameID) {
                gameString = $"{gameString} {gameIDs[i]}";
            }
        }
        PlayerPrefs.SetString("games", gameString.Trim());
        gameID = null;
        confirmDeletePanel.SetActive(false);
        showGamesSummary();
    }

    public void showGamesSummary() {
        foreach (Transform child in StackedGames.transform)
        {
            Destroy(child.gameObject);
        }
        string [] gameIDs = getGameIDs();
        Array.Reverse(gameIDs);
        for (int i = 0; i < gameIDs.Length; i++){
            Button b = Instantiate(buttonPrefab, StackedGames.transform, false);
            b.GetComponentInChildren<Text>().text = getGameSummaryText(gameIDs[i]);
            string id = gameIDs[i];
            b.onClick.AddListener( () => loadGame(id) );
        }
        gameDetailPanel.SetActive(false);
        gameSummaryPanel.SetActive(true);
    }
    public void showBlankGameDetail() {
        statLine.text = "";
        player.text = "";
        opponent.text = "";
        DateTime today = DateTime.Today;
        date.text = today.ToString("yyyy-MM-dd");
        gameDetailPanel.SetActive(true);
        gameSummaryPanel.SetActive(false);
    }
}
