using System;
using System.ComponentModel;

namespace Basic_CSOM.Entities.Enums
{
    public enum StateEnum
    {
        [DefaultValue("Signed")]
        Signed = 0,
        [DefaultValue("Design")]
        Design = 1,
        [DefaultValue("Development")]
        Development = 2,
        [DefaultValue("Maintenance")]
        Maintenance = 3,
        [DefaultValue("Closed")]
        Closed = 4

    }
}
