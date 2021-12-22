// Copyright (c) Intangible Realities Lab. All rights reserved.
// Licensed under the GPL. See License.txt in the project root for license information.

using System;
using Narupa.Visualisation.Properties;
using Narupa.Visualisation.Property;
using Plugins.Narupa.Core;

namespace Narupa.Visualisation.Node.Input
{
    /// <summary>
    /// Input for the visualisation system that provides a <see cref="IMapping{TFrom,TTo}" /> value.
    /// </summary>
    [Serializable]
    public class ElementColorMappingInputNode : InputNode<ElementColorMappingProperty>
    {
    }
}