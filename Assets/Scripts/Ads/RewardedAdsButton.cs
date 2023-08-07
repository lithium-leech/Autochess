using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using System.Threading.Tasks;

/// <summary>
/// A button that runs an add before providing its function
/// </summary>
public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] BasicButton _showAdButton;
#if UNITY_IOS
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
#elif UNITY_ANDROID
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
#endif
    string _adUnitId = null; // This will remain null for unsupported platforms

    // An event that is triggered when a reward is granted
    public UnityAction onReward;

    // Initializes ads for this button, after unity ads have been initialized
    public void Initialize()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
        _showAdButton.SetEnabled(false);

        // Start loading the first ad
        LoadAd();
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log($"Loading Ad {_adUnitId}...");
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log($"Completed loading Ad {adUnitId}.");
        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
            
            // Enable the button for users to click:
            _showAdButton.SetEnabled(true);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load Ad {adUnitId}: {error} - {message}");
        // TODO: Use the error details to determine whether to try to load another ad
        // Try again after 10 seconds
        Task retry = new(async () =>
        {
            await Task.Delay(10000);
            Advertisement.Load(_adUnitId, this);
        });
        retry.Start();
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showAdButton.SetEnabled(false);
        _showAdButton.onClick.RemoveAllListeners();

        // Then show the ad:
        Debug.Log($"Showing Ad {_adUnitId}...");
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log($"Finished showing Ad {_adUnitId}.");
            
            // Grant a reward.
            onReward.Invoke();
            
            // Load another ad
            Advertisement.Load(_adUnitId, this);
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Failed to show Ad {adUnitId}: {error} - {message}");
        // TODO: Use the error details to determine whether to try to load another ad
        // Move on as if the ad was shown
        OnUnityAdsShowComplete(adUnitId, UnityAdsShowCompletionState.COMPLETED);
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}
