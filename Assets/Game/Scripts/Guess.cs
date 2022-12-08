public enum Guess
{
    None,
    Yes,
    No
}

public class GuessUtils
{
    public static Guess Next(Guess guessValue)
    {
        switch (guessValue)
        {
            case Guess.None:
                return Guess.No;
            case Guess.No:
                return Guess.Yes;
            case Guess.Yes:
                return Guess.None;
        }
        return Guess.None;
    }
}

