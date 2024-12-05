using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.Platform2D.Entity
{
    /// <summary>
    /// An implementation of IEntity that provides base functionality to be extended in a game using the Platform2D engine.
    /// </summary>
    public class EntityBase : IEntity, IHostedEntity
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="host">The IEngine object this entity belongs to.</param>
        public EntityBase(IEngine host)
        {
            this.Host = host ?? throw new ArgumentNullException("host");

            IUniqueIdGenerator idGen = this.Host.GetComponent<IUniqueIdGenerator>();

            if (idGen == null)
                throw new ArgumentNullException("The supplied host does not have a component registered for IUniqueIdGenerator, which is required in order to create Entity objects. You can create your own implementation, or you can use the Platform2D.Engine.UniqueIdGenerator implemenation.");

            this.UniqueId = idGen.GenerateUniqueId();
        }

        #region IHostedEntity Implementation
        /// <summary>
        /// The engine this scene belongs to.
        /// </summary>
        public IEngine Host { get; private set; }
        #endregion

        #region IEntity Impelementation
        /// <summary>
        /// An event that fires when this entity is disposed.
        /// </summary>
        public event EventHandler<EventArgs> Disposed;

        /// <summary>
        /// Whether or not this entity is alive, or if it can be removed from processing.
        /// </summary>
        public bool Alive { get; set; } = true;

        /// <summary>
        /// A unique identifier generated when the entity is created.
        /// </summary>
        public string UniqueId { get; private set; }

        /// <summary>
        /// Whether or not this entity has been disposed.
        /// </summary>
        public bool IsDisposed { get { return _isDisposed; } }

        /// <summary>
        /// The layer that the entity exists on.
        /// </summary>
        public IEntityContainer OwningContainer { get; set; } = null;

        /// <summary>
        /// Whether or not this entity should be updated.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    OnEnabledChanged();
                }
            }
        }
        private bool _enabled = true;

        /// <summary>
        /// Whether or not this entity should be drawn.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (value != _visible)
                {
                    _visible = value;
                    OnVisibleChanged();
                }
            }
        }
        private bool _visible = true;

        /// <summary>
        /// The order in which this scene should be updated.
        /// </summary>
        public int UpdateOrder
        {
            get { return _updateOrder; }
            set
            {
                if (value != _updateOrder)
                {
                    _updateOrder = value;
                    OnUpdateOrderChanged();
                }
            }
        }
        private int _updateOrder = 0;

        /// <summary>
        /// The order in which this scene should be drawn.
        /// </summary>
        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                if (value != _drawOrder)
                {
                    _drawOrder = value;
                    OnDrawOrderChanged();
                }
            }
        }
        private int _drawOrder = 0;

        /// <summary>
        /// An event that fires before drawing begins.
        /// </summary>
        public event EventHandler<ValueEventArgs<GameTime>> DrawBegin;

        /// <summary>
        /// An event that fires before drawing ends.
        /// </summary>
        public event EventHandler<ValueEventArgs<GameTime>> DrawEnd;

        /// <summary>
        /// Fired when the value of Enabled changes.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Fired when the value of UpdateOrder changes.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Fired when the value of DrawOrder changes.
        /// </summary>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>
        /// Fired when the value of Visible changes.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Update this scene.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        /// <summary>
        /// Preform any rendering that should be done before the core rendering pass. This typically includes things like rendering to different targets.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public void PreDraw(GameTime gameTime)
        {
            OnPreDraw(gameTime);
        }

        /// <summary>
        /// Draw this scene.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public void Draw(GameTime gameTime)
        {
            OnDrawBegin(gameTime);
            OnDraw(gameTime);
            OnDrawEnd(gameTime);
        }

        /// <summary>
        /// Draw this scene to the specified render target. This should not be called while any render objects, such as an ISpriteBatch object, are open. A good place to call this would be from PreDraw :)
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        /// <param name="target">The target to draw to.</param>
        /// <param name="clearColour">If set, clears the graphics device with the specified colour.</param>
        public void Draw(GameTime gameTime, IRenderTarget2D target, Color? clearColour = null)
        {
            this.Host.Graphics.GraphicsDevice.SetRenderTarget(target);

            if (clearColour != null)
                this.Host.Graphics.GraphicsDevice.Clear(clearColour.Value);

            this.Draw(gameTime);

            this.Host.Graphics.GraphicsDevice.SetRenderTarget(null);
        }
        #endregion

        #region IDisposable Implementation
        private bool _isDisposed = false; // To detect redundant calls

        /// <summary>
        /// A method that is called when this object disposes.
        /// </summary>
        /// <param name="disposing">True for disposing from managed code, false otherwise.</param>
        protected virtual void OnDispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (this.OwningContainer != null)
                        this.OwningContainer.RemoveEntity(this);
                }

                _isDisposed = true;

                if (this.Disposed != null)
                    this.Disposed(this, new EventArgs());
            }
        }

        /// <summary>
        /// Dispose of this object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            OnDispose(true);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Occurs when the value of Enabled has changed.
        /// </summary>
        protected void OnEnabledChanged()
        {
            if (this.EnabledChanged != null)
                this.EnabledChanged(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the value of UpdateOrder has changed.
        /// </summary>
        protected void OnUpdateOrderChanged()
        {
            if (this.UpdateOrderChanged != null)
                this.UpdateOrderChanged(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the value of DrawOrder has changed.
        /// </summary>
        protected void OnDrawOrderChanged()
        {
            if (this.DrawOrderChanged != null)
                this.DrawOrderChanged(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the value of Visible has changed.
        /// </summary>
        protected void OnVisibleChanged()
        {
            if (this.VisibleChanged != null)
                this.VisibleChanged(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when Update is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected virtual void OnUpdate(GameTime gameTime) { }

        /// <summary>
        /// Occurs when PreDraw is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected virtual void OnPreDraw(GameTime gameTime) { }

        /// <summary>
        /// Occurs when Draw is called.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected virtual void OnDraw(GameTime gameTime) { }

        /// <summary>
        /// Occurs before drawing begins.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected virtual void OnDrawBegin(GameTime gameTime)
        {
            if (this.DrawBegin != null)
                this.DrawBegin(this, new ValueEventArgs<GameTime>(gameTime));
        }

        /// <summary>
        /// Occurs when drawing ends.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        protected virtual void OnDrawEnd(GameTime gameTime)
        {
            if (this.DrawEnd != null)
                this.DrawEnd(this, new ValueEventArgs<GameTime>(gameTime));
        }
        #endregion
    }
}
