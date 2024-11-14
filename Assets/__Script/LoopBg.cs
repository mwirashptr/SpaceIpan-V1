using UnityEngine;

public class LoopBg : MonoBehaviour
{
    public float loopSpeed;
    public Renderer bgRenderer;

    // Update is called once per frame
    private void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(loopSpeed * Time.deltaTime, 0f);
    }
}