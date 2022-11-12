using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    [Tooltip("How fast should the texture be faded out?")]
    [SerializeField] protected float _fadeTime;
 
    [Tooltip("How long will the screen stay black?")]
    [SerializeField] protected float _minDuration;
 
    [Tooltip("Choose the color, which will fill the screen.")]
    [SerializeField] protected Color _color;
    
    private Texture2D _texture;
 
    private float _passedBlackScreenTime;
    private FadeState _state = FadeState.None;
    public enum FadeState
    {
        None,
        In,
        Out
    }

    private void Start()
    {
        _texture = new Texture2D(1, 1);
        SetTextureAlpha(0);
    }
    
    [ContextMenu("TestFade")]
    public void TestFade()
    {
        //DoFadeOut(() => DoFadeIn(() => Debug.Log("FadeOutDone")));
        AsyncLoadTransition(null);
    }

    private void HandleFadeAlphaChanged(float alpha)
    {
        _texture.SetPixel(0, 0, new Color(_color.r, _color.g, _color.b, alpha));
        _texture.Apply();
    }

    public void OnGUI()
    {
        if (_texture != null && _state != FadeState.None)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
        }
    }
 
    private void SetTextureAlpha(float alpha)
    {
        _texture.SetPixel(0, 0, new Color(_color.r, _color.g, _color.b, alpha));
        _texture.Apply();
    }

    public async Task AsyncOut()
    {
        Tween tween = DOTween.To(HandleFadeAlphaChanged, 0, 1, _fadeTime).SetEase(Ease.OutCubic);
        await Task.Run(() => { _state = FadeState.Out; });
        await tween.AsyncWaitForCompletion();
    }
    
    public async Task AsyncIn()
    {
        Tween tween = DOTween.To(HandleFadeAlphaChanged, 1, 0, _fadeTime).SetEase(Ease.Flash);
        await Task.Run(() => { _state = FadeState.In; });
        await tween.AsyncWaitForCompletion();
    }
    
    public async void AsyncLoadTransition(Task loadTask)
    {
        var waitMinDuration = Task.Delay((int)_minDuration * 100);
        await AsyncOut();
        if (loadTask != null)
        {
            await Task.WhenAll(waitMinDuration, loadTask);
        }
        else
        {
            await waitMinDuration;
        }
        await AsyncIn();
        await Task.Run(() => { _state = FadeState.None; });
    }
}
