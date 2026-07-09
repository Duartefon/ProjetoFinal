using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Scenes.TP3.Scripts.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TypewriterEffect : MonoBehaviour
    {
        [Header("Test String")] [SerializeField]
        private string testText;

        private TMP_Text _textBox;
        private int _currentVisibleCharacterIndex;
        private Coroutine _typewriterCoroutine;
        private bool _readyForNewText = true;

        private WaitForSeconds _simpleDelay;
        private WaitForSeconds _interpunctuationDelay;

        [Header("Typewriter Settings")] 
        [SerializeField] private float charactersPerSecond = 20;
        [SerializeField] private float interpunctuationDelay = 0.5f;

    
        public bool CurrentlySkipping { get; private set; }
        private WaitForSeconds _skipDelay;
    
        [Header("Skip options")] [SerializeField]
        private bool quickSkip;

        [SerializeField] [Min(1)] private int skipSpeedup = 5;

        private WaitForSeconds _textboxFullEventDelay;
        [SerializeField] [Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f;
        private static event Action CompleteTextRevealed;
        private static event Action<char> CharacterRevealed;
    
 

        private void Awake()
        {
            _textBox = GetComponent<TMP_Text>();
            _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
            _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

            _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));
            _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);


        }

    
        public void PrepareForNewText(Object  obj)
        {

            if (!_readyForNewText || obj != _textBox ) return;
                _readyForNewText = false;
            if(_typewriterCoroutine != null)
                StopCoroutine((_typewriterCoroutine));

            _textBox.maxVisibleCharacters = 0;
            _currentVisibleCharacterIndex = 0;

            _typewriterCoroutine = StartCoroutine(Typewriter());
        }

        private IEnumerator Typewriter()
        {
            TMP_TextInfo textInfo = _textBox.textInfo;
        
            while (_currentVisibleCharacterIndex < textInfo.characterCount + 1)
            {
                var lastCharacterIndex = textInfo.characterCount - 1;

                if (_currentVisibleCharacterIndex == lastCharacterIndex)
                {
                    _textBox.maxVisibleCharacters++;
                    yield return _textboxFullEventDelay;
                    CompleteTextRevealed?.Invoke();
                    _readyForNewText = true;
                    yield break;
                }
            
                char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

                _textBox.maxVisibleCharacters++;

                if (!CurrentlySkipping && "?.,:,!-".Contains(character))
                {
                    yield return _interpunctuationDelay;
                }

                else
                {
                    yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
                }
            
                CharacterRevealed?.Invoke(character);
                _currentVisibleCharacterIndex++;
            }
        }

        void Skip()
        {
            if (CurrentlySkipping)
                return;
            CurrentlySkipping = true;

            if (!quickSkip)
            {
                StartCoroutine(SkipSpeedupReset());
                return;
            }
        
            StopCoroutine(_typewriterCoroutine);
            _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
            _readyForNewText = true;
            CompleteTextRevealed?.Invoke();
        }

        private IEnumerator SkipSpeedupReset()
        {
            yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
            CurrentlySkipping = false;
        }
        
        
        public void OnRightMouseClick()
        {
            if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
                Skip();
        }

        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);

        }
    }
}