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
    public Renderer[] KeysRender;
    public TextMesh[] KeysText;
    public TextMesh ScreenText;
    public GameObject Underline;

    private bool ModuleSolved;
    private bool Calculating;
    private bool Unicorn;

    private int Stage = 1;

    //Only used for Twitch Plays Solve Handler\\
    private int CorrectKey;

    Color greenText = new Color(0.15f, 1f, 0f);
    Color redText = new Color(1f, 0.15f, 0);

    private readonly string[] Stage1Words = { "いち", "に", "さん", "し", "ご", "ろく", "なな", "はち", "きゅう", "じゅう", "かわ", "りゅう", "やま", "つぎ", "いま", "ひ", "とき", "えん", "ひと", "みず" };
    private readonly string[] Stage2Words = { "ひだり", "みぎ", "きた", "ひがし", "にし", "みなみ", "そと", "たかい", "ちいさい", "なか", "ながい", "きん", "しろい", "あめ", "くるま", "はなし", "あかい", "あき", "そら", "あさ", "だれ", "さむらい", "こころ", "あい", "うつ", "えき", "うま", "うみ", "まつり", "いえ", "いぬ", "ねこ", "なに", "はな", "ふとい" };
    private readonly string[] Stage3Words = { "きょう", "きのう", "あした", "もんだい", "がっこう", "あんぜん", "けいさつ", "さいきん", "じゆう", "しゃかい", "せかい", "せんそう", "にんじゃ", "ひつよう", "りゆう", "かがく", "きけん", "へいわ", "きかい", "しけん", "ばくはつ", "でんき", "でんしゃ", "でんわ", "けいご", "おとな", "こども", "けいけん", "どうぐ", "しつもん", "かみさま", "せつめい", "てんさい", "にほん", "ほんじつ" };
    private readonly string[] Stage1Char = { "一", "ニ", "三", "四", "五", "六", "七", "八", "九", "十", "川", "龍", "山", "次", "今", "火", "時", "円", "人", "水" };
    private readonly string[] Stage2Char = { "左", "右", "北", "東", "西", "南", "外", "高", "小", "中", "長", "金", "白", "雨", "車", "話", "赤", "秋", "空", "朝", "誰", "侍", "心", "愛", "鬱", "駅", "馬", "海", "祭", "家", "犬", "猫", "何", "花", "太" };
    private readonly string[] Stage3Char = { "今日", "昨日", "明日", "問題", "学校", "安全", "警察", "最近", "自由", "社会", "世界", "戦争", "忍者", "必要", "理由", "科学", "危険", "平和", "機械", "試験", "爆発", "電気", "電車", "電話", "敬語", "大人", "子供", "経験", "道具", "質問", "神様", "説明", "天才", "日本", "本日" };

    // Yes the word and char array indexes are identical but I wanted to use a Dictionary anyway.
    private readonly Dictionary<string, string> Combinations = new Dictionary<string, string>()
    {
        //Stage 1
        { "いち", "一" }, { "に", "ニ" }, { "さん", "三" }, { "し", "四" }, { "ご", "五" }, { "ろく", "六" }, { "なな", "七" }, { "はち", "八" }, { "きゅう", "九" }, { "じゅう", "十" },
        { "かわ", "川" }, { "りゅう","龍" }, { "やま", "山" }, { "つぎ", "次" }, { "いま", "今" }, { "ひ", "火" }, { "とき", "時" }, { "えん", "円" }, { "ひと", "人" }, { "みず", "水" },

        //Stage 2 
        { "左", "ひだり" }, { "右", "みぎ" }, { "北", "きた" }, { "東", "ひがし" }, { "西", "にし" }, { "南", "みなみ" }, { "外", "そと" }, { "高", "たかい" }, { "小", "ちいさい" },
        { "中", "なか" }, { "長", "ながい" }, { "金", "きん" }, { "白", "しろい" }, { "雨", "あめ" }, { "車", "くるま" }, { "話", "はなし" }, { "赤", "あかい" }, { "秋", "あき" },
        { "空", "そら" }, { "朝", "あさ" }, { "誰", "だれ" }, { "侍", "さむらい" }, { "心", "こころ" }, { "愛", "あい" }, { "鬱", "うつ" }, { "駅", "えき" }, { "馬", "うま" },
        { "海", "うみ" }, { "祭", "まつり" }, { "家", "いえ" }, { "犬", "いぬ" }, { "猫", "ねこ" }, { "何", "なに" }, { "花", "はな" }, { "太", "ふとい"},

        //Stage 3
        { "きょう", "今日" }, { "きのう", "昨日"}, { "あした", "明日"}, { "もんだい", "問題"}, { "がっこう", "学校"}, { "あんぜん", "安全"}, { "けいさつ", "警察"}, { "さいきん", "最近"},
        { "じゆう", "自由" }, { "しゃかい","社会"}, { "せかい", "世界"}, { "せんそう", "戦争"}, { "にんじゃ", "忍者"}, { "ひつよう", "必要"}, { "りゆう", "理由"}, { "かがく", "科学"},
        { "きけん", "危険"}, { "へいわ", "平和"}, { "きかい", "機械"}, { "しけん", "試験"}, { "ばくはつ", "爆発"}, { "でんき", "電気"}, { "でんしゃ","電車"}, { "でんわ","電話"},
        { "けいご","敬語"}, { "おとな","大人"}, { "こども","子供"}, { "けいけん","経験"}, { "どうぐ","道具"}, { "しつもん","質問"}, { "かみさま","神様"}, { "せつめい","説明"},
        { "てんさい","天才"}, { "にほん","日本"}, { "ほんじつ", "本日" }
    };

    private List<string> moduleNames = new List<string>();

    //Logging
    static int ModuleIDCounter = 1;
    int ModuleID;

    private void Awake()
    {
        ModuleID = ModuleIDCounter++;

        foreach (KMSelectable Key in Keys)
        {
            Key.OnInteract += delegate ()
            {
                if (!ModuleSolved && !Calculating)
                {
                    Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Key.transform);
                    Key.AddInteractionPunch(.4f);
                    Debug.LogFormat("[Kanji #{0}] You pressed '{1}'.", ModuleID, Key.GetComponentInChildren<TextMesh>().text);
                    var CorrectButton = Combinations[ScreenText.text];
                    var SubmittedButton = Key.GetComponentInChildren<TextMesh>().text;
                    var Result = CorrectButton == SubmittedButton;

                    if (Unicorn && SubmittedButton != "龍")
                    {
                        Debug.LogFormat("[Kanji #{0}] Unicorn was present but the wrong button was submitted!", ModuleID);
                        StartCoroutine(HandleStrike());
                        return false;
                    }

                    if (Unicorn && SubmittedButton == "龍")
                    {
                        Debug.LogFormat("[Kanji #{0}] Unicorn! Congratulations!", ModuleID);
                        StartCoroutine(HandleSolve());
                        return false;
                    }

                    if (!Result)
                    {
                        Debug.LogFormat("[Kanji #{0}] Incorrect! The required button was not pressed. The correct button was {1}. Strike issued and resetting stage...", ModuleID, CorrectButton);
                        StartCoroutine(HandleStrike());
                        return false;
                    }

                    Stage += 1;
                    if (Stage == 4)
                        StartCoroutine(HandleSolve());
                    else
                        StartCoroutine(StageAdvanceAnimation());
                    return false;
                }
                return false;
            };
        }
    }

    // Initialization
    private void Start()
    {
        PickChar();
    }

    private void PickChar()
    {
         switch (Stage)
         {
             case 2:
                     foreach (var Key in KeysText)
                         Key.fontSize = 19;
                     break;
             case 3:
                     foreach (var Key in KeysText)
                         Key.fontSize = 35;
                     break;
         }

        Debug.LogFormat("[Kanji #{0}] Current Stage: " + Stage, ModuleID);
        var WordList = new[] { Stage1Words, Stage2Char, Stage3Words }[Stage - 1];
        ScreenText.text = WordList[Random.Range(0, WordList.Length)];
        Debug.LogFormat("[Kanji #{0}] The Screen Displays: " + ScreenText.text, ModuleID);
        var CharacterList = new[] { Stage1Char, Stage2Words, Stage3Char }[Stage - 1];
        List<string> StageCharacters = new List<string>(CharacterList);
        StageCharacters.Shuffle().Remove(Combinations[ScreenText.text]);
        var characters = StageCharacters.Shuffle().Where(text => text != Combinations[ScreenText.text]).ToArray();
        
        for (int i = 0; i < KeysText.Length; i++)
        {
            KeysText[i].text = characters[i];
        }

        CorrectKey = Random.Range(0, Keys.Count());
        KeysText[CorrectKey].text = Combinations[ScreenText.text];
        
        moduleNames = Bomb.GetModuleNames();
        Unicorn = false;
        for (int i = 0; i < moduleNames.Count; i++)
        {
            if (moduleNames[i] == "Dragon Energy" && KeysText[3].text == "龍")
            {
                Unicorn = true;
            }
        }
        Calculating = false;
    }

    private IEnumerator HandleSolve()
    {
        Debug.LogFormat("[Kanji #{0}] Module Solved!", ModuleID);
        ModuleSolved = true;
        Audio.PlaySoundAtTransform("SolveSound", transform);
        ScreenText.text = "ナイス！";
        ScreenText.color = greenText;
        GetComponent<KMBombModule>().HandlePass();

        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                KeysRender[i].material.color = greenText;
                KeysText[i].text = "";
                yield return new WaitForSeconds(0.3f);
            }

            for (int i = 0; i < 4; i++)
            {
                KeysRender[i].material.color = Color.black;
                yield return new WaitForSeconds(0.3f);
            }
        }

    }

    private IEnumerator HandleStrike()
    {
        Calculating = true;
        Audio.PlaySoundAtTransform("StrikeSound", transform);
        ScreenText.color = redText;
        int i = 0;
        while (i < 170)
        {
            foreach (var key in KeysText)
            {
                key.color = redText;
                yield return new WaitForSeconds(0.0001f);
                key.color = Color.black;
                yield return new WaitForSeconds(0.0001f);
                key.color = Color.white;
                yield return new WaitForSeconds(0.0001f);
                ++i;
            }
        }
        ScreenText.color = Color.black;
        GetComponent<KMBombModule>().HandleStrike();
        PickChar();
    }

    private IEnumerator StageAdvanceAnimation()
    {
        Debug.LogFormat("[Kanji #{0}] Correct! Advancing stage...", ModuleID);
        Calculating = true;
        Audio.PlaySoundAtTransform("StageSound", transform);

        int i = 0;
        while (i < 12)
        {
            foreach (var Key in KeysText)
            {
                Key.color = greenText;
                var WordList = new[] { Stage1Words, Stage2Char, Stage3Words }[Stage - 1];
                Key.text = WordList[Random.Range(0, WordList.Length)];
                yield return new WaitForSeconds(0.3f);
                ScreenText.text = Stage3Words[Random.Range(0, Stage3Words.Count())];
                Key.color = Color.white;
                i += 1;
            }
        }
        PickChar();
    }

    //Twitch Plays
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press <1234> [Presses the specified buttons in reading order]";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.ToLowerInvariant().Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        int Result;
        if (parameters.Length == 2 && parameters[0] == "press" && int.TryParse(parameters[1], out Result) && Result >= 1 && Result <= 4)
        {
            yield return null;
            yield return "strike";
            yield return "solve";

            Keys[Result - 1].OnInteract();
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator TwitchHandleForcedSolve()
    {
        while (!ModuleSolved)
        {
            Keys[CorrectKey].OnInteract();
            yield return new WaitForSeconds(4f);
        }
        yield break;
    }
}