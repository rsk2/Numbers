﻿namespace NumbersHelper;

public class NumbersLib
{
    #region Constants
    public const int DefaultMax = 100;
    public const int DefaultMin = 1;
    #endregion

    #region Constructors
    public NumbersLib() : this(DefaultMin, DefaultMax) {}

    public NumbersLib(int minimum, int maximum)
    {
        MinimumNumber = minimum;
        MaximumNumber = maximum;
        ThresholdHigh = MaximumNumber;
        ThresholdLow  = MinimumNumber;
        var rnd = new Random();
        SecretNumber = rnd.Next(MinimumNumber, MaximumNumber + 1);
    }
    #endregion

    #region Properties
    public bool CorrectGuess    {get; private set;}
    public int  CurrentGuess    {get; private set;}
    public bool Forfeit         {get; private set;}
    public int  MaximumNumber   {get;}
    public int  MinimumNumber   {get;}
    public int  NumberOfGuesses {get; private set;}

    public int SecretNumber  {get;}
    public int ThresholdHigh {get; private set;}
    public int ThresholdLow  {get; private set;}
    #endregion

    public GuessResult MakeGuess(string guess)
    {
        _ = int.TryParse(guess, out var guessResult);
        return MakeGuess(guessResult);
    }

    /// <summary>Checks the user's guess against the secret number.</summary>
    /// <param name="guess">The current number being guessed.</param>
    /// <returns>
    ///     A <see cref="GuessResult" /> indicating the relationship between the guess and the secret number.
    /// </returns>
    /// <remarks>
    ///     Possible values are:
    ///     CorrectGuess: the user guess the correct number
    ///     Higher: the user guessed higher than the secret number
    ///     Lower: the user guessed lower than the secret number
    ///     PreviousGuess: The user guessed this number previously
    ///     PreviousGuessHigher: The user guessed a lower number already - this guess is already outside the possible range
    ///     PreviousGuessLower The user guessed a higher number already - this guess is already outside the possible range
    ///     Forfeit: the user entered 0 (or invalid input) - assume they wish to exit the game
    /// </remarks>
    public GuessResult MakeGuess(int guess)
    {
        NumberOfGuesses++;
        CurrentGuess = guess;

        if (guess == 0)
        {
            Forfeit = true;
            return GuessResult.Forfeit;
        }

        if (CurrentGuess == SecretNumber)
        {
            CorrectGuess = true;
            return GuessResult.CorrectGuess;
        }

        if (CurrentGuess < ThresholdLow)
        {
            return GuessResult.PreviousGuessLower;
        }

        if (CurrentGuess > ThresholdHigh)
        {
            return GuessResult.PreviousGuessHigher;
        }

        if (CurrentGuess < SecretNumber)
        {
            ThresholdLow = CurrentGuess;
            return GuessResult.Higher;
        }

        ThresholdHigh = CurrentGuess;
        return GuessResult.Lower;
    }
}