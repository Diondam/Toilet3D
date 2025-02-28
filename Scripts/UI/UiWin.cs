using System.Collections;
using DG.Tweening;
using TMPro;
using Unicorn;
using Unicorn.UI;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UiWin : UICanvas
    {
        //[SerializeField] private Button btnCollectGold;
        [SerializeField] private Button btnX3Coin;
        //[SerializeField] private Button btnReplay;
        [SerializeField] private Button btnNextLevel;

        [SerializeField] private Image iconRewardBg;
        [SerializeField] private Image iconReward;
        [SerializeField] private Image imgProcessReward;

        [SerializeField] private TextMeshProUGUI txtProcessReward;
        [SerializeField] private TextMeshProUGUI txtCoin;
        [SerializeField] private TextMeshProUGUI txtCoinBonus;
        [SerializeField] private TextMeshProUGUI txtCoinBonusFree;
        [SerializeField] private TextMeshProUGUI txtCoinReward;

        [SerializeField] private ForceBar forceBar;
        [SerializeField] private DataRewardEndGame dataRewards;
        [SerializeField] private GameObject objFx;

        private int percentReward;
        private int _gold;
        private bool isShowAd;

        public int Gold
        {
            get => _gold;
            protected set => _gold = value;
        }

        private void Start()
        {
            //btnCollectGold.onClick.AddListener(OnClickCollectGold);
            btnX3Coin.onClick.AddListener(OnClickBtnX3Coin);
            //btnReplay.onClick.AddListener(OnClickBtnReplay);
            btnNextLevel.onClick.AddListener(OnClickNextLevel);
        }

        private void OnClickCollectGold()
        {
            SoundManager.Instance.PlayFxSound(SoundManager.GameSound.RewardClick);
            //DisableButton();
            btnX3Coin.interactable = false;
            GameManager.Instance.Profile.AddGold(Gold, "end_game");
            GameManager.Instance.ChangeLevelType();
            GameManager.Instance.LoadLevel();
            //nếu mà thêm hệ thống reward thay đổi skin hoặc gì đó thì mới cần về lobby
            //StartCoroutine(IEGoLobby(true));
        }

        private void OnClickNextLevel()
        {
            SoundManager.Instance.PlayFxSound(SoundManager.GameSound.RewardClick);
            //DisableButton();
            btnNextLevel.interactable = false;
            //thay bằng trừ vàng vào đây
            
            GameManager.Instance.ChangeLevelType();
            GameManager.Instance.LoadLevel();
        }

        // private void OnClickBtnReplay()
        // {
        //     //cho thêm Ads vòa đây
        //     SoundManager.Instance.PlayFxSound(SoundManager.GameSound.RewardClick);
        //     //DisableButton();
        //     btnReplay.interactable = false;
        //     GameManager.Instance.Replay();
        // }

        void DisableButton()
        {
            //btnReplay.interactable = false;
            btnX3Coin.interactable = false;
            btnNextLevel.interactable = false;
            //btnCollectGold.interactable = false;
        }
        private void OnClickBtnX3Coin()
        {
            btnX3Coin.interactable = false;
             if (!isShowAd)
             {
                 OnRewardVideo();
                 return;
             }
            
             UnicornAdManager.ShowAdsReward(OnRewardVideo, Helper.video_reward_x3_gold_end_game);
            // GameManager.Instance.ChangeLevelType();
            // GameManager.Instance.LoadLevel();
        }

        /// <summary>
        /// khi UI win được bật lên thì làm gì
        /// </summary>
        /// <param name="gold"></param>
        public virtual void Init(int gold)
        {
            //btnCollectGold.gameObject.SetActive(true);
            btnX3Coin.gameObject.SetActive(true);
            //btnReplay.gameObject.SetActive(true);
            btnNextLevel.gameObject.SetActive(true);
            
            //btnCollectGold.interactable = true;
            btnX3Coin.interactable = true;
            //btnReplay.interactable = true;
            btnNextLevel.interactable = true;
            
            objFx.SetActive(false);
            CalculateGold(gold);
            GameManager.Instance.Profile.AddGold(Gold, "end_game");
            txtCoin.text = $"COLLECT {Gold}";
            
            // SetupRewardEndGame();
            int lvlShowRate = RocketRemoteConfig.GetIntConfig("config_show_rate_game", 4);
            if (PlayerPrefs.GetInt("showRate", 0) == 0 && GameManager.Instance.DataLevel.DisplayLevel >= lvlShowRate)
            {
                StartCoroutine(IEShowRateGame());
                PlayerPrefs.SetInt("showRate", 1);
            }
            StartCoroutine(IEWaitShowFx());
            
            // isShowAd = !(GameManager.Instance.DataLevel.DisplayLevel <= 1);
            // if (!isShowAd)//<1
            // {
            //     //btnX3Coin.transform.GetChild(0).gameObject.SetActive(false);
            //     btnX3Coin.transform.GetChild(1).gameObject.SetActive(true);
            //     btnCollectGold.gameObject.SetActive(true);
            // }
            // else//>1
            // {
            //     //btnX3Coin.transform.GetChild(0).gameObject.SetActive(true);
            //     btnX3Coin.transform.GetChild(1).gameObject.SetActive(true);
            //     btnCollectGold.gameObject.SetActive(true);
            // }
        }

        /// <summary>
        /// 1 cái set Gold = gold được thay tên :))
        /// </summary>
        /// <param name="gold"></param>
        protected virtual void CalculateGold(int gold)
        {
            Gold = gold;
        }

        private void Update()
        {
            var value = forceBar.GetValue();
            txtCoinBonus.text = $"+{Gold * value}";
            txtCoinBonusFree.text = $"+{Gold * value}";
        }

        private void SetupRewardEndGame()
        {
            txtCoinReward.gameObject.SetActive(false);
            var playerData = GameManager.Instance.PlayerDataManager;
            int indexReward = PlayerDataManager.Instance.GetCurrentIndexRewardEndGame();
            if (indexReward >= dataRewards.Datas.Count)
            {
                indexReward = 0;
                PlayerDataManager.Instance.SetCurrentIndexRewardEndGame(indexReward);
            }

            var reward = dataRewards.Datas[indexReward];
            if (PlayerDataManager.Instance.GetUnlockSkin(reward.Type, reward.Id))
            {
                iconReward.sprite = playerData.DataTexture.IconCoin;
                txtCoinReward.text = $"+{reward.NumberCoinReplace}";
                txtCoinReward.gameObject.SetActive(true);
            }
            else
            {
                iconReward.sprite = playerData.DataTextureSkin.GetIcon(reward.Type, reward.Id);
            }

            iconRewardBg.sprite = iconReward.sprite;
            iconRewardBg.SetNativeSize();
            iconReward.SetNativeSize();

            int process = PlayerDataManager.Instance.GetProcessReceiveRewardEndGame();
            process++;
            PlayerDataManager.Instance.SetProcessReceiveRewardEndGame(process);
            if (process >= reward.NumberWin)
            {
                PlayerDataManager.Instance.SetProcessReceiveRewardEndGame(0);

                StartCoroutine(IEWaitShowRewardEndGame(reward));
            }
            else
            {
                // check show rate game
            }

            float ratio = (float)process / (float)reward.NumberWin;
            imgProcessReward.fillAmount = 0;
            imgProcessReward.DOFillAmount(ratio, 1f);
            percentReward = (int)(ratio * 100);

            iconReward.fillAmount = 0;
            iconReward.DOFillAmount(ratio, 1f);

            SetPercentReward(percentReward);
        }

        private Tweener tweenCoin;
        private int tmpPercent;

        private void SetPercentReward(int percent)
        {
            tweenCoin = tweenCoin ?? DOTween.To(() => tmpPercent, x =>
            {
                tmpPercent = x;
                txtProcessReward.text = string.Format("{0}%", tmpPercent);
            }, percent, 1f).SetAutoKill(false).OnComplete(() =>
            {
                tmpPercent = percentReward;
                txtProcessReward.text = string.Format("{0}%", tmpPercent);
            });

            tweenCoin.ChangeStartValue(tmpPercent);
            tweenCoin.ChangeEndValue(percent);
            tweenCoin.Play();
        }


        private void OnRewardVideo()
        {
            if (!isShow)
                return;
            //
            // btnX3Coin.gameObject.SetActive(false);
            // btnCollectGold.interactable = false;
            forceBar.StopRunning();
            GameManager.Instance.Profile.AddGold(Gold * forceBar.GetValue(), Helper.video_reward_end_game);

            //txtCoin.text = string.Format("+{0}", _gold * 3);
            SoundManager.Instance.PlaySoundReward();

            //StartCoroutine(IEGoLobby(false));
        }

        private IEnumerator IEWaitShowRewardEndGame(RewardEndGame reward)
        {
            yield return new WaitForSeconds(1f);
            GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.END_GAME);
        }

        private IEnumerator IEWaitShowFx()
        {
            yield return new WaitForSeconds(0.5f);
            objFx.SetActive(true);
        }

        private IEnumerator IEShowRateGame()
        {
            yield return new WaitForSeconds(0.5f);
            PopupRateGame.Instance.Show();
        }

        private IEnumerator IEGoLobby(bool isShowInter)
        {
            yield return new WaitForSeconds(0.5f);
            OnBackPressed();

            GameManager.Instance.LoadLobby();
        }
    }
}