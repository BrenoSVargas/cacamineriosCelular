using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    //Singleton
    private static UIManager _instance;
    public static UIManager Instance

    {
        get
        {
            if (_instance == null)
                Debug.Log("Error instance GameManger is null.");

            return _instance;
        }
    }

    //BeforeQuizStart
    public Text tittleQuiz;
    public Text textQuiz;
    public Sprite cardSprite;

    //Scene Number
    private int indexScene;
    public int IndexScene { get => indexScene; }

    //Ui
    private GameObject _answersParent;
    private Button[] _answer = new Button[6];
    private Button _cardBtn, _playAgain, _restart, _exit;
    private Image _imageQuiz;

    //
    int answerInt = 7;
    int correctAnswer = 7;
    int correctAnswer2 = 7;
    int number = 0;
    Question quizUI;

    //Tips
    public Tips[] tips;
    private Button _getTips;
    private Button[] _tipsBtn = new Button[6];
    private Button selectedBtn;
    GameObject _panelTip;
    private Text _txtTip;
    private Image _imgTip;


    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += Load;
    }

    void Load(Scene scene, LoadSceneMode mode)
    {
        indexScene = SceneManager.GetActiveScene().buildIndex;
        if (indexScene == 0)
        {
            _cardBtn = GameObject.Find("Play").GetComponent<Button>();
            _cardBtn.onClick.AddListener(Play);

            _exit = GameObject.Find("Exit").GetComponent<Button>();
            _exit.onClick.AddListener(Exit);
        }
        if (indexScene == 1)
        {
            tittleQuiz = GameObject.Find("TittleQuiz").GetComponent<Text>();
            textQuiz = GameObject.Find("TextQuiz").GetComponent<Text>();
            _imageQuiz = GameObject.Find("QuizSprite").GetComponent<Image>();

            _answer[0] = GameObject.Find("Cobre").GetComponent<Button>();
            _answer[1] = GameObject.Find("Minerio").GetComponent<Button>();
            _answer[2] = GameObject.Find("Nenhum").GetComponent<Button>();
            _answer[3] = GameObject.Find("Manganes").GetComponent<Button>();
            _answer[4] = GameObject.Find("Niquel").GetComponent<Button>();
            _answer[5] = GameObject.Find("Fosfato").GetComponent<Button>();


            _cardBtn = GameObject.Find("Cards").GetComponent<Button>();
            _cardBtn.onClick.AddListener(Cards);

            _answer[0].onClick.AddListener(Cobre);
            _answer[1].onClick.AddListener(Minerio);
            _answer[2].onClick.AddListener(Nenhum);
            _answer[3].onClick.AddListener(Manganes);
            _answer[4].onClick.AddListener(Niquel);
            _answer[5].onClick.AddListener(Fosfato);


            _answersParent = GameObject.Find("Answers");
            _answersParent.SetActive(false);

            _playAgain = GameObject.Find("PlayAgain").GetComponent<Button>();
            _restart = GameObject.Find("Restart").GetComponent<Button>();
            _restart.onClick.AddListener(Restart);
            _playAgain.onClick.AddListener(PlayAgain);
            _playAgain.gameObject.SetActive(false);

            _exit = GameObject.Find("Exit").GetComponent<Button>();
            _exit.onClick.AddListener(Exit);

            //Tips
            _getTips = GameObject.Find("TipsBtn").GetComponent<Button>();
            _tipsBtn[0] = GameObject.Find("ReturnGame").GetComponent<Button>();
            _tipsBtn[1] = GameObject.Find("TipCobre").GetComponent<Button>();
            _tipsBtn[2] = GameObject.Find("TipMinerio").GetComponent<Button>();
            _tipsBtn[3] = GameObject.Find("TipManganes").GetComponent<Button>();
            _tipsBtn[4] = GameObject.Find("TipNiquel").GetComponent<Button>();
            _tipsBtn[5] = GameObject.Find("TipFosfato").GetComponent<Button>();
            _panelTip = GameObject.Find("TipsPanel");
            _txtTip = GameObject.Find("TextTip").GetComponent<Text>();
            _imgTip = GameObject.Find("TipImg").GetComponent<Image>();
            _getTips.onClick.AddListener(GetTips);

            for (int i = 1; i < 6; i++)
            {
                _tipsBtn[i].onClick.AddListener(Tip);

            }

            _tipsBtn[0].onClick.AddListener(ReturnGame);
            _panelTip.SetActive(false);



        }

    }

    void Cards()
    {
        GameManager.Instance.StartQuestion(_cardBtn);
    }

    public void FormulateQuiz(Question quiz)
    {
        quizUI = quiz;
        tittleQuiz.text = quiz.quiz;
        _imageQuiz.sprite = quiz.illustratation;
        _answersParent.SetActive(true);
        correctAnswer = quiz.answer1;
        if (quiz.hasTwoAnswers)
        {
            correctAnswer2 = quiz.answer2;
        }
    }

    void Cobre() { answerInt = 0; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Minerio() { answerInt = 1; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Nenhum() { answerInt = 2; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Manganes() { answerInt = 3; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Niquel() { answerInt = 4; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Fosfato() { answerInt = 5; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }

    void CorrectQuestion(Button btn)
    {
        if (quizUI.hasTwoAnswers)
        {
            if (answerInt == correctAnswer || answerInt == correctAnswer2)
            {
                btn.interactable = false;
                number++;
                if (number >= 2)
                    StartCoroutine(RightAnswer());
                else
                {
                    tittleQuiz.text = "Você acertou. Qual outro minério?";
                    ScoreManager.Instance.ScoreSub();
                }
            }
            else
            {
                btn.interactable = false;
                tittleQuiz.text = "Errado. Menos 1 ponto. Tente novamente.";
                ScoreManager.Instance.ScoreSub();

            }
        }
        else
        {
            if (answerInt == correctAnswer)
            {
                btn.interactable = false;
                StartCoroutine(RightAnswer());
            }
            else
            {
                btn.interactable = false;
                tittleQuiz.text = "Errado. Menos 1 ponto. Tente novamente.";
                ScoreManager.Instance.ScoreSub();
            }
        }

        if (ScoreManager.Instance.score <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(GameOver());
        }
    }

    IEnumerator RightAnswer()
    {
        tittleQuiz.text = "Parabéns!";
        yield return new WaitForSeconds(1.2f);
        number = 0;
        answerInt = 7;
        ActiveAnswers();
        GameManager.Instance.GetRandomQuestion();
    }

    IEnumerator GameOver()
    {
        _answersParent.SetActive(false);

        yield return new WaitForSeconds(0.8f);

        tittleQuiz.text = "Não foi dessa vez.";
        textQuiz.text = "Seus pontos acabaram. Tente outra vez. Lembre-se de usar as consultas.";
        _playAgain.gameObject.SetActive(true);
    }

    void PlayAgain()
    {
        if(_panelTip){
            ReturnGame();
        }
        textQuiz.enabled = true;
        textQuiz.text = "Toque na carta para começar o jogo.";
        tittleQuiz.text = "Toque na carta para começar o jogo.";
        _cardBtn.interactable = true;
        _imageQuiz.sprite = cardSprite;
        _playAgain.gameObject.SetActive(false);
        ActiveAnswers();
        ScoreManager.Instance.StartGame();
        GameManager.Instance.StartGame();
    }

    void ActiveAnswers()
    {
        for (int i = 0; i < 6; i++)
        {
            _answer[i].interactable = true;
        }
    }

    void Restart()
    {
        StopAllCoroutines();
        _answersParent.SetActive(false);
        _getTips.interactable = true;
        PlayAgain();
    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }

    void Exit()
    {
        Application.Quit();
    }

    void ReturnGame()
    {
        _txtTip.text = " ";
        _imgTip.sprite = null;
        if (selectedBtn)
            selectedBtn.interactable = true;
        _panelTip.SetActive(false);
    }

    void GetTips()
    {
        _panelTip.SetActive(true);
        ScoreManager.Instance.TipSub();
        _imgTip.color = Color.clear;

        if (ScoreManager.Instance.tipsInt == 0)
        {
            _getTips.interactable = false;
        }
    }

    void Tip()
    {
        if (selectedBtn)
        {
            selectedBtn.interactable = true;
        }
        for (int i = 0; i < 5; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == tips[i].nameTip)
            {
                _imgTip.color = Color.white;
                _txtTip.text = tips[i].txtTip;
                _txtTip.color = tips[i].colorTip;
                _imgTip.sprite = tips[i].imgTip;

                Button n = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                selectedBtn = n;
                n.interactable = false;
                break;
            }
        }
    }


}
