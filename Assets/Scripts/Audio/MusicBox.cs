using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object for playing music
/// </summary>
public class MusicBox : MonoBehaviour
{
    /// Properties to set using Unity interface
    public Song[] Songs;

    /// <summary>A collection of playable audio sources</summary>
    private IDictionary<SongName, AudioSource> Sources { get; } = new Dictionary<SongName, AudioSource>();

    /// <summary>The music currently being played</summary>
    private SongName CurrentSong { get; set; } = SongName.None;

    /// <summary>The volume level to use for audio sources</summary>
    public float Volume
    {
        get { return _volume; }
        set
        {
            _volume = value;
            foreach (Song song in Songs)
                song.Source.volume = value;
        }
    }
    private float _volume = 0.7f;

    public void Awake()
    {
        foreach (Song song in Songs)
            Sources.Add(song.Name, song.Source);
    }

    /// <summary>Plays the music with the given name</summary>
    /// <param name="name">The name of the song to play</param>
    public void PlayMusic(SongName name)
    {

        // Stop the music if none is given
        if (name == SongName.None)
            StopMusic();
        // Change the music if a new song is requested
        else if (name != CurrentSong)
        {
            StopMusic();
            Debug.Log($"Playing {name} music...");
            CurrentSong = name;
            Sources[CurrentSong].Play();
        }
    }

    /// <summary>Stops any music that's playing</summary>
    public void StopMusic()
    {
        if (CurrentSong != SongName.None)
        {
            Debug.Log($"Stopping music...");
            Sources[CurrentSong].Stop();
            CurrentSong = SongName.None;
        }
    }
}
