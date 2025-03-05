using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenLineRenderFeature : ScriptableRendererFeature
{
    class ScreenLinePass : ScriptableRenderPass
    {
        private Material lineMaterial;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;
        public Vector2 LightSource; // 敵の位置

        public ScreenLinePass(Material material)
        {
            this.lineMaterial = material;
            tempTexture.Init("_TempScreenTexture");
        }

        public void SetLightSource(Vector2 position)
        {
            LightSource = position;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            source = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(tempTexture.id, descriptor, FilterMode.Bilinear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (lineMaterial == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("ScreenLinePass");

            // 敵の位置をシェーダーにセット
            lineMaterial.SetVector("_LightSource", LightSource);

            // 画面に描画
            Blit(cmd, source, tempTexture.Identifier(), lineMaterial);
            Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    public Material lineMaterial;
    private ScreenLinePass screenLinePass;

    public override void Create()
    {
        screenLinePass = new ScreenLinePass(lineMaterial) {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };
    }

    public void SetLightSource(Vector2 position)
    {
        screenLinePass.SetLightSource(position);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(screenLinePass);
    }
}
