using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Audio.Factory;
using ArbitraryPixel.Common.ContentManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.Platform2D.Audio
{
    /// <summary>
    /// A simple implementation of ISoundController, providing the ability to create sound resources and provide basic control over them.
    /// </summary>
    public class SoundController : ISoundController
    {
        private bool _enabled = true;
        private float _volume = 1f;
        private ISoundResourceFactory _factory;
        private List<ISoundResource> _ownedResources = new List<ISoundResource>();

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="factory">An object responsible for creating ISoundResource objects.</param>
        public SoundController(ISoundResourceFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException();
        }

        #region ISoundController Implementation
        /// <summary>
        /// An event that occurs when this sound control's enabled state changes.
        /// </summary>
        public event EventHandler<StateChangedEventArgs<bool>> EnabledChanged;

        /// <summary>
        /// Whether or not sounds created by this controller should play audibly.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value != _enabled)
                {
                    bool previousValue = _enabled;

                    _enabled = value;

                    foreach (ISoundResource resource in _ownedResources)
                        resource.Enabled = _enabled;

                    OnEnabledChanged(new StateChangedEventArgs<bool>(previousValue, _enabled));
                }
            }
        }

        /// <summary>
        /// The volume for this controller and all resources it has created.
        /// </summary>
        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;

                foreach (ISoundResource resource in _ownedResources)
                    resource.MasterVolume = _volume;
            }
        }

        /// <summary>
        /// Create a new resource to be controlled by this controller.
        /// </summary>
        /// <param name="content">An object responsible for managing content.</param>
        /// <param name="assetName">The name of the sound resource asset to create.</param>
        /// <returns>The newly created resource.</returns>
        public ISoundResource CreateSoundResource(IContentManager content, string assetName)
        {
            ISoundResource sound = _factory.Create(content, assetName);
            sound.Enabled = this.Enabled;
            sound.MasterVolume = this.Volume;
            sound.Disposed += Handle_SoundResourceDisposed;

            _ownedResources.Add(sound);

            return sound;
        }

        /// <summary>
        /// Stop all sounds controlled by this controller.
        /// </summary>
        public void StopAll()
        {
            foreach (ISoundResource resource in _ownedResources)
                resource.StopAll();
        }

        /// <summary>
        /// Dispose of any resources this controller has created.
        /// </summary>
        public void ClearOwnedResources()
        {
            if (_ownedResources.Count > 0)
            {
                var ownedResources = new List<ISoundResource>(_ownedResources);
                foreach (ISoundResource resource in _ownedResources)
                    resource.Dispose();
            }
        }

        /// <summary>
        /// Update this controller.
        /// </summary>
        /// <param name="gameTime">The current time state for the game.</param>
        public void Update(GameTime gameTime)
        {
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Occurs when the state of Enabled for this object changes.
        /// </summary>
        /// <param name="e">Arguments for this event, containing both the previous and the new state.</param>
        protected virtual void OnEnabledChanged(StateChangedEventArgs<bool> e)
        {
            if (this.EnabledChanged != null)
                this.EnabledChanged(this, e);
        }
        #endregion

        #region Private Methods
        private void Handle_SoundResourceDisposed(object sender, EventArgs e)
        {
            if (sender is ISoundResource && _ownedResources.Contains((ISoundResource)sender))
                _ownedResources.Remove((ISoundResource)sender);
        }
        #endregion
    }
}
