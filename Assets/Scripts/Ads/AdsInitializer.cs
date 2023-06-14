using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// An object for initializing advertisements
/// </summary>
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    [SerializeField] RewardedAdsButton AdsButton;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        if (!Advertisement.isInitialized)
        {
            Debug.Log("Initializing Unity Ads...");
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        else
        {
            AdsButton.Initialize();
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        AdsButton.Initialize();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");

        // Try again after 10 seconds
        Task retry = new Task(async () =>
        {
            await Task.Delay(10000);
            Advertisement.Initialize(_gameId, _testMode, this);
        });
        retry.Start();
    }
}
