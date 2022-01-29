using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using Doozy.Engine.Nody;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks {
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] TMP_InputField playerNameInput;

    private Coroutine startGame;

    void Awake() {
        Instance = this;
    }

    void Start() {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby() {
        //sMenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
        this.setPlayerNickName(false);
    }

    public void CreateRoom()
    {
        PhotonNetwork.OfflineMode = false;
        if (string.IsNullOrEmpty(roomNameInputField.text)) { return; }

        var roomOptions = new RoomOptions {MaxPlayers = 10};
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
        //MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.OfflineMode = false;
        this.setPlayerNickName(true);
        //MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++) {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        errorText.text = "Room Creation Failed: " + message;
        Debug.LogError("Room Creation Failed: " + message);
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame() {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(2);
    }
    public void StartLocalGame() { 
      //  PhotonNetwork.CurrentRoom.IsOpen = false;
      // PhotonNetwork.CurrentRoom.IsVisible = false;
      if (startGame == null) {
          startGame = StartCoroutine(StartLocalGameAsync());

      }
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        //MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info) {
        PhotonNetwork.JoinRoom(info.Name);
        FindObjectOfType<GraphController>().GoToNodeByName("Room Menu");
        //MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom() {
        //MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach (Transform trans in roomListContent) {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList) { continue; }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        this.incrementeNickName(newPlayer);
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void setPlayerNickName(bool checkPlayersNickName = false){
        string playerName = PhotonNetwork.NickName;
        // Init playerName
        if (playerName == "")
        {
            var defaultName = "Player_" + Random.Range(0, 1000).ToString("0000");
            playerName = PlayerPrefs.GetString("playerName", defaultName);
        }
        if (this.playerNameInput != null){
            if (this.playerNameInput.text == "")
            {
                this.playerNameInput.text = playerName;
            }
            else
            {
                playerName = this.playerNameInput.text;
            }
        }

        // Check nickname duplicate
        if (checkPlayersNickName)
        {
            playerName = this.incrementeNickName(PhotonNetwork.LocalPlayer);
        }

        PhotonNetwork.NickName = playerName;
        PlayerPrefs.SetString("playerName", playerName);
    }

    private string incrementeNickName (Player player) {
        string playerName = player.NickName;

        Player[] players = PhotonNetwork.PlayerList;
        string newName = playerName;
        int increment = 0;
		do { 
            bool isDuplicate = false;
            foreach(Player currentPlayer in players) {
                if(currentPlayer != player && currentPlayer.NickName == newName) {
                    increment++;
                    newName = playerName + "_" + increment;
                    isDuplicate = true;
                    break;
				}
			}
            if(!isDuplicate && increment > 0) {
                playerName += "_" + increment;
                increment = 0; //exit do...while
			}
        } while (increment != 0);

        player.NickName = playerName;
        return playerName;
    }
    
    IEnumerator Connect()

    {

        PhotonNetwork.ConnectUsingSettings();

        while (!PhotonNetwork.IsConnected)

        {

            yield return null;

        }

        PhotonNetwork.OfflineMode = false;

    }
    
    IEnumerator StartLocalGameAsync()

    {

        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)

        {

            yield return null;

        }

        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.LoadLevel(2);

    }
}