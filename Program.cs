using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

public class Rotor
{
    public string Setting;
    public char Notch;
    public int NowPosition;

    public Rotor(string setting, char notch)
    {
        Setting = setting;
        Notch = notch;
        NowPosition = 0;
    }

    public void SetNowPosition(char setting)
    {
        NowPosition = Setting.IndexOf(setting);
    }

    //로터를 count만큼 회전시키는 함수
    public bool Move(out int nextStep, int count = 1)
    {
        int p = NowPosition;
        nextStep = 0;
        //_Move() : 로터를 한 칸 회전시키는 함수
        //만일 로터가 한바퀴 돌았다면 true를 반환하고, 이는 다음 로터가 회전해야 함을 나타냄
        for (int i = 0; i < count; i++) {
            nextStep += _Move() == true ? 1 : 0;
        }
        if (nextStep > 0)
            return true;
        return false;
    }

    bool _Move()
    {
        NowPosition = (NowPosition + 1) % Setting.Length;
        return Setting[(NowPosition)] == Notch;
    }

    //글자가 로터의 오른쪽에서 왼쪽으로(forward) 통과했을 때 변환하는 함수
    public char FlowForward(char c)
    {
        //입력한 글자의 알파벳 순서. A는 0, B는 1... Z는 25
        int input = ConvertChar2IDX(c);
        //로터의 회전을 고려한 입력 글자의 위치
        //로터는 NowPosition만큼 회전한 것으로 설정하여, 이 값을 더하여 이동을 나타냄
        int enter_contact = (input + NowPosition) % 26;
        //회전을 고려한 입력 글자에 해당하는 로터의 대응 글자를 찾음
        int exit_contact = ConvertChar2IDX(Setting[enter_contact]);
        //실제 물리적 구조라면 단순히 입력에 회전을 고려하는 것으로 계산 가능하지만,
        //코드로 구현할 때에는 출력 값에도 회전을 고려해야므로, 회전한 정도를 -를 이용하여 계산함
        int exit_position = (exit_contact - NowPosition + 26) % 26;
        return ConvertIDX2Char(exit_position);
    }
    //글자가 로터의 왼쪽에서 오른쪽으로(backward) 통과했을 때 변환하는 함수
    //FlowForward를 역과정으로 구현한 함수
    public char FlowBackward(char c)
    {
        int input = ConvertChar2IDX(c);
        int enter_contact = (input + NowPosition) % 26;
        int exit_contact = 26 - strchr(Setting, ConvertIDX2Char(enter_contact)).Length;
        int exit_position = (exit_contact - NowPosition + 26) % 26;
        return ConvertIDX2Char(exit_position);
    }

    private string strchr(string originalString, char charToSearch)
    {
        int found = originalString.IndexOf(charToSearch);
        return found > -1 ? originalString.Substring(found) : null;
    }

    public int ConvertChar2IDX(char input)
    {
        return input - 'A';
    }

    public char ConvertIDX2Char(int input)
    {
        return (char)(input + 'A');
    }
}

public class Reflector
{
    public string Setting;

    public Reflector(string setting)
    {
        Setting = setting;
    }

    public char Reflect(char input)
    {
        int index = ConvertChar2Index(input);
        //대응표(Setting)에서 입력 글자(input)에 해당하는 글자를 찾아서 반환
        return Setting[index];
    }

    public int ConvertChar2Index(char input)
    {
        return input - 'A';
    }
}

public class PlugBoard
{
    public bool Contains(char input)
    {
        for (int i = 0; i < wiring.Count; i++)
        {
            if (wiring.Keys.ElementAt(i) == input)
            {
                return true;
            }

            if (wiring.Values.ElementAt(i) == input)
            {
                return true;
            }
        }
        return false;
    }
    public Dictionary<char, char> wiring;

    public PlugBoard(Dictionary<char, char> wiring)
    {
        this.wiring = new Dictionary<char, char>();

        // Ensure the wiring is bidirectional
        foreach (var pair in wiring)
        {
            Add(pair.Key, pair.Value);
            //this.wiring[pair.Key] = pair.Value;
            //this.wiring[pair.Value] = pair.Key;
        }
    }

