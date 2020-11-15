using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using KModkit;

public class KanjiModule : MonoBehaviour
{
    //General
    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMSelectable[] Keys;
    public TextMesh[] KeysText;
    public TextMesh ScreenText;
    public GameObject Underline;
    private bool moduleSolved;
    private bool currentlyCalculating;
    private int Stage = 1;

    private bool pogchamp;
    private bool unicorn;

    private int s1CorrectButton;
    private int s2CorrectButton;
    private int s3CorrectButton;

    private int[] indexTest = new int[4];
    int index0;
    int index1;
    int index2;
    int index3;

    Color greenText;
    Color redText;

    private string[] Stage1Words = { "いち", "に", "さん", "し", "ご", "ろく", "なな", "はち", "きゅう", "じゅう", "かわ", "りゅう", "やま", "つぎ", "いま", "ひ", "とき", "えん", "ひと", "みず" };
    private string[] Stage2Words = { "ひだり", "みぎ", "きた", "ひがし", "にし", "みなみ", "そと", "たかい", "ちいさい", "なか", "ながい", "きん", "しろい", "あめ", "くるま", "はなす", "あかい", "あき", "そら", "あさ", "だれ", "さむらい", "こころ", "あい", "うつ", "えき", "うま", "うみ", "まつり", "いえ", "いぬ", "ねこ",  "なに",  "はな",  "ふとい" };
    private string[] Stage3Words = { "きょう", "きのう", "あした", "もんだい", "がっこう", "あんぜん", "けいさつ", "さいきん", "じゆう", "しゃかい", "せかい", "せんそう", "にんじゃ", "ひつよう", "りゆう", "かがく", "きけん", "へいわ", "きかい", "しけん", "ばくはつ", "でんき", "でんしゃ", "でんわ", "けいご", "おとな", "こども", "けいけん", "どうぐ", "しつもん", "かみさま", "せつめい", "てんさい", "にほん", "ほんじつ" };
    private string[] Stage1Char = { "一", "ニ", "三", "四", "五", "六", "七", "八", "九", "十", "川", "龍", "山", "次", "今", "火", "時", "円", "人", "水" };
    private string[] Stage2Char = { "左", "右", "北", "東", "西", "南", "外", "高", "小", "中", "長", "金", "白", "雨", "車", "話", "赤", "秋", "空", "朝", "誰", "侍", "心", "愛", "鬱", "駅", "馬", "海", "祭", "家", "犬", "猫", "何", "花", "太" };
    private string[] Stage3Char = { "今日", "昨日", "明日", "問題", "学校", "安全", "警察", "最近", "自由", "社会", "世界", "戦争", "忍者", "必要", "理由", "科学", "危険", "平和", "機械", "試験", "爆発", "電気", "電車", "電話", "敬語", "大人", "子供", "経験", "道具", "質問", "神様", "説明", "天才", "日本", "本日" };

