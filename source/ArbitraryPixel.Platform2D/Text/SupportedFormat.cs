namespace ArbitraryPixel.Platform2D.Text
{
    /// <summary>
    /// The supported format names that ITextFormatProcessor can support.
    /// </summary>
    public enum SupportedFormat
    {
        /// <summary>
        /// Set a colour.
        /// </summary>
        Colour,

        /// <summary>
        /// Set TimePerCharacter.
        /// </summary>
        TimePerCharacter,

        /// <summary>
        /// Set the font name.
        /// </summary>
        FontName,

        /// <summary>
        /// Set the alignment of the current text line.
        /// </summary>
        LineAlignment,
    }
}
