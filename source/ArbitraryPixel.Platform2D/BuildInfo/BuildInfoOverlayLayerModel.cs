using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Text;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitraryPixel.Platform2D.BuildInfo
{
    /// <summary>
    /// Represents data for a BuildInfoOverlayLayer.
    /// </summary>
    public interface IBuildInfoOverlayLayerModel
    {
        /// <summary>
        /// The top-left of the text block for build information.
        /// </summary>
        Vector2 TextAnchor { get; set; }

        /// <summary>
        /// The text objects that make up the text block for build information.
        /// </summary>
        List<ITextObject> TextObjects { get; }

        /// <summary>
        /// The overall size of the text block for build information.
        /// </summary>
        SizeF TextSize { get; }
    }

    /// <summary>
    /// Data model for a BuildInfoOverlayLayer object.
    /// </summary>
    public class BuildInfoOverlayLayerModel : IBuildInfoOverlayLayerModel
    {
        private Vector2 _textAnchor = Vector2.Zero;

        #region IBuildInfoOverlayLayerModel Implementation
        /// <summary>
        /// The top-left of the text block for build information.
        /// </summary>
        public Vector2 TextAnchor
        {
            get { return _textAnchor; }
            set
            {
                Vector2 offset = value - _textAnchor;

                _textAnchor = value;

                foreach (ITextObject textObject in this.TextObjects)
                    textObject.Location += offset;
            }
        }

        /// <summary>
        /// The text objects that make up the text block for build information.
        /// </summary>
        public List<ITextObject> TextObjects { get; } = new List<ITextObject>();

        /// <summary>
        /// The overall size of the text block for build information.
        /// </summary>
        public SizeF TextSize
        {
            get
            {
                SizeF blockSize = SizeF.Empty;
                foreach (ITextObject textObject in this.TextObjects)
                {
                    SizeF textSize = textObject.TextDefinition.Font.MeasureString(textObject.TextDefinition.Text);
                    blockSize.Width = Math.Max(blockSize.Width, textSize.Width);
                    blockSize.Height += textSize.Height;
                }

                return blockSize;
            }
        }
        #endregion

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="buildInfoStore">An object responsible for providing information about a build.</param>
        /// <param name="textBuilder">An object responsible for building ITextObject objects from a format string.</param>
        /// <param name="textColour">The colour of the overlay text. Of not specified, white with an alpha of 0.25 will be used.</param>
        public BuildInfoOverlayLayerModel(IBuildInfoStore buildInfoStore, ITextObjectBuilder textBuilder, Color? textColour = null)
        {
            if (buildInfoStore == null || textBuilder == null)
                throw new ArgumentNullException();

            if (textColour == null)
                textColour = new Color(255, 255, 255, 64);

            StringBuilder overlayText = new StringBuilder();
            overlayText.Append($"{{Alignment:Centre}}{{C:{textColour.Value.R.ToString()}, {textColour.Value.G.ToString()}, {textColour.Value.B.ToString()}, {textColour.Value.A.ToString()}}}");
            overlayText.AppendLine(buildInfoStore.Title + $" ({buildInfoStore.Platform})");
            overlayText.AppendLine(buildInfoStore.Version + " - " + buildInfoStore.Date);
            overlayText.AppendLine("DO NOT DISTRIBUTE");

            this.TextObjects.AddRange(textBuilder.Build(overlayText.ToString(), new RectangleF(Vector2.Zero, new SizeF(1, 1))));

            // We centred on a 1 pixel boundary, so now find out how wide the text is and shift all objects over.
            // This puts our text in alignment with the default value of TextAnchor (0, 0).
            Vector2 offset = new Vector2(this.TextSize.Width / 2f, 0);
            foreach (ITextObject textObject in this.TextObjects)
                textObject.Location += offset;
        }
    }
}