    public bool Add(char a, char b)
    {
        if (a == b)
            return true;
        for (int i = 0; i < wiring.Count; i++)
        {
            if (wiring.Keys.ElementAt(i) == a)
            {
                if (wiring.Values.ElementAt(i) != b)
                    return false;
                else
                    return true;
            }
            if (wiring.Keys.ElementAt(i) == b)
            {
                if (wiring.Values.ElementAt(i) != a)
                    return false;
                else
                    return true;
            }

            if (wiring.Values.ElementAt(i) == a)
            {
                if (wiring.Keys.ElementAt(i) != b)
                    return false;
                else
                    return true;
            }
            if (wiring.Values.ElementAt(i) == b)
            {
                if (wiring.Keys.ElementAt(i) != a)
                    return false;
                else
                    return true;
            }
        }
        //Console.WriteLine($"ADD : {a} - {b}");
        wiring.Add(a, b);
        return true;
    }
    public bool Contains(char a, char b)
    {
        if (a == b)
            return true;
        for (int i = 0; i < wiring.Count; i++)
        {
            if (wiring.Keys.ElementAt(i) == a)
            {
                if (wiring.Values.ElementAt(i) != b)
                    return false;
                else
                    return true;
            }
            if (wiring.Keys.ElementAt(i) == b)
            {
                if (wiring.Values.ElementAt(i) != a)
                    return false;
                else
                    return true;
            }

            if (wiring.Values.ElementAt(i) == a)
            {
                if (wiring.Keys.ElementAt(i) != b)
                    return false;
                else
                    return true;
            }
            if (wiring.Values.ElementAt(i) == b)
            {
                if (wiring.Keys.ElementAt(i) != a)
                    return false;
                else
                    return true;
            }
        }
        return false;
    }
    public void Show()
    {
        for (int i = 0; i < wiring.Count; i++)
        {
            Console.Write("({0} {1})  ", wiring.Keys.ElementAt(i), wiring.Values.ElementAt(i));
        }
        Console.WriteLine("");
    }
    public void Clear()
    {
        wiring.Clear();
    }

    public char Substitute(char c)
    {
        //딕셔너리는 컴퓨터가 자료를 저장하는 방법 중 하나로, 순서쌍과 비슷함
        //(key, value) 형태로 값을 저장함

        //입력 글자가 딕셔너리의 key에 존재한다면, 그에 해당하는 value를 반환
        if (wiring.TryGetValue(c, out char value)) {
            return value;
        }

        //입력 글자가 딕셔너리의 value에 존재한다면, 그에 해당하는 key를 반환
        if (wiring.Values.Contains(c)) {
            return wiring.FirstOrDefault(x => x.Value == c).Key;
        }
        //어디에도 포함되어있지 않다면, 플러그보드 설정이 존재하지 않는 것으로 판단
        //들어온 글자 그대로 반환
        return c;
    }
}

public class Enigma
{
    public void ShowRotors()
    {
        Console.WriteLine(
            $"{Rotors[0].Setting[((Rotors[0].NowPosition))]} - " +
            $"{Rotors[1].Setting[((Rotors[1].NowPosition))]} - " +
            $"{Rotors[2].Setting[((Rotors[2].NowPosition))]}");
    }
    public Rotor[] Rotors;
    Reflector Reflector;
    PlugBoard PlugBoard;
    public void SetStartPosition(string setting)
    {
        for (int i = 0; i < Rotors.Length; i++)
        {
            Rotors[i].SetNowPosition(setting[i]);
        }
    }
    public Enigma(Rotor[] rotors, Reflector reflector, PlugBoard plugBoard)
    {
        Rotors = rotors;
        Reflector = reflector;
        PlugBoard = plugBoard;
    }
    public void Rotate(int count)
    {
        bool shoudMoveNextRotor = true;
        int next = count;
        for (int i = Rotors.Length - 1; i >= 0; i--) {
            if (shoudMoveNextRotor) {
                int step = next;
                shoudMoveNextRotor = Rotors[i].Move(out next, step);
            }
            else {
                break;
            }
        }
    }

