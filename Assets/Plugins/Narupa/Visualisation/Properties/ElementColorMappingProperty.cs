using System;
using Narupa.Visualisation.Node.Color;
using Narupa.Visualisation.Property;
using Plugins.Narupa.Core;
using Plugins.Narupa.Core.Science;
using UnityEngine;

namespace Narupa.Visualisation.Properties
{
    /// <summary>
    /// Serializable <see cref="Property" /> for a <see cref="ElementColorMapping" />
    /// value.
    /// </summary>
    [Serializable]
    public class ElementColorMappingProperty : InterfaceProperty<IMapping<Element, Color>>
    {
    }
}