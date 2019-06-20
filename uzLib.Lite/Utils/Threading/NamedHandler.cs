using System;

namespace uzLib.Lite.Utils.Threading
{
    public class NamedHandler<TArg>
    {
        public readonly Func<string, TArg> Handler;

        public NamedHandler(Func<string, TArg> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Handler = arg =>
            {
                using (new ThreadMarker(arg))
                {
                    return handler(arg);
                }
            };
        }
    }
}