    public char EncryptCharacter(char input)
    {
        bool shoudMoveNextRotor = true;
        int next = 1;
        for (int i = Rotors.Length - 1; i >= 0; i--) {
            if (shoudMoveNextRotor) {
                int step = next;
                shoudMoveNextRotor = Rotors[i].Move(out next, step);
            }
            else
            {
                break;
            }
        }

        char character = input;

        //플러그보드를 통한 변환
        character = PlugBoard.Substitute(character);

        //로터를 통한 변환
        for (int i = Rotors.Length - 1; i >= 0; i--) {
            character = Rotors[i].FlowForward(character);
        }

        //반사판을 통한 변환
        character = Reflector.Reflect(character);

        //로터를 통한 변환
        for (int i = 0; i < Rotors.Length; i++) {
            character = Rotors[i].FlowBackward(character);
        }
        
        //플러그보드를 통한 변환
        character = PlugBoard.Substitute(character);

        return character;
    }

    public string EncryptString(string input)
    {
        string result = "";
        for (int i = 0; i < input.Length; i++)
        {
            result += EncryptCharacter(input[i]);
        }
        return result;
    }
}
public class Bombe
{
    public class BombeData
    {
        public Rotor[] Rotors;
        public Reflector relf;
        public PlugBoard plugboard;

        public string Encrtyp;
        public string Crib;

        public string StartPosition;
        public int offset;
        public Dictionary<char, int> freq;
        public string RotorSeq;
    }
    public static Rotor rotor1
    {
        get { return Data.rotor1; }
    }
    public static Rotor rotor2
    {
        get { return Data.rotor2; }
    }
    public static Rotor rotor3
    {
        get { return Data.rotor3; }
    }
    public static Reflector reflector1
    {
        get { return Data.reflector1; }
    }

    public static Rotor GetRotor(int idx)
    {
        return Data.Rotors[idx];
    }

    Enigma enigma;

    public Bombe()
    {
        enigma = new Enigma(null, null, null);
    }
    Stopwatch watch;
public void Run(string encryptedText, string cribText)
{
    BombeData d = new BombeData();
        watch = new Stopwatch();
        watch.Start();
    //평문 탐색
    FindCrib(encryptedText, cribText, d, (data_cribed) =>
    {
        //로터 종류 탐색
        BruteForce_RotorType(data_cribed, (data_rotorType) =>
        {
            //로터 배열 탐색
            BruteForce_RotorSequence(data_rotorType, (data_rotorSeq) =>
            {
                //플러그보드 추론
                FindPlugboardSetting(data_rotorSeq);
            });
        });
    });
}

void BruteForce_RotorType(BombeData data, System.Action<BombeData> rotorsequencer)
{
    data.Rotors = new Rotor[3];
    data.relf = Data.reflector1;
    //(a, b, c) 각각은 첫번째, 두번째, 세번째 로터의 종류를 나타냄. 값의 범위는 0, 1, 2
    for (int a = 0; a < Data.Rotors.Length; a++) {
        for (int b = 0; b < Data.Rotors.Length; b++) {
            for (int c = 0; c < Data.Rotors.Length; c++) {
                //같은 종류의 로터는 무시함
                if (a == b) continue;
                if (a == c) continue;
                if (b == c) continue;
                //로터 종류 설정
                data.Rotors[0] = GetRotor(a);
                data.Rotors[1] = GetRotor(b);
                data.Rotors[2] = GetRotor(c);
                    data.RotorSeq = $"{a + 1} - {b + 1} - {c + 1}";
                rotorsequencer(data);
            }
        }
    }
}

