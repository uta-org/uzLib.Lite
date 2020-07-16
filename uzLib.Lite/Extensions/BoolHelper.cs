using System.Collections.Generic;

namespace UnityEngine.Extensions
{
    public static class BoolHelper
    {
        public static bool All(this IEnumerable<bool> bools)
        {
            foreach (var @bool in bools)
                if (!@bool)
                    return false;

            return true;
        }
    }
}