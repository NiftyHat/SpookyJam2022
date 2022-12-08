using System;
using System.Collections.Generic;
using Data.Monsters;

public class CharacterTypeGuess
{
    private Dictionary<MonsterEntityTypeData, Guess> _guessMonster = new Dictionary<MonsterEntityTypeData, Guess>();
    public Dictionary<MonsterEntityTypeData, Guess> GuessMonster => _guessMonster;
    private Guess _guessHuman;
    public Guess Human => _guessHuman;
    private Guess _guessKiller;
    public Guess Killer => _guessKiller;
    
    public event Action<CharacterTypeGuess> OnChange;

    public void Set(MonsterEntityTypeData monsterType, Guess guess)
    {
        _guessMonster[monsterType] = guess;
        OnChange?.Invoke(this);
    }

    public void SetHuman(Guess guess)
    {
        _guessHuman = guess;
        OnChange?.Invoke(this);
    }

    public void SetKiller(Guess guess)
    {
        _guessKiller = guess;
         OnChange?.Invoke(this);
    }

    public void SetMonster(Dictionary<MonsterEntityTypeData, Guess> guessData)
    {
        _guessMonster = guessData;
        OnChange.Invoke(this);
    }
}