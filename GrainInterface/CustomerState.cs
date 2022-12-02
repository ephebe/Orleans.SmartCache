using System;
using System.Collections.Generic;
using System.Text;

namespace GrainInterface;

[GenerateSerializer,Immutable]
public sealed record CustomerState
{
    [Id(0)]
    public Guid Id { get; set; }
    [Id(1)]
    public string Name { get; set; }
}
