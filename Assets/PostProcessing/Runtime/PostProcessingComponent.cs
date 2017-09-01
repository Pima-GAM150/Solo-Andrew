using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
    public abstract class PostProcessingComponent<T> : PostProcessingComponentBase
        where T : PostProcessingModel
    {
        #region Public Properties

        public T model { get; internal set; }

        #endregion Public Properties

        #region Public Methods

        public override PostProcessingModel GetModel()
        {
            return model;
        }

        public virtual void Init(PostProcessingContext pcontext, T pmodel)
        {
            context = pcontext;
            model = pmodel;
        }

        #endregion Public Methods
    }

    public abstract class PostProcessingComponentBase
    {
        #region Public Fields

        public PostProcessingContext context;

        #endregion Public Fields

        #region Public Properties

        public abstract bool active { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.None;
        }

        public abstract PostProcessingModel GetModel();

        public virtual void OnDisable()
        { }

        public virtual void OnEnable()
        { }

        #endregion Public Methods
    }

    public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T>
        where T : PostProcessingModel
    {
        #region Public Methods

        public abstract CameraEvent GetCameraEvent();

        public abstract string GetName();

        public abstract void PopulateCommandBuffer(CommandBuffer cb);

        #endregion Public Methods
    }

    public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T>
        where T : PostProcessingModel
    {
        #region Public Methods

        public virtual void Prepare(Material material)
        { }

        #endregion Public Methods
    }
}