    private List<string> moduleNames = new List<string>();

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable Key in Keys)
        {
            KMSelectable pressedKey = Key;
            Key.OnInteract += delegate () { KeyPress(pressedKey); return false; };
        }
        greenText = new Color(0.15f, 1f, 0f);
        redText = new Color(1f, 0.15f, 0);
    }

    // Initialization
    void Start()
    {
        moduleNames = Bomb.GetModuleNames();
        for (int i = 0; i < moduleNames.Count; i++)
        {
            if (moduleNames[i] == "Dragon Energy")
            {
                pogchamp = true;
            }
        }
        PickChar();
	}

    void PickChar()
    {
        currentlyCalculating = true;
        foreach (var index in indexTest)
        {
            switch (Stage)
            {
                case 1:
                    UnityEngine.Random.Range(0, 20);
                    break;
                case 2:
                    UnityEngine.Random.Range(0, 35);
                    break;
                case 3:
                    UnityEngine.Random.Range(0, 35);
                    break;
                default:
                    Debug.LogFormat("Error A1");
                        break;
            } 
        }
        s1CorrectButton = UnityEngine.Random.Range(0, 4);
        s2CorrectButton = UnityEngine.Random.Range(0, 4);
        s3CorrectButton = UnityEngine.Random.Range(0, 4);

        for (int i = 0; i <= 3; i++)
        {
            if (Stage == 1)
            {
                while (index0 == index1 || index0 == index2 || index0 == index3)
                {
                    index0 = UnityEngine.Random.Range(0, 20);
                }
                while (index1 == index2 || index1 == index3)
                {
                    index1 = UnityEngine.Random.Range(0, 20);
                }
                while (index2 == index3)
                {
                    index2 = UnityEngine.Random.Range(0, 20);
                }
            }
            else if (Stage == 2)
            {
                while (index0 == index1 || index0 == index2 || index0 == index3)
                {
                    index0 = UnityEngine.Random.Range(0, 35);
                }
                while (index1 == index2 || index1 == index3)
                {
                    index1 = UnityEngine.Random.Range(0, 35);
                }
                while (index2 == index3)
                {
                    index2 = UnityEngine.Random.Range(0, 35);
                }
            }
            else if (Stage == 3)
            {
                while (index0 == index1 || index0 == index2 || index0 == index3)
                {
                    index0 = UnityEngine.Random.Range(0, 35);
                }
                while (index1 == index2 || index1 == index3)
                {
                    index1 = UnityEngine.Random.Range(0, 35);
                }
                while (index2 == index3)
                {
                    index2 = UnityEngine.Random.Range(0, 35);
                }
            }
            KeysText[i].text = Keys[i].GetComponentInChildren<TextMesh>().text;
            if (Stage == 1)
            {
                KeysText[0].text = Stage1Char[index0];
                KeysText[1].text = Stage1Char[index1];
                KeysText[2].text = Stage1Char[index2];
                KeysText[3].text = Stage1Char[index3];
            }
            else if (Stage == 2)
            {
                KeysText[i].fontSize = 20;

                KeysText[0].text = Stage2Words[index0];
                KeysText[1].text = Stage2Words[index1];
                KeysText[2].text = Stage2Words[index2];
                KeysText[3].text = Stage2Words[index3];
            }
            else if (Stage == 3)
            {
                KeysText[i].fontSize = 35;

                KeysText[0].text = Stage3Char[index0];
                KeysText[1].text = Stage3Char[index1];
                KeysText[2].text = Stage3Char[index2];
                KeysText[3].text = Stage3Char[index3];
            }
        }
        Debug.LogFormat("[Kanji #{0}] Current stage: {1}.", moduleId, Stage);
        StageAnsConfig();
    }

    void StageAnsConfig()
    {
        if (Stage == 1)
        {
            if (pogchamp && KeysText[3].text == "龍")
            {
                unicorn = true;
                Debug.LogFormat("[Kanji #{0}] There is a Dragon Energy on the bomb and a '龍' character on the bottom right button. Unicorn! Press the '龍' button to solve.", moduleId);
            }
            if (s1CorrectButton == 0)
            {
                ScreenText.text = Stage1Words[index0];
            }
            else if (s1CorrectButton == 1)
            {
                ScreenText.text = Stage1Words[index1];
            }
            else if (s1CorrectButton == 2)
            {
                ScreenText.text = Stage1Words[index2];
            }
            else
            {
                ScreenText.text = Stage1Words[index3];
            }
        }

        else if (Stage == 2)
        {
            if (s2CorrectButton == 0)
            {
                ScreenText.text = Stage2Char[index0];
            }
            else if (s2CorrectButton == 1)
            {
                ScreenText.text = Stage2Char[index1];
            }
            else if (s2CorrectButton == 2)
            {
                ScreenText.text = Stage2Char[index2];
            }
            else
            {
                ScreenText.text = Stage2Char[index3];
            }

        }

        else if (Stage == 3)
        {
            if (s3CorrectButton == 0)
            {
                ScreenText.text = Stage3Words[index0];
            }
            else if (s3CorrectButton == 1)
            {
                ScreenText.text = Stage3Words[index1];
            }
            else if (s3CorrectButton == 2)
            {
                ScreenText.text = Stage3Words[index2];
            }
            else
            {
                ScreenText.text = Stage3Words[index3];
            }
        }
        Debug.LogFormat("[Kanji #{0}] The display says '{1}'.", moduleId, ScreenText.text);
        Debug.LogFormat("[Kanji #{0}] The buttons display {1}, {2}, {3} and {4}.", moduleId, KeysText[0].GetComponentInChildren<TextMesh>().text, KeysText[1].GetComponentInChildren<TextMesh>().text, KeysText[2].GetComponentInChildren<TextMesh>().text, KeysText[3].GetComponentInChildren<TextMesh>().text);
        currentlyCalculating = false;
    }

    void KeyPress(KMSelectable Key)
    {
        if (!moduleSolved && !currentlyCalculating)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Key.transform);
            Key.AddInteractionPunch(.4f);
            Debug.LogFormat("[Kanji #{0}] You pressed '{1}'.", moduleId, Key.GetComponentInChildren<TextMesh>().text);

            if (unicorn && Key == Keys[3])
            {
                HandleSolve();
                return;
            }

            if (Stage == 1)
            {
                if (s1CorrectButton == 0 && Key == Keys[0])
                {
                    StageAdvance();
                }
                else if (s1CorrectButton == 1 && Key == Keys[1])
                {
                    StageAdvance();
                }
                else if (s1CorrectButton == 2 && Key == Keys[2])
                {
                    StageAdvance();
                }
                else if (s1CorrectButton == 3 && Key == Keys[3])
                {
                    StageAdvance();
                }
                else
                {
                    HandleStrike();
                }
            }
            else if (Stage == 2)
            {
                if (s2CorrectButton == 0 && Key == Keys[0])
                {
                    StageAdvance();
                }
                else if (s2CorrectButton == 1 && Key == Keys[1])
                {
                    StageAdvance();
                }
                else if (s2CorrectButton == 2 && Key == Keys[2])
                {
                    StageAdvance();
                }
                else if (s2CorrectButton == 3 && Key == Keys[3])
                {
                    StageAdvance();
                }
                else
                {
                    HandleStrike();
                }
            }
            else if (Stage == 3)
            {
                if (s3CorrectButton == 0 && Key == Keys[0])
                {
                    StageAdvance();
                }
                else if (s3CorrectButton == 1 && Key == Keys[1])
                {
                    StageAdvance();
                }
                else if (s3CorrectButton == 2 && Key == Keys[2])
                {
                    StageAdvance();
                }
                else if (s3CorrectButton == 3 && Key == Keys[3])
                {
                    StageAdvance();
                }
                else
                {
                    HandleStrike();
                }
            }
        }
    }

    void StageAdvance()
    {       
        if (Stage == 3)
        {
            HandleSolve();
        }
        else
        {
            currentlyCalculating = true;
            Debug.LogFormat("[Kanji #{0}] Correct! Advancing Stage...", moduleId);
            Stage++;           
            StartCoroutine(StageAdvanceAnimation());
        }
    }

    void HandleStrike()
    {
        currentlyCalculating = true;
        if (Stage == 1)
        {
            Debug.LogFormat("[Kanji #{0}] Strike! The correct button was '{1}'. Resetting Stage...", moduleId, KeysText[s1CorrectButton].text);
        }
        else if (Stage == 2)
        {
            Debug.LogFormat("[Kanji #{0}] Strike! The correct button was '{1}'. Resetting Stage...", moduleId, KeysText[s2CorrectButton].text);
        }
        else if (Stage == 3)
        {
            Debug.LogFormat("[Kanji #{0}] Strike! The correct button was '{1}'. Resetting Stage...", moduleId, KeysText[s3CorrectButton].text);
        }
        StartCoroutine(StrikeKaboomAnimation());
    }

    void HandleSolve()
    {
        moduleSolved = true;
        Audio.PlaySoundAtTransform("SolveSound", transform);
        StartCoroutine(SolveAnimation());
    }

    IEnumerator SolveAnimation()
    {
        ScreenText.color = greenText;
        KeysText[0].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = Color.black;
        KeysText[1].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = greenText;
        KeysText[2].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = Color.black;
        KeysText[3].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = greenText;
        KeysText[0].color = Color.white;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = Color.black;
        KeysText[1].color = Color.white;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = greenText;
        KeysText[2].color = Color.white;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = Color.black;
        KeysText[3].color = Color.white;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = greenText;
        KeysText[0].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = Color.black;
        KeysText[1].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = greenText;
        KeysText[2].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = Color.black;
        KeysText[3].color = greenText;
        yield return new WaitForSeconds(0.25f);
        ScreenText.color = greenText;
        int m1 = 0;
        int m2 = 0;
        int m3 = 0;
        int m4 = 0;
        while (m1 != 30)
        {
            yield return new WaitForSeconds(0.015f);
            Keys[0].transform.localPosition = Keys[0].transform.localPosition + Vector3.up * -0.00021f;
            m1++;
        }
        while (m2 != 30)
        {
            yield return new WaitForSeconds(0.015f);
            Keys[1].transform.localPosition = Keys[1].transform.localPosition + Vector3.up * -0.00021f;
            m2++;
        }
        while (m3 != 30)
        {
            yield return new WaitForSeconds(0.015f);
            Keys[2].transform.localPosition = Keys[2].transform.localPosition + Vector3.up * -0.00021f;
            m3++;
        }
        while (m4 != 30)
        {
            yield return new WaitForSeconds(0.015f);
            Keys[3].transform.localPosition = Keys[3].transform.localPosition + Vector3.up * -0.00021f;
            m4++;
        }
        ScreenText.text = "";
        Underline.transform.localPosition = Underline.transform.localPosition + Vector3.back * -500f;
        Debug.LogFormat("[Kanji #{0}] Correct! Module Solved!", moduleId);
        GetComponent<KMBombModule>().HandlePass();
        yield return null;
    }

    IEnumerator StrikeKaboomAnimation()
    {
        GetComponent<KMBombModule>().HandleStrike();
        Audio.PlaySoundAtTransform("StrikeSound", transform);
        ScreenText.color = redText;
        KeysText[0].color = redText;
        yield return new WaitForSeconds(0.60f);
        KeysText[1].color = redText;
        yield return new WaitForSeconds(0.2f);
        KeysText[2].color = redText;
        yield return new WaitForSeconds(0.47f);
        KeysText[3].color = redText;
        yield return new WaitForSeconds(0.70f);
        KeysText[0].color = Color.white;
        yield return new WaitForSeconds(0.3f);
        KeysText[1].color = Color.white;
        yield return new WaitForSeconds(0.3f);
        KeysText[2].color = Color.white;
        yield return new WaitForSeconds(0.3f);
        KeysText[3].color = Color.white;
        yield return new WaitForSeconds(0.2f);
        ScreenText.color = Color.black;
        PickChar();
        yield return null;
    }

    IEnumerator StageAdvanceAnimation()
    {
        KeysText[0].fontSize = 65;
        KeysText[1].fontSize = 65;
        KeysText[2].fontSize = 65;
        KeysText[3].fontSize = 65;
        Audio.PlaySoundAtTransform("StageSound", transform);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[0].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.2f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[1].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.2f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[2].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.2f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[3].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.2f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[0].color = Color.white;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.30f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[1].color = Color.white;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.35f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[2].color = Color.white;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.40f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[3].color = Color.white;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.42f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[0].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.40f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[1].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.36f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[2].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.35f);
        ScreenText.text = Stage1Words[UnityEngine.Random.Range(0, 20)];
        KeysText[3].color = greenText;
        KeysText[0].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[1].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[2].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        KeysText[3].text = Stage1Char[UnityEngine.Random.Range(0, 20)];
        yield return new WaitForSeconds(0.70f);
        KeysText[0].color = Color.white;
        yield return new WaitForSeconds(0.2f);
        KeysText[1].color = Color.white;
        yield return new WaitForSeconds(0.2f);
        KeysText[2].color = Color.white;
        yield return new WaitForSeconds(0.2f);
        KeysText[3].color = Color.white;
        yield return new WaitForSeconds(0.2f);

        PickChar();

        yield return null;
    }
    //Twitch Plays (Thanks Exish) :)
    private bool isCmdValid(string cmd)
    {
        char[] valids = { '1', '2', '3', '4' };
        if ((cmd.Length >= 1) || (cmd.Length <= 4))
        {
            foreach (char c in cmd)
            {
                if (!valids.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press <1234> [Presses the specified buttons in reading order]";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length >= 2)
            {
                string sub = command.Substring(6);
                sub = sub.Replace(" ", "");
                parameters[1] = sub;
                if (isCmdValid(parameters[1]))
                {
                    yield return null;
                    for (int i = 0; i < parameters[1].Length; i++)
                    {
                        if (parameters[1].ElementAt(i).Equals('1'))
                        {
                            Keys[0].OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('2'))
                        {
                            Keys[1].OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('3'))
                        {
                            Keys[2].OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('4'))
                        {
                            Keys[3].OnInteract();
                        }
                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return new WaitForSeconds(0.5f);
                    yield break;
                }
            }
        }
        if (moduleSolved)
            yield return "solve";
    }
    IEnumerator TwitchHandleForcedSolve()
    {
        while (!moduleSolved)
        {
            if (Stage == 1)
            {
                Keys[s1CorrectButton].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
            else if (Stage == 2)
            {
                Keys[s2CorrectButton].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
            else if (Stage == 3)
            {
                Keys[s3CorrectButton].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}