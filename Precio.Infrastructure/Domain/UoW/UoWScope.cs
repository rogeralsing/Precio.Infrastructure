using System;

namespace Precio.Domain
{
    public static partial class UoW
    {
        #region Nested type: UoWExistingScope

        public sealed class UoWExistingScope : UoWScope
        {
            public override void Dispose()
            {
            }
        }

        #endregion

        #region Nested type: UoWNewScope

        public sealed class UoWNewScope : UoWScope
        {
            public override void Dispose()
            {
                End();
            }
        }

        #endregion

        #region Nested type: UoWScope

        public abstract class UoWScope : IDisposable
        {
            #region IDisposable Members

            public abstract void Dispose();

            #endregion
        }

        #endregion
    }
}