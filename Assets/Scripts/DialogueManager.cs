using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [SerializeField]
    private DOTweenVisualManager _signPanel;
    public DOTweenVisualManager _dialogueBox;
    public Text dialogueText;  // UI Text 컴포넌트
    public float typingSpeed = 0.05f;  // 타이핑 속도
    private string[] sentences;  // 대화 문장 배열
    private int index = 0;  // 현재 문장 인덱스
    private Coroutine typingCoroutine;  // 현재 실행 중인 Coroutine을 관리하기 위한 변수
    private bool isTyping = false;  // 현재 타이핑 중인지 확인하기 위한 변수

    [SerializeField] BoxCollider2D _collider;

    [SerializeField] Sprite[] _changeEmotions;
    Image _emotion;

    [SerializeField]
    private DOTweenVisualManager _selectUIPanel;

    [SerializeField]
    private Text _yes_SetenceText;
    [SerializeField]
    private Text _no_SetenceText;

    public static DialogueManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        _collider.size = new Vector2(Screen.width, Screen.height);
        _emotion = _collider.GetComponent<Image>();
        TalkLauncher(1);
    }

    public void Talk()
    {
        if (isTyping)
        {
            // 타이핑 중일 경우, 현재 문장 전체를 즉시 표시
            CompleteCurrentSentence();
        }
        else
        {
            if (JsonParseManager.Instance._actionTypeList.Count != 0)
            {
                for (int i = 0; i < JsonParseManager.Instance._actionTypeList.Count; i++)
                {
                    if (JsonParseManager.Instance._actionTypeList[i] == index - 1)
                    {
                        JsonParseManager.Instance.ActionType(JsonParseManager.Instance._actionType[i]);
                        break;
                    }
                }
            }

            // 타이핑이 끝났으면 다음 문장으로 이동
            DisplayNextSentence();
        }
    }

    public void TalkLauncher(int _talkNumber)
    {
        if (!_dialogueBox.enabled)
        {
            _dialogueBox.enabled = true;
            if (FindObjectOfType<GameManager>() && JsonParseManager.Instance.Tutorial)
            {
                GameManager.Instance.GameStop = true;
            }
        }

        //문장 설정
        sentences = JsonParseManager.Instance.AddTalk(_talkNumber, JsonParseManager.Instance.PrintDataForTypes(_talkNumber));
        //문장 출력 시작
        StartDialogue();
    }

    public void Change_Emotion(int _number)
    {
        if (_number > 0)
        {
            _emotion.color = new Color(255, 255, 255, 255);
            _emotion.sprite = _changeEmotions[_number - 1];
        }
        else if (_number == -1)
        {
            _emotion.color = new Color(255, 255, 255, 0);
            _emotion.sprite = null;
        }
    }

    public void SignPanelUI()
    {
        if (_signPanel.enabled)
        {
            //gameObject.SetActive(true);
            _dialogueBox.enabled = true;
            TalkLauncher(4);
            _signPanel.enabled = false;
        }
        else
        {
            _dialogueBox.enabled = false;
            _signPanel.enabled = true;
            Debug.Log("test");
            //gameObject.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        index = 0;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // 현재 Coroutine이 실행 중이면 중지
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (index < sentences.Length)
        {
            // 다음 문장 타이핑 시작
            typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
            index++;
        }
        else
        {
            // 모든 문장을 다 출력했을 때 처리 (예: 대화 종료)
            Debug.Log("대화가 끝났습니다.");

            if (JsonParseManager.Instance.YesNumber != -1 || JsonParseManager.Instance.NoNumber != -1)
            {
                if (!_selectUIPanel.enabled)
                {
                    _selectUIPanel.enabled = true;
                    _yes_SetenceText.text = JsonParseManager.Instance.Yes_Setence;
                    _no_SetenceText.text = JsonParseManager.Instance.No_Setence;
                }
            }
            else
            {
                dialogueText.text = ""; // 대화 종료 후 텍스트 초기화
            }

            _dialogueBox.enabled = false;
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;  // 타이핑 중으로 상태 설정
        dialogueText.text = "";  // 기존 텍스트 초기화
        Change_Emotion(JsonParseManager.Instance._emotionList[JsonParseManager.Instance.emotionNumber]);
        JsonParseManager.Instance.emotionNumber++;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);  // 지정된 시간만큼 대기
        }

        isTyping = false;  // 타이핑 완료 후 상태 설정
        typingCoroutine = null;
    }

    public void CompleteCurrentSentence()
    {
        // 현재 타이핑 중인 문장을 즉시 완성
        if (index > 0 && index <= sentences.Length)
        {
            dialogueText.text = sentences[index - 1];
        }

        // 타이핑 상태를 false로 변경
        isTyping = false;

        // 현재 실행 중인 Coroutine을 종료
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    public void Yes_Btn()
    {
        TalkLauncher(JsonParseManager.Instance.YesNumber);
        _selectUIPanel.enabled = false;
    }

    public void No_Btn()
    {
        TalkLauncher(JsonParseManager.Instance.NoNumber);
        _selectUIPanel.enabled = false;
    }
}
