using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using FiroozehGameService.Models.BasicApi;
using FiroozehGameService.Models.GSLive;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class LeadersHandler : MonoBehaviour
{
    [SerializeField] private SoundHandler _soundHandler;
    public Text leadersPanelErrortxt;
    public Button easyBtn, normalBtn, hardBtn;
    public ScrollRect easyView, normalView, hardView;
    public GameObject easyTempPlayer, normalTempPlayer, hardTempPlayer;
    private List<GameObject> easyLeadersPlayerList, normalLeadersPlayerList, hardLeadersPlayerList;
    
    
    private void Awake()
    {
        easyLeadersPlayerList = new List<GameObject>();
        normalLeadersPlayerList = new List<GameObject>();
        hardLeadersPlayerList = new List<GameObject>();
    }

    //be game servis vasl mishavim, record haye khod ra ersal mikonim va tamami dade haye leader board haro migirim va set mikonim
    async Task LoginToGameservice()
    {
        ViewEasy();
        transform.Find("Loading").gameObject.SetActive(true);
        try
        {
            //agar az ghabl vasl nashode bashim vals mishavim va nick name ra set mikonim
            if (!GameService.IsAuthenticated())
            {
                await GameService.LoginOrSignUp.LoginAsGuest();
                EditUserProfile profile = new EditUserProfile(nickName: PlayerPrefs.GetString("nickName"));
                await GameService.Player.EditCurrentPlayerProfile(profile);
            }

            #region EasyHandler
            
            //ersal record be servis
            if(PlayerPrefs.GetInt("easyRecord") > 0)
                await GameService.Leaderboard.SubmitScore("619f66ca11c8a2001907cfae", PlayerPrefs.GetInt("easyRecord"));
            //daryaft dade haye leaderBoard
            LeaderBoardDetails easyLeaderBoards = await GameService.Leaderboard.GetLeaderBoardDetails("619f66ca11c8a2001907cfae"
                ,50);
            //sakhtan list player ha
            foreach (var profile in easyLeaderBoards.Scores)
            {
                var _player = Instantiate(easyTempPlayer, Vector3.zero, quaternion.identity);
                _player.transform.parent = easyTempPlayer.transform.parent;
                if (profile.Submitter.User.IsMe)
                    _player.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                _player.SetActive(true);
                easyLeadersPlayerList.Add(_player);
                _player.transform.Find("Rank").GetComponent<Text>().text = profile.Rank.ToString();
                _player.transform.Find("Nickname").GetComponent<TMP_Text>().text = profile.Submitter.Name;
                _player.transform.Find("Record").GetComponent<Text>().text = profile.Value.ToString();
            }

            #endregion
            
            #region NormalHandler
            
            //ersal record be servis
            if(PlayerPrefs.GetInt("normalRecord") > 0)
                await GameService.Leaderboard.SubmitScore("619f671211c8a2001907cfaf", PlayerPrefs.GetInt("normalRecord"));
            //daryaft dade haye leaderBoard
            LeaderBoardDetails normalLeaderBoards = await GameService.Leaderboard.GetLeaderBoardDetails("619f671211c8a2001907cfaf"
                ,50);
            //sakhtan list player ha
            foreach (var profile in normalLeaderBoards.Scores)
            {
                var _player = Instantiate(normalTempPlayer, Vector3.zero, quaternion.identity);
                _player.transform.parent = normalTempPlayer.transform.parent;
                if (profile.Submitter.User.IsMe)
                    _player.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                _player.SetActive(true);
                normalLeadersPlayerList.Add(_player);
                _player.transform.Find("Rank").GetComponent<Text>().text = profile.Rank.ToString();
                _player.transform.Find("Nickname").GetComponent<TMP_Text>().text = profile.Submitter.Name;
                _player.transform.Find("Record").GetComponent<Text>().text = profile.Value.ToString();
            }

            #endregion
            
            #region NormalHandler
            
            //ersal record be servis
            if(PlayerPrefs.GetInt("hardRecord") > 0)
                await GameService.Leaderboard.SubmitScore("619f673111c8a2001907cfb0", PlayerPrefs.GetInt("hardRecord"));
            //daryaft dade haye leaderBoard
            LeaderBoardDetails hardLeaderBoards = await GameService.Leaderboard.GetLeaderBoardDetails("619f673111c8a2001907cfb0"
                ,50);
            //sakhtan list player ha
            foreach (var profile in hardLeaderBoards.Scores)
            {
                var _player = Instantiate(hardTempPlayer, Vector3.zero, quaternion.identity);
                _player.transform.parent = hardTempPlayer.transform.parent;
                if (profile.Submitter.User.IsMe)
                    _player.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                _player.SetActive(true);
                hardLeadersPlayerList.Add(_player);
                _player.transform.Find("Rank").GetComponent<Text>().text = profile.Rank.ToString();
                _player.transform.Find("Nickname").GetComponent<TMP_Text>().text = profile.Submitter.Name;
                _player.transform.Find("Record").GetComponent<Text>().text = profile.Value.ToString();
            }

            #endregion
            
            transform.Find("Loading").gameObject.SetActive(false);
        }
        
        catch (Exception e)
        {
            leadersPanelErrortxt.gameObject.SetActive(true);
            transform.Find("Loading").gameObject.SetActive(false);
            if (e is GameServiceException)
            { 
                leadersPanelErrortxt.text = "Game Server Error: " + e.Message;
            }
            else
            {
                leadersPanelErrortxt.text = e.Message;
            }
        }
    }

    //namayesh tab easyMode
    public void ViewEasy()
    {
        easyBtn.GetComponent<Image>().color = new Color32(27, 38, 46, 255);
        normalBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        hardBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        
        easyView.gameObject.SetActive(true);
        normalView.gameObject.SetActive(false);
        hardView.gameObject.SetActive(false);
    }

    //namayesh tab normalMode
    public void ViewNormal()
    {
        easyBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        normalBtn.GetComponent<Image>().color = new Color32(27, 38, 46, 255);
        hardBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        
        easyView.gameObject.SetActive(false);
        normalView.gameObject.SetActive(true);
        hardView.gameObject.SetActive(false);
    }

    //namayesh tab HardMode
    public void ViewHard()
    {
        easyBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        normalBtn.GetComponent<Image>().color = new Color32(29, 59, 84, 255);
        hardBtn.GetComponent<Image>().color = new Color32(27, 38, 46, 255);
        
        easyView.gameObject.SetActive(false);
        normalView.gameObject.SetActive(false);
        hardView.gameObject.SetActive(true);
    }

    //bazkardan panel leaderBoard
    public void OpenLeadersboard()
    {
        gameObject.SetActive(true);
        _soundHandler.PlayEfx("click");
        LoginToGameservice();
    }
    
    //bastan panel LeaderBoard
    public void CloseLeadersboard()
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
        leadersPanelErrortxt.gameObject.SetActive(false);
        transform.Find("Loading").gameObject.SetActive(false);
        gameObject.SetActive(false);
        _soundHandler.PlayEfx("click");
    }
}
