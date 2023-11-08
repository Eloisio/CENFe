/**
 * A Professional HTML Renderer You Will Use
 * 
 * The BSD License (BSD)
 * Copyright (c) 2011 Jose Menendez Póo, http://www.codeproject.com/Articles/32376/A-Professional-HTML-Renderer-You-Will-Use
 * 
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
 * BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
 * THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System.Drawing;

namespace MetroFramework.Drawing.Html
{

    /// Represents a word inside an inline box

    /// <remarks>
    /// Because of performance, words of text are the most atomic 
    /// element in the project. It should be characters, but come on,
    /// imagine the performance when drawing char by char on the device.
    /// 
    /// It may change for future versions of the library
    /// </remarks>
    internal class CssBoxWord
        : CssRectangle
    {
        #region Fields


        private string _word;
        private PointF _lastMeasureOffset;
        private CssBox _ownerBox;
        private Image _image;


        #endregion

        #region Ctor

        internal CssBoxWord(CssBox owner)
        {
            _ownerBox = owner;
            _word = string.Empty;
        }


        /// Creates a new BoxWord which represents an image

        /// <param name="owner"></param>
        /// <param name="image"></param>
        public CssBoxWord(CssBox owner, Image image)
            : this(owner)
        {
            Image = image;
        }

        #endregion

        #region Properties


        /// Gets the width of the word including white-spaces

        public float FullWidth
        {
            //get { return OwnerBox.ActualWordSpacing * (SpacesBefore + SpacesAfter) + Width; }
            get { return Width; }
        }


        /// Gets the image this words represents (if one)

        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;

                if (value != null)
                {
                    CssLength w = new CssLength(OwnerBox.Width);
                    CssLength h = new CssLength(OwnerBox.Height);
                    if (w.Number > 0 && w.Unit == CssLength.CssUnit.Pixels)
                    {
                        Width = w.Number;
                    }
                    else
                    {
                        Width = value.Width;
                    }

                    if (h.Number > 0 && h.Unit == CssLength.CssUnit.Pixels)
                    {

                        Height = h.Number;
                    }
                    else
                    {
                        Height = value.Height;
                    }

                    Height += OwnerBox.ActualBorderBottomWidth + OwnerBox.ActualBorderTopWidth + OwnerBox.ActualPaddingTop + OwnerBox.ActualPaddingBottom;

                }
            }
        }


        /// Gets if the word represents an image.

        public bool IsImage
        {
            get { return Image != null; }
        }


        /// Gets a bool indicating if this word is composed only by spaces.
        /// Spaces include tabs and line breaks

        public bool IsSpaces
        {
            get { return string.IsNullOrEmpty(Text.Trim()); }
        }


        /// Gets if the word is composed by only a line break

        public bool IsLineBreak
        {
            get { return Text == "\n"; }
        }


        /// Gets if the word is composed by only a tab

        public bool IsTab
        {
            get { return Text == "\t"; }
        }


        /// Gets the Box where this word belongs.

        public CssBox OwnerBox
        {
            get { return _ownerBox; }
        }


        /// Gets the text of the word

        public string Text
        {
            get { return _word; }
        }


        /// Gets or sets an offset to be considered in measurements

        internal PointF LastMeasureOffset
        {
            get { return _lastMeasureOffset; }
            set { _lastMeasureOffset = value; }
        }

        #endregion

        #region Methods


        /// Removes line breaks and tabs on the text of the word,
        /// replacing them with white spaces

        internal void ReplaceLineBreaksAndTabs()
        {
            _word = _word.Replace('\n', ' ');
            _word = _word.Replace('\t', ' ');
        }


        /// Appends the specified char to the word's text

        /// <param name="c"></param>
        internal void AppendChar(char c)
        {
            _word += c;
        }


        /// Represents this word for debugging purposes

        /// <returns></returns>
        public override string ToString()
        {

            return string.Format("{0} ({1} char{2})", Text.Replace(' ', '-').Replace("\n", "\\n"), Text.Length, Text.Length != 1 ? "s" : string.Empty);
        }

        #endregion
    }
}
