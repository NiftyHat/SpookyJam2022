using System;
using System.Collections.Generic;
using Data.Monsters;

public class CharacterTypeGuess
{
    private Dictionary<MonsterEntityTypeData, Guess> _guessMonster = new Dictionary<MonsterEntityTypeData, Guess>();
    public Dictionary<MonsterEntityTypeData, Guess> GuessMonster => _guessMonster;
    private Guess _guessKiller;
    public Guess Killer => _guessKiller;

    public event Action<CharacterTypeGuess> OnChange;

    public CharacterTypeGuess()
    {
    }

    public void Set(MonsterEntityTypeData monsterType, Guess guess)
    {
        _guessMonster[monsterType] = guess;
        OnChange?.Invoke(this);
    }

    public void SetMonster(Dictionary<MonsterEntityTypeData, Guess> guessData)
    {
        _guessMonster = guessData;
        OnChange?.Invoke(this);
    }
    
    public Guess GetMonsterGuess(MonsterEntityTypeData item)
    {
        if (_guessMonster.TryGetValue(item, out Guess guess))
        {
            return guess;
        }
        return Guess.None;
    }
    
}