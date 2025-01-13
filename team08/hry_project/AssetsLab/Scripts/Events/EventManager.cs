using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// EventManager implements singleton pattern. This implementation is not thread safe.
/// </summary>
public class EventManager : MonoBehaviour
{

    public UnityEvent OnEnemyDestroyed = new UnityEvent();

    private static EventManager _instance = null;

    /// <summary>
    /// If there is no game object with EventManager component,
    /// new object is created. This means, EventManager. Instance should
    /// be called from Unity thread or it must be certain, that the class
    /// was already instantiated inside the Unity thread. Returns null
    /// when exiting application.
    /// </summary>
    public static EventManager Instance
    {
        get
        {
            if (_applicationIsQuitting)
                return null;


            if (_instance == null)
            {
                // Try to find it in the scene
                _instance = FindObjectOfType<EventManager>();
                if (_instance == null)
                {
                    // Doesn't exist in the scene, so create it
                    GameObject go = new GameObject("EventManager");
                    _instance = go.AddComponent<EventManager>();
                }
            }

            return _instance;
        }
    }

    private static bool _applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application or scene quits.
    /// If any script calls Instance after it have been destroyed, 
    /// it will create a buggy ghost object that will stay on the Editor scene
    /// even after stopping playing the Application. Bad script! This prevents
    /// such nasty behaviour.
    /// </summary>
    public void OnDestroy()
    {
        _applicationIsQuitting = true;
    }

}
