using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessingPass : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public Material passMaterial;
        private RenderTargetIdentifier currentTarget;

        public CustomRenderPass(Material material)
        {
            this.passMaterial = material;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (passMaterial == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("Custom Post Processing");
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            int tempTarget = Shader.PropertyToID("_TempTarget");
            cmd.GetTemporaryRT(tempTarget, opaqueDesc);

            cmd.Blit(source, tempTarget);
            cmd.Blit(tempTarget, source, passMaterial);
            cmd.ReleaseTemporaryRT(tempTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    CustomRenderPass m_ScriptablePass;
    public Material material;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(material);
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
