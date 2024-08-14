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
    public Text dialogueText;  // UI Text ������Ʈ
    public float typingSpeed = 0.05f;  // Ÿ���� �ӵ�
    private string[] sentences;  // ��ȭ ���� �迭
    private int index = 0;  // ���� ���� �ε���
    private Coroutine typingCoroutine;  // ���� ���� ���� Coroutine�� �����ϱ� ���� ����
    private bool isTyping = false;  // ���� Ÿ���� ������ Ȯ���ϱ� ���� ����

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
            // Ÿ���� ���� ���, ���� ���� ��ü�� ��� ǥ��
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

            // Ÿ������ �������� ���� �������� �̵�
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

        //���� ����
        sentences = JsonParseManager.Instance.AddTalk(_talkNumber, JsonParseManager.Instance.PrintDataForTypes(_talkNumber));
        //���� ��� ����
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
        // ���� Coroutine�� ���� ���̸� ����
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (index < sentences.Length)
        {
            // ���� ���� Ÿ���� ����
            typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
            index++;
        }
        else
        {
            // ��� ������ �� ������� �� ó�� (��: ��ȭ ����)
            Debug.Log("��ȭ�� �������ϴ�.");

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
                dialogueText.text = ""; // ��ȭ ���� �� �ؽ�Ʈ �ʱ�ȭ
            }

            _dialogueBox.enabled = false;
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;  // Ÿ���� ������ ���� ����
        dialogueText.text = "";  // ���� �ؽ�Ʈ �ʱ�ȭ
        Change_Emotion(JsonParseManager.Instance._emotionList[JsonParseManager.Instance.emotionNumber]);
        JsonParseManager.Instance.emotionNumber++;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);  // ������ �ð���ŭ ���
        }

        isTyping = false;  // Ÿ���� �Ϸ� �� ���� ����
        typingCoroutine = null;
    }

    public void CompleteCurrentSentence()
    {
        // ���� Ÿ���� ���� ������ ��� �ϼ�
        if (index > 0 && index <= sentences.Length)
        {
            dialogueText.text = sentences[index - 1];
        }

        // Ÿ���� ���¸� false�� ����
        isTyping = false;

        // ���� ���� ���� Coroutine�� ����
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
