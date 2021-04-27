using System.Diagnostics;
using System.Windows.Forms;

namespace MetroFramework
{
     
    /// Message box overlay display properties.
     
    public class MetroMessageBoxProperties
    {
         
        /// Creates a new instance of MessageBoxOverlayProperties.
         
        /// <param name="owner"></param>
        public MetroMessageBoxProperties(MetroMessageBoxControl owner)
        { _owner = owner; }

         
        /// Gets or sets the message box buttons in the message box overlay.
         
        public MessageBoxButtons Buttons
        { get; set; }

         
        /// Gets or sets the message box default button.
         
        public MessageBoxDefaultButton DefaultButton
        { get; set; }

         
        /// Gets or sets the message box overlay icon.
         
        public MessageBoxIcon Icon
        { get; set; }

         
        /// Gets or sets the message box overlay message contents.
         
        public string Message
        { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MetroMessageBoxControl _owner = null;

         
        /// Gets the property owner.
         
        public MetroMessageBoxControl Owner
        { get { return _owner; } }

         
        /// Gets or sets the message box overlat title.
         
        public string Title
        { get; set; }

    }
}
