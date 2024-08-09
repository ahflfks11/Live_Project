using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [SerializeField]
    private DOTweenVisualManager _signPanel;
    public Text dialogueText;  // UI Text ������Ʈ
    public float typingSpeed = 0.05f;  // Ÿ���� �ӵ�
    private string[] sentences;  // ��ȭ ���� �迭
    private int index = 0;  // ���� ���� �ε���
    private Coroutine typingCoroutine;  // ���� ���� ���� Coroutine�� �����ϱ� ���� ����
    private bool isTyping = false;  // ���� Ÿ���� ������ Ȯ���ϱ� ���� ����

    [SerializeField]
    private DOTweenVisualManager _selectUIPanel;

    [SerializeField]
    private Text _yes_SetenceText;
    [SerializeField]
    private Text _no_SetenceText;

    public static DialogueManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TalkLauncher(1);
    }

    void Update()
    {
        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
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
        }
    }

    public void TalkLauncher(int _talkNumber)
    {
        //���� ����
        sentences = JsonParseManager.Instance.AddTalk(_talkNumber, JsonParseManager.Instance.PrintDataForTypes(_talkNumber));
        //���� ��� ����
        StartDialogue();
    }

    public void SignPanelUI()
    {
        if (_signPanel.enabled)
        {
            TalkLauncher(4);
            _signPanel.enabled = false;
        }
        else
            _signPanel.enabled = true;
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
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;  // Ÿ���� ������ ���� ����
        dialogueText.text = "";  // ���� �ؽ�Ʈ �ʱ�ȭ

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);  // ������ �ð���ŭ ���
        }

        isTyping = false;  // Ÿ���� �Ϸ� �� ���� ����
        typingCoroutine = null;
    }

    private void CompleteCurrentSentence()
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
