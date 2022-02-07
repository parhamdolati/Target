using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using FiroozehGameService.Core;
using FiroozehGameService.Models.BasicApi;
using TMPro;

public class LeaderboardManager : InitManager
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Text leadersPanelErrortxt;
    [SerializeField] private Button easyBtn, normalBtn, hardBtn, exitBtn;
    [SerializeField] private ScrollRect easyView, normalView, hardView;
    [SerializeField] private GameObject easyTempPlayer, normalTempPlayer, hardTempPlayer;
    private List<GameObject> easyLeadersPlayerList, normalLeadersPlayerList, hardLeadersPlayerList;
    private bool onLeaderLoading;
    
    private void Start()
    {
        Manager.Instance.ConnectToServer += LoginToGameservice;
        easyLeadersPlayerList = new List<GameObject>();
        normalLeadersPlayerList = new List<GameObject>();
        hardLeadersPlayerList = new List<GameObject>();
        onLeaderLoading = false;
        LeaderButtonListener();
        isInited = true;
    }

    void LeaderButtonListener()
    {
        easyBtn.onClick.AddListener(() => ViewEasy());
        normalBtn.onClick.AddListener(() => ViewNormal());
        hardBtn.onClick.AddListener(() => ViewHard());
        exitBtn.onClick.AddListener(() => CloseLeadersboard());
    }

    //bastan panel LeaderBoard
    void CloseLeadersboard()
    {
        //agar load kardan leader tamam shode bashad vaghti leaders ra mibandim list haro pak kon
        if (!onLeaderLoading)
            ClearAllLeadersList();
        
        leadersPanelErrortxt.gameObject.SetActive(false);
        leaderboardPanel.transform.Find("Loading").gameObject.SetActive(false);
        leaderboardPanel.SetActive(false);
        Manager.Instance.PlayEfx("click");
    }
    
    //be game servis vasl mishavim, record haye khod ra ersal mikonim va tamami dade haye leader board haro migirim va set mikonim
    async Task LoginToGameservice()
    {
        switch (Manager.Instance.gameMode)
        {
            case 0:
                ViewEasy();
                break;
            case 1:
                ViewNormal();
                break;
            case 2:
                ViewHard();
                break;
        }
        leaderboardPanel.transform.Find("Loading").gameObject.SetActive(true);
        Debug.Log("mikhaym be server vasl shim");
        try
        {
            //agar karbar chand bar posht sare ham leaders ra baz o baste konad, chandin list ejad mishavad chon chandin darkhast ferestade misgavd
            //baraye hale in moshkel ba boolian va pak kardan list ghabl az baz kardan in moshkel ra hal mikonim
            if (!onLeaderLoading)
            {
                ClearAllLeadersList();
                onLeaderLoading = true;
                //agar az ghabl vasl nashode bashim vals mishavim va nick name ra set mikonim
                if (!GameService.IsAuthenticated())
                {
                    await GameService.LoginOrSignUp.LoginAsGuest();
                    EditUserProfile profile = new EditUserProfile(nickName: PlayerPrefs.GetString("nickName"));
                    await GameService.Player.EditCurrentPlayerProfile(profile);
                }

                #region EasyHandler

                //ersal record be servis
                if (PlayerPrefs.GetInt("easyRecord") > 0)
                    await GameService.Leaderboard.SubmitScore("619f66ca11c8a2001907cfae",
                        PlayerPrefs.GetInt("easyRecord"));
                //daryaft dade haye leaderBoard
                LeaderBoardDetails easyLeaderBoards = await GameService.Leaderboard.GetLeaderBoardDetails(
                    "619f66ca11c8a2001907cfae"
                    , 50);
                //sakhtan list player ha
                foreach (var profile in easyLeaderBoards.Scores)
                {
                    var _player = Instantiate(easyTempPlayer, Vector3.zero, Quaternion.identity);
                    _player.transform.parent = easyTempPlayer.transform.parent;
                    if (profile.Submitter.User.IsMe)
                        _player.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    _player.transform.localScale = new Vector3(.95f, .95f, .95f);
                    _player.SetActive(true);
                    easyLeadersPlayerList.Add(_player);
                    _player.transform.Find("Rank").GetComponent<Text>().text = profile.Rank.ToString();
                    _player.transform.Find("Nickname").GetComponent<TMP_Text>().text = profile.Submitter.Name;
                    _player.transform.Find("Record").GetComponent<Text>().text = profile.Value.ToString();
                }

                #endregion

                #region NormalHandler

                //ersal record be servis
                if (PlayerPrefs.GetInt("normalRecord") > 0)
                    await GameService.Leaderboard.SubmitScore("619f671211c8a2001907cfaf",
                        PlayerPrefs.GetInt("normalRecord"));
                //daryaft dade haye leaderBoard
                LeaderBoardDetails normalLeaderBoards = await GameService.Leaderboard.GetLeaderBoardDetails(
                    "619f671211c8a2001907cfaf"
                    , 50);
                //sakhtan list player ha
                foreach (var profile in normalLeaderBoards.Scores)
                {
                    var _player = Instantiate(normalTempPlayer, Vector3.zero, Quaternion.identity);
                    _player.transform.parent = normalTempPlayer.transform.parent;
                    if (profile.Submitter.User.IsMe)
                        _player.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    _player.transform.localScale = new Vector3(.95f, .95f, .95f);
                    _player.SetActive(true);
                    normalLeadersPlayerList.Add(_player);
                    _player.transform.Find("Rank").GetComponent<Text>().text = profile.Rank.ToString();
                    _player.transform.Find("Nickname").GetComponent<TMP_Text>().text = profile.Submitter.Name;
                    _player.transform.Find("Record").GetComponent<Text>().text = profile.Value.ToString();
                }

                #endregion

                #region HardHandler

                //ersal record be servis
                if (PlayerPrefs.GetInt("hardRecord") > 0)
                    await GameService.Leaderboard.SubmitScore("619f673111c8a2001907cfb0",
                        PlayerPrefs.GetInt("hardRecord"));
                //daryaft dade haye leaderBoard
                LeaderBoardDetails hardLeaderBoards = await GameService.Leaderboard.GetLeaderBoardDetails(
                    "619f673111c8a2001907cfb0"
                    , 50);
                //sakhtan list player ha
                foreach (var profile in hardLeaderBoards.Scores)
                {
                    var _player = Instantiate(hardTempPlayer, Vector3.zero, Quaternion.identity);
                    _player.transform.parent = hardTempPlayer.transform.parent;
                    if (profile.Submitter.User.IsMe)
                        _player.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                    _player.transform.localScale = new Vector3(.95f, .95f, .95f);
                    _player.SetActive(true);
                    hardLeadersPlayerList.Add(_player);
                    _player.transform.Find("Rank").GetComponent<Text>().text = profile.Rank.ToString();
                    _player.transform.Find("Nickname").GetComponent<TMP_Text>().text = profile.Submitter.Name;
                    _player.transform.Find("Record").GetComponent<Text>().text = profile.Value.ToString();
                }

                #endregion

                leaderboardPanel.transform.Find("Loading").gameObject.SetActive(false);
                leadersPanelErrortxt.gameObject.SetActive(false);
                onLeaderLoading = false;
            }
        }
        
        catch (Exception e)
        {
            leadersPanelErrortxt.gameObject.SetActive(true);
            leaderboardPanel.transform.Find("Loading").gameObject.SetActive(false);
            leadersPanelErrortxt.text = "Opss!\nPlease turn on internet and re-open leaders panel\n(turn off your vpn!)";
            onLeaderLoading = false;
        }
    }

    //namayesh tab easyMode
    void ViewEasy()
    {
        easyBtn.GetComponent<Image>().color = new Color32(27, 38, 46, 255);
        normalBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        hardBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        
        easyView.gameObject.SetActive(true);
        normalView.gameObject.SetActive(false);
        hardView.gameObject.SetActive(false);
    }

    //namayesh tab normalMode
    void ViewNormal()
    {
        easyBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        normalBtn.GetComponent<Image>().color = new Color32(27, 38, 46, 255);
        hardBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        
        easyView.gameObject.SetActive(false);
        normalView.gameObject.SetActive(true);
        hardView.gameObject.SetActive(false);
    }

    //namayesh tab HardMode
    void ViewHard()
    {
        easyBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        normalBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        hardBtn.GetComponent<Image>().color = new Color32(27, 38, 46, 255);
        
        easyView.gameObject.SetActive(false);
        normalView.gameObject.SetActive(false);
        hardView.gameObject.SetActive(true);
    }

    void ClearAllLeadersList()
    {
        foreach (GameObject player in easyLeadersPlayerList)
            Destroy(player);
        foreach (GameObject player in normalLeadersPlayerList)
            Destroy(player);
        foreach (GameObject player in hardLeadersPlayerList)
            Destroy(player);
        easyLeadersPlayerList.Clear();
        normalLeadersPlayerList.Clear();
        hardLeadersPlayerList.Clear();
    }
}