    void FindCrib(string encryptedText, string cribText, BombeData d, System.Action<BombeData> rotorFinder)
    {
        //알려진 평문이 i만큼 밀렸다고 가정함
        //i값의 범위는 0 ~ (암호문의 길이 - 평문의 길이 + 1)로,
        //0은 알려진 평문이 밀리지 않았음을, 마지막 값은 암호문의 마지막 부분에 해당한다는 것을 뜻함
        for (int i = 0; i < encryptedText.Length - cribText.Length + 1; i++)
        {
            bool possible = true; //알려진 평문이 i만큼 밀리는 것이 가능한가?
            for (int k = 0; k < cribText.Length; k++)
            {
                //논리적으로는 '평문'이 i만큼 밀렸다고 가정하지만, 실제 구현에서는 '암호문'이 i만큼 밀렸다고 가정하는 것이 더 간단함
                //i만큼 밀린 암호문 [k + i]와 평문 [k]가 일치하는지 확인
                if (encryptedText[k + i] == cribText[k])
                {
                    //만일 두 글자가 겹친다면 불가능한 경우로 판단(possible = false)
                    possible = false;
                    break;
                }
            }
            //만약 알려진 평문이 i만큼 밀리는 것이 가능한 경우라면,
            if (possible)
            {
                Console.WriteLine();
                //출력
                Console.WriteLine(encryptedText);
                for (int k = 0; k < i; k++)
                    Console.Write(" ");
                Console.WriteLine(cribText);

                string encrpyted = encryptedText.Substring(i, cribText.Length); //평문에 해당하는 암호문의 부분만 추출
                var freq = GetCharacterFrequencySorted(encrpyted + cribText); //빈도순으로 정렬
                //출력
                for (int f = 0; f < freq.Count; f++) {
                    //Console.WriteLine($"{freq.ElementAt(f).Key} - {freq.ElementAt(f).Value}");
                }
                //정보 설정(암호문, 평문, 빈도수, 밀린 정도(offset))
                d.Encrtyp = encrpyted;
                d.Crib = cribText;
                d.freq = freq;
                d.offset = i;
                rotorFinder(d);
            }
        }
    }
void BruteForce_RotorSequence(BombeData d, System.Action<BombeData> plugboardFinder)
{
    //(r1, r2, r3) 각각은 첫번째, 두번째, 세번째 로터의 시작 지점을 나타냄
    //값의 범위는 0~25로, 각각이 알파벳에 대응됨(0 - A, 1 - B ... 25 - Z)
    for (int r1 = 0; r1 < 26; r1++) {
        Console.WriteLine($"{(char)(r1 + 'A')}");
        for (int r2 = 0; r2 < 26; ++r2) {
            for (int r3 = 0; r3 < 26; ++r3) {
                //로터의 시작 지점 설정
                d.StartPosition = $"{(char)(r1 + 'A')}{(char)(r2 + 'A')}{(char)(r3 + 'A')}";
                plugboardFinder(d);
            }
        }
    }
}
public void FindPlugboardSetting(BombeData d)
{
    var freq = d.freq; //빈도순으로 정렬된 글자들
    var offset = d.offset; //평문이 밀린 정도
    string startPosition = d.StartPosition; //로터의 시작지점
    string cribText = d.Crib; //알려진 평문
    string encrpyted = d.Encrtyp; //암호문
    d.plugboard = new PlugBoard(new Dictionary<char, char>()); //플러그보드. 초깃값은 비어있도록 설정
    var plugboard = d.plugboard;
    enigma = new Enigma(d.Rotors, d.relf, d.plugboard); //에니그마 생성

    //빈도가 가장 큰 글자부터 플러그보드의 설정을 가정
    for (int idx = 0; idx < freq.Count; idx++) {
        //빈도순으로 정렬된 그 글자는 letter에 해당함
        char letter = freq.ElementAt(idx).Key;

        //letter와 연결된 글자를 가정함(0~25 각각이 A~Z에 대응)
        for (int plug = 0; plug < 26; plug++) {
            //letter와 A~Z가 연결되어 있다고 가정
            char letter_plug = (char)(plug + 65);
            plugboard.Clear();
            plugboard.Add(letter, letter_plug); //플러그보드 연결
            //plugboard.Show();

            //에니그마의 초기(Start) 설정
            enigma.SetStartPosition(startPosition);
            enigma.Rotate(offset);
            //Console.WriteLine(startPosition);

            //암호문과 평문의 앞에서부터 각 글자를 하나하나 탐색
            for (int s = 0; s < cribText.Length; s++) {
                //enigma.ShowRotors();
                PlugBoard p = new PlugBoard(plugboard.wiring);

                //만일 암호문에 있는 글자가 플러그보드에 존재할 경우, 이 결과는 신뢰할 수 있음
                if (plugboard.Contains(encrpyted[s]))
                {
                    //암호문을 다시 에니그마에 넣어 계산된 결과는 (플러그보드만 제외된) 평문과 같음. 이 값이 c.
                    char c = enigma.EncryptCharacter(encrpyted[s]);
                    //두 결과가 같으면 플러그보드의 영향이 존재하지 않는 것이므로, 건너뜀
                    if (cribText[s] == c)
                        continue;
                    //Console.WriteLine($"Enc {encrpyted[s]} converted to {c} - {cribText[s]}, idx:{(char)('A' + enigma.Rotors[2].NowPosition)}, s:{s}");
                    //enigma.ShowRotors();

                    //두 결과가 다르면 플러그보드의 영향이 존재하는 것이라고 판단
                    //원래는 cribText[s]여야 하지만, 플러그보드의 영향을 제외하면 c가 나옴
                    //따라서 cribText[s]와 c는 플러그보드로 연결되어있다고 판단
                    //이 두 알파벳을 플러그보드의 대응 관계에 추가(Add)
                    //plugboard.Add(a, b) : a와 b를 플러그보드 대응 관계로 추가함. 만일 모순이 발생하면 false를 반환
                    if (plugboard.Add(cribText[s], c) == false)
                    {
                        //Add의 결과가 false이면 모순이 발생한 것으로 판단, 이 결과를 버림(break)
                        //이때의 모순은 이미 cribText[s]나 c가 다른 알파벳과 연결된 경우를 뜻함
                        //Console.WriteLine($"Fail Enc {encrpyted[s]} converted to {c} - {cribText[s]}, idx:{enigma.Rotors[2].NowPosition}, s:{s}"); plugboard.Show();
                        break;
                    }
                    else if (p.Contains(cribText[s], c) == false)
                    {
                        //만일 추가에 성공하고, 이전에 없던 새로운 알파벳이 추가되었으면, 암호문과 평문을 다시 처음부터 탐색
                        //암호문/평문의 글자의 번호를 나타내는 값은 s인데, 값이 구조상 나중에 +1이 될 것을 고려해 -1로 설정
                        s = -1;
                        //암호문과 평문을 처음부터 탐색하는 것에 맞추어, 에니그마 또한 초깃값으로 재설정
                        enigma.SetStartPosition(startPosition);
                        enigma.Rotate(offset);
                        continue;
                    }
                }
                //만일 평문에 있는 글자가 플러그보드에 존재할 경우, 이 결과는 신뢰할 수 있음
                else if (plugboard.Contains(cribText[s]))
                {
                    //평문을 다시 에니그마에 넣어 계산된 결과는 (플러그보드만 제외된) 암호문과 같음. 이 값이 c.
                    char c = enigma.EncryptCharacter(cribText[s]);
                    //두 결과가 같으면 플러그보드의 영향이 존재하지 않는 것이므로, 건너뜀
                    if (encrpyted[s] == c)
                        continue;
                    //Console.WriteLine($"Crb {cribText[s]} converted to {c} - {encrpyted[s]}, idx:{(char)('A' + enigma.Rotors[2].NowPosition)}, s:{s}");
                    //enigma.ShowRotors();

                    //위와 동일
                    if (plugboard.Add(encrpyted[s], c) == false)
                    {
                        //위와 동일
                        //Console.WriteLine($"Fail Crb {cribText[s]} converted to {c} - {encrpyted[s]}, idx:{enigma.Rotors[2].NowPosition}, s:{s}"); plugboard.Show();
                        break;
                    }
                    else if (p.Contains(encrpyted[s], c) == false)
                    {
                        //위와 동일
                        s = -1;
                        enigma.SetStartPosition(startPosition);
                        enigma.Rotate(offset);
                        continue;
                    }
                }
                //아무 경우에도 해당하지 않으면, 그냥 건너 뜀
                else
                    enigma.Rotate(1);
            }

            //만일 지금까지 추론한 에니그마의 설정이 평문을 완벽하게 암호분으로 변환한다면, 유효한 설정이라고 판단
            enigma.SetStartPosition(startPosition);
            enigma.Rotate(offset);
            string tmp = enigma.EncryptString(cribText);
            if (tmp == encrpyted)
            {
                Console.WriteLine("=============================");
                plugboard.Show();
                    Console.WriteLine(d.RotorSeq);
                Console.WriteLine(startPosition);
                Console.WriteLine(tmp);
                Console.WriteLine(encrpyted);
                    watch.Stop();
                    Console.WriteLine("Elapsed Time : {0}", (watch.ElapsedMilliseconds) / 1000f);
                    watch.Restart();

            }
        }
    }
}

