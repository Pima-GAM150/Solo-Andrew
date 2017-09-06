using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
    public abstract class PostProcessingComponent<T> : PostProcessingComponentBase
        where T : PostProcessingModel
    {
        public T model { get; internal set; }

        public override PostProcessingModel GetModel()
        {
            return model;
        }

        public virtual void Init(PostProcessingContext pcontext, T pmodel)
        {
            context = pcontext;
            model = pmodel;
        }
    }

    public abstract class PostProcessingComponentBase
    {
        public abstract bool active { get; }
        public PostProcessingContext context;

        public virtual DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.None;
        }

        public abstract PostProcessingModel GetModel();

        public virtual void OnDisable()
        { }

        public virtual void OnEnable()
        { }
    }

    public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T>
        where T : PostProcessingModel
    {
        public abstract CameraEvent GetCameraEvent();

        public abstract string GetName();

        public abstract void PopulateCommandBuffer(CommandBuffer cb);
    }

    public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T>
        where T : PostProcessingModel
    {
        public virtual void Prepare(Material material)
        { }
    }
}