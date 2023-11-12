using ACE.WhatIf;

namespace ACE.Extensions
{
    internal static class WhatIfChangeExtensions
    {
        public static WhatIfAfterBeforeChange? GetChange(this WhatIfChange change)
        {
            return change.after ?? change.before;
        }
    }
}