    public static Dictionary<char, int> GetCharacterFrequencySorted(string input)
    {
        // Step 1: Count the frequency of each character
        Dictionary<char, int> charCount = new Dictionary<char, int>();
        foreach (char c in input)
        {
            if (charCount.ContainsKey(c))
            {
                charCount[c]++;
            }
            else
            {
                charCount[c] = 1;
            }
        }

        // Step 2: Sort the dictionary by value in descending order
        var sortedCharCount = charCount.OrderByDescending(x => x.Value)
                                        .ThenBy(x => x.Key)
                                        .ToDictionary(x => x.Key, x => x.Value);

        return sortedCharCount;
    }
    public static string PreProcessString(string input)
    {
        return input.Trim(' ').ToUpper();
    }
}
public class Data
{
    public static Rotor rotor1 = new Rotor("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'Q');
    public static Rotor rotor2 = new Rotor("AJDKSIRUXBLHWTMCQGZNPYFVOE", 'E');
    public static Rotor rotor3 = new Rotor("BDFHJLCPRTXVZNYEIWGAKMUSQO", 'V');
    public string RotorSeq;
    public static Rotor[] Rotors = new Rotor[]
    {
        rotor1, rotor2, rotor3
    };

    public static Reflector reflector1 = new Reflector("EJMZALYXVBWFCRQUONTSPIKHGD");
}
public class Program
{
    public static Rotor rotor1
    {
        get { return Data.rotor1; }
    }
    public static Rotor rotor2
    {
        get { return Data.rotor2; }
    }
    public static Rotor rotor3
    {
        get { return Data.rotor3; }
    }
    public static Reflector reflector1
    {
        get { return Data.reflector1; }
    }
    public static void Main(string[] args)
    {
        //PlugBoard plugBoard = new PlugBoard(new Dictionary<char, char>
        //{
        //    { 'A', 'D' }, {'S', 'F'}, {'W', 'L'}, {'E', 'K'}, {'X', 'C'}
        //});
PlugBoard plugBoard = new PlugBoard(new Dictionary<char, char>
{
    { 'D', 'F' }, {'W', 'Q'}, {'E', 'V'}, {'N', 'O'}, {'J', 'S'},
    {'T', 'K'}, {'I', 'Z'}, {'B', 'G'}, {'R', 'P'}, {'M', 'A'}
});

Enigma enigma = new Enigma(new Rotor[]
{
    rotor1, rotor3, rotor2
}, reflector1, plugBoard);
enigma.SetStartPosition("FPB");

        enigma.ShowRotors();
        plugBoard.Show();

string inputText = "Lorem ipsum dolor sit amet consectetur adipisicing elit sed do eiusmod tempor";
string encryptedText = enigma.EncryptString(PreProcessString(inputText));

Console.WriteLine($"Enigma:{PreProcessString(inputText)} -- {encryptedText}");

string crib_de = PreProcessString("ipsum dolor sit amet");

Bombe bombe = new Bombe();
bombe.Run(encryptedText, crib_de);
    }

    public static string PreProcessString(string input)
    {
        string s = input.Replace(" ", string.Empty);
        Console.Write(s);
        return s.ToUpper();
    }
}
