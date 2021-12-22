using System;
using Narupa.Visualisation.Property;
using Plugins.Narupa.Core.Science;

namespace Narupa.Visualisation.Properties.Collections
{
    /// <summary>
    /// Serializable <see cref="Property" /> for an array of <see cref="Element" />
    /// values.
    /// </summary>
    [Serializable]
    public class ElementArrayProperty : ArrayProperty<Element>
    {
    }
}