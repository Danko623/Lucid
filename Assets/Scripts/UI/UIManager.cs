using Quest;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Manager Settings")]
        [SerializeField] private TMP_Text notificationText; 
        [SerializeField] private TMP_Text questText;
        [SerializeField] private TMP_Text dialogLineText;
        [SerializeField] private float textDuration = 3.5f;
        
        private float _notificationTimer;
        private bool _isShowingNotification;
        public static UIManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if (!_isShowingNotification) return;

            _notificationTimer -= Time.deltaTime;
            if (_notificationTimer <= 0f)
            {
                HideNotification();
            }
        }

        public void ShowDialog(string text)
        {
            if (!dialogLineText){
                return;
            }
            dialogLineText.text = text;
        }
        
        public void HideDialog()
        {
            if (dialogLineText) dialogLineText.text = string.Empty;
        }

        public void ShowNotification(string text)
        {
            if (!notificationText)return;
            
            notificationText.text = text;
            _notificationTimer = textDuration;
            _isShowingNotification = true;
        }

        private void HideNotification()
        {
            _isShowingNotification = false;
            if (notificationText) notificationText.text = string.Empty;
        }
        
        public void UpdateQuestUI(QuestData quest)
        {
            if (quest == null) return;
            
            if (questText) questText.text = $"<b>{quest.questName}</b>\n{quest.questDescription}";
        }
        
        public void ClearQuestUI()
        {
            if (questText != null)
            {
                questText.text = string.Empty;
            }
        }
        
    }
}