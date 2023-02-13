using UnityEngine;

/// <summary>
/// An object for playing music
/// </summary>
public class MusicBox
{
    /// <summary>Creates a new instance of a MusicBox</summary>
    /// <param name="music">The music to be played by this music box</param>
    public MusicBox(AudioSource[] music)
    {
        Music= music;
    }

    /// <summary>The music this music box can play</summary>
    private AudioSource[] Music { get; }

    /// <summary>The index of the music currently being played</summary>
    /// <remarks>-1 when no music is being played</remarks>
    private int CurrentMusic { get; set; } = -1;

    /// <summary>Plays the music at the given index</summary>
    /// <param name="index">The index of the music to play</param>
    public void PlayMusic(int index)
    {
        // Change the music if a new song is requested
        if (CurrentMusic != index && index > 0 && index < Music.Length)
        {
            StopMusic();
            CurrentMusic = index;
            Music[CurrentMusic].Play();
        }
    }

    /// <summary>Stops any music that's playing</summary>
    public void StopMusic()
    {
        if (CurrentMusic > -1)
        {
            Music[CurrentMusic].Stop();
            CurrentMusic = -1;
        }
    }
